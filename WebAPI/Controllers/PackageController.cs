using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageRepository _packageRepository;
        private readonly IStudentRepository _studentRepository;

        public PackageController(ILogger<PackageController> logger, IPackageRepository packageRepository, IStudentRepository studentRepository)
        {
            _logger = logger;
            _packageRepository = packageRepository;
            _studentRepository = studentRepository;
        }

        //Alle pakketten ophalen
        [HttpGet(Name = "GetAllPackages")]
        public IEnumerable<Package> Get()
        {
            return _packageRepository.GetAllPackagesGraphQL().ToArray();
        }


        //Reservatie maken
        [HttpPost(Name = "PostNewReservation")]
        public IActionResult CreateReservation([FromBody] CreateReservationModel createReservation)
        {
            var student = _studentRepository.GetByEmailAddress(createReservation.EmailAddress);
            var package = _packageRepository.GetById(createReservation.PackageId);

            //Heeft de student reserveringen?
            if (student.Packages != null)
            {
                foreach (var p in student.Packages)
                {
                    //Is de (al bestaande) reserving op dezelfde dag als het ophaalmoment van het pakket
                    if (p.PickupMoment.Year.Equals(package.PickupMoment.Year) && p.PickupMoment.Month.Equals(package.PickupMoment.Month) && p.PickupMoment.Day.Equals(package.PickupMoment.Day))
                    {
                        return StatusCode(400, new { status = 400, message = "This student already has a reservation that day." });
                    }
                }
            }

            //Checkt of het pakket alcohol bevat
            if (package.IsOver18)
            {
                //Checkt of de student 18 jaar is tijdens het ophalen van het pakket 
                //Jonger -> getal > 0 -> mag niet reserveren
                //Ouder -> getal < 0
                //Precies18 -> getal = 0
                if (DateTime.Compare(student.Birthday.AddYears(18), package.PickupMoment) > 0)
                {
                    return StatusCode(400, new { status = 400, message = "Package contains alocohol and the student is under 18 at the pickupmoment." });
                }
            }

            //Checkt of het pakket al een reservering heeft
            if (package.Reservation != null)
            {
                return StatusCode(400, new { status = 400, message = "There is already an other reservation on this package." });
            }

            package.Reservation = student;

            _packageRepository.Update(package);

            return Ok(new {status = 200, message = "Reservation created"});
        }
    }
}
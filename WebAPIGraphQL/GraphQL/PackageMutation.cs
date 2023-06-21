using Core.DomainServices;

namespace WebAPIGraphQL.GraphQL
{
    public class PackageMutation
    {
        public ReservationPayload CreateReservation([Service] IPackageRepository packageRepository, [Service] IStudentRepository studentRepository, ReservationInput reservationInput)
        {
            var student = studentRepository.GetByEmailAddress(reservationInput.EmailAddress);
            var package = packageRepository.GetById(reservationInput.PackageId);

            //Heeft de student reserveringen?
            if (student.Packages != null)
            {
                foreach (var p in student.Packages)
                {
                    //Is de (al bestaande) reserving op dezelfde dag als het ophaalmoment van het pakket
                    if (p.PickupMoment.Year.Equals(package.PickupMoment.Year) && p.PickupMoment.Month.Equals(package.PickupMoment.Month) && p.PickupMoment.Day.Equals(package.PickupMoment.Day))
                    {
                        return new ReservationPayload { Error = "This student already has a reservation that day." };
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
                    return new ReservationPayload { Error = "Package contains alocohol and the student is under 18 at the pickupmoment." };
                }
            }

            //Checkt of het pakket al een reservering heeft
            if (package.Reservation != null)
            {
                return new ReservationPayload { Error = "There is already an other reservation on this package." };
            }

            package.Reservation = student;

            packageRepository.Update(package);

            return new ReservationPayload { package = package };
        }

    }
}

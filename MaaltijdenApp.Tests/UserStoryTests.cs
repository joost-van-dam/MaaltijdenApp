using Moq;
using Core.Domain;
using Core.DomainServices;
using MaaltijdenApp.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MaaltijdenApp.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MaaltijdenApp.Tests
{
    public class UserStory0Tests
    {
        [Fact]
        public async void PackageController_Should_Return_ViewModel_With_All_Available_Packages_To_A_Student()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Peer", ContainsAlcohol = false };
            var product3 = new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false };

            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            //await roleManagerMock.Object.CreateAsync(new IdentityRole("Student"));

            var student1 = new IdentityUser("TestStudentJoost");
            await userManagerMock.Object.CreateAsync(student1, "Henk123$");
            await userManagerMock.Object.SetEmailAsync(student1, "teststudentjoost@avans.nl");
            await userManagerMock.Object.AddToRoleAsync(student1, "Student");

            // system under test
            var sut = new PackageController(
                loggerMock.Object, 
                userManagerMock.Object, 
                packageRepositoryMock.Object, 
                productRepositoryMock.Object, 
                studentRepositoryMock.Object, 
                canteenRepositoryMock.Object, 
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            packageRepositoryMock.Setup(packageRepository => packageRepository.Get()).Returns(

                new List<Package>
                {
                    new Package
                    {
                        Id = 1,
                        Name = "Fruitmandje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 2,
                        Name = "Fruitschaaltje",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },
                }
            );

            // Act
            var result = await sut.OverviewStudent() as ViewResult;

            // Assert
            var assortmentStudentViewModel = result.Model as AssortmentStudentViewModel;
            Assert.Equal(2, assortmentStudentViewModel?.NotReservedAssortment?.Count);
        }

        [Fact]
        public async void PackageController_Should_Return_ViewModel_With_All_Reserved_Packages_Of_The_Student()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Peer", ContainsAlcohol = false };
            var product3 = new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false };

            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            //await roleManagerMock.Object.CreateAsync(new IdentityRole("Student"));
            var studentWithReservation = new Student
            {
                Name = "TestStudentJoost",
                EmailAddress = "teststudentjoost@avans.nl",
                Birthday = DateTime.Now.AddYears(-21),
                PhoneNumber = "0611223344",
                City = City.Breda
            };

            var student1 = new IdentityUser
            {
                UserName = "TestStudentJoost",
                Email = "teststudentjoost@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            await userManagerMock.Object.CreateAsync(student1, "Henk123$");
            await userManagerMock.Object.SetEmailAsync(student1, "teststudentjoost@avans.nl");
            await userManagerMock.Object.AddToRoleAsync(student1, "Student");

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(student1));

            packageRepositoryMock.Setup(packageRepository => packageRepository.Get()).Returns(
                new List<Package>
                {
                    new Package
                    {
                        Id = 1,
                        Name = "Fruitmandje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                        Reservation = studentWithReservation,
                    },

                    new Package
                    {
                        Id = 2,
                        Name = "Fruitschaaltje",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 3,
                        Name = "Bananensmoothie",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Drank,
                        Reservation = studentWithReservation,
                    },
                }
            );

            // Act
            var result = await sut.ReservedOverviewStudent() as ViewResult;

            // Assert
            var assortmentStudentViewModel = result.Model as AssortmentStudentViewModel;
            Assert.Equal(2, assortmentStudentViewModel?.ReservedAssortment?.Count);
        }

    }

    public class UserStory02Tests
    {
        [Fact]
        public async void PackageController_Should_Return_ViewModel_With_All_Packages_Of_The_Employees_Own_Canteen_Sorted_By_PickupMoment()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Peer", ContainsAlcohol = false };
            var product3 = new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false };

            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            //await roleManagerMock.Object.CreateAsync(new IdentityRole("Student"));

            var employeeOfCanteen = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employee1 = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            await userManagerMock.Object.CreateAsync(employee1, "Henk123$");
            await userManagerMock.Object.SetEmailAsync(employee1, "testemployeethijs@avans.nl");
            await userManagerMock.Object.AddToRoleAsync(employee1, "Employee");

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employee1));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employee1.Email)).Returns(employeeOfCanteen);


            // Lijst met pakketten die uit de database zouden moeten komen
            List<Package> packages = new List<Package>
                {
                    new Package
                    {
                        Id = 1,
                        Name = "Fruitmandje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now.AddHours(3),
                        PickupClosingTime = DateTime.Now.AddHours(6),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 2,
                        Name = "Fruitschaaltje",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(3),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 3,
                        Name = "Bananensmoothie",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now.AddHours(4),
                        PickupClosingTime = DateTime.Now.AddHours(7),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 4,
                        Name = "Fruitzakje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now.AddHours(3),
                        PickupClosingTime = DateTime.Now.AddHours(6),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 5,
                        Name = "Bakje bananen",
                        Products = new List<Product> {product2},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now.AddHours(2),
                        PickupClosingTime = DateTime.Now.AddHours(3),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 6,
                        Name = "Bananensmoothie",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(7),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Drank,
                    },
                };

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());

            // Act
            var result = await sut.OverviewEmployee() as ViewResult;

            // Assert
            //Lijst vergelijken voor links in Assert.Equal
            List<Package> sortedPackagesOnPickupMomentAndOwnCanteen = packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList();

            var assortmentEmployeeViewModel = result.Model as AssortmentEmployeeViewModel;
            Assert.Equal(3, assortmentEmployeeViewModel?.OwnAssortment.Count);
            Assert.Equal(sortedPackagesOnPickupMomentAndOwnCanteen, assortmentEmployeeViewModel?.OwnAssortment);
        }

        [Fact]
        public async void PackageController_Should_Return_ViewModel_With_All_Packages_Of_The_Other_Canteens_And_Not_Of_The_Employees_Own_Canteen_Sorted_By_PickupMoment()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Peer", ContainsAlcohol = false };
            var product3 = new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false };

            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>();
            //var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            //await roleManagerMock.Object.CreateAsync(new IdentityRole("Student"));

            var employeeOfCanteen = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employee1 = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            await userManagerMock.Object.CreateAsync(employee1, "Henk123$");
            await userManagerMock.Object.SetEmailAsync(employee1, "testemployeethijs@avans.nl");
            await userManagerMock.Object.AddToRoleAsync(employee1, "Employee");

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employee1));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employee1.Email)).Returns(employeeOfCanteen);


            // Lijst met pakketten die uit de database zouden moeten komen
            List<Package> packages = new List<Package>
                {
                    new Package
                    {
                        Id = 1,
                        Name = "Fruitmandje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now.AddHours(3),
                        PickupClosingTime = DateTime.Now.AddHours(6),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 2,
                        Name = "Fruitschaaltje",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(3),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 3,
                        Name = "Bananensmoothie",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen1,
                        PickupMoment = DateTime.Now.AddHours(4),
                        PickupClosingTime = DateTime.Now.AddHours(7),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 4,
                        Name = "Fruitzakje",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now.AddHours(3),
                        PickupClosingTime = DateTime.Now.AddHours(6),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 5,
                        Name = "Bakje bananen",
                        Products = new List<Product> {product2},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now.AddHours(2),
                        PickupClosingTime = DateTime.Now.AddHours(3),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Bread,
                    },

                    new Package
                    {
                        Id = 6,
                        Name = "Bananensmoothie",
                        Products = new List<Product> {product3},
                        City = City.Breda,
                        Canteen = canteen2,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(7),
                        IsOver18 = false,
                        TypeOfMeal = TypeOfMeal.Drank,
                    },
                };

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetOtherByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen != employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());

            // Act
            var result = await sut.OverviewEmployee() as ViewResult;

            // Assert
            //Lijst vergelijken voor links in Assert.Equal
            List<Package> sortedPackagesOnPickupMomentAndOtherCanteens = packages.Where(p => p.Canteen != employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList();

            var assortmentEmployeeViewModel = result.Model as AssortmentEmployeeViewModel;
            Assert.Equal(3, assortmentEmployeeViewModel?.OtherCanteensAssortment.Count);
            Assert.Equal(sortedPackagesOnPickupMomentAndOtherCanteens, assortmentEmployeeViewModel?.OtherCanteensAssortment);
        }
    }

    public class UserStory03Tests
    {
        [Fact]
        public async void Employee_Should_Be_Able_To_Create_A_Package()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Appel", ContainsAlcohol = false },
                new Product { Id = 2, Name = "Peer", ContainsAlcohol = false },
                new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false },
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            productRepositoryMock.Setup(productRepository => productRepository.Get(new List<int> { 1, 2, 3 })).Returns(listWithProducts);
            //_productRepository.Get
            //packageRepositoryMock.Setup(packageRepository => packageRepository.GetByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());

            var newPackageInCreatePackageViewModel = new CreatePackageViewModel
            {
                Name = "Brood",
                idsOfPickedProducts = new List<int> { 1, 2, 3 },
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            // Act
            var result = await sut.Create(newPackageInCreatePackageViewModel) as RedirectToActionResult;

            // Assert
            Assert.Equal("OverviewEmployee", result.ActionName);
        }

        [Fact]
        public async void Employee_Should_Be_Able_To_Update_A_Package()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Appel", ContainsAlcohol = false },
                new Product { Id = 2, Name = "Peer", ContainsAlcohol = false },
                new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false },
            };

            var oldPackageWithoutReservation = new Package
            {
                Name = "Brood",
                Products = listWithProducts,
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            productRepositoryMock.Setup(productRepository => productRepository.Get(new List<int> { 1, 2, 3 })).Returns(listWithProducts);
            //_productRepository.Get
            //packageRepositoryMock.Setup(packageRepository => packageRepository.GetByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());
            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(1)).Returns(oldPackageWithoutReservation);

            var updatedPackageInUpdatePackageViewModel = new UpdatePackageViewModel
            {
                Id = 1,
                Name = "Brood",
                idsOfPickedProducts = new List<int> { 1, 2, 3 },
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            // Act
            var result = await sut.Update(updatedPackageInUpdatePackageViewModel) as RedirectToActionResult;

            // Assert
            //Checkt of de tempdata leeg is, deze is vol als er al een reservering op het pakket zit
            Assert.Null(sut.TempData["PackageIsReserved"]);
            Assert.Equal("OverviewEmployee", result.ActionName);
        }

        [Fact]
        public void Employee_Should_Be_Able_To_Delete_A_Package()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            int id = 1;

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Appel", ContainsAlcohol = false },
                new Product { Id = 2, Name = "Peer", ContainsAlcohol = false },
                new Product { Id = 3, Name = "Banaan", ContainsAlcohol = false },
            };

            var oldPackageWithoutReservation = new Package
            {
                Name = "Brood",
                Products = listWithProducts,
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            //Mock het ophalen van een pakket om vervolgens te checken of deze een reservering heeft
            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(oldPackageWithoutReservation);

            //Mockt de remove functie in de pakketenrepository maar is niet nodig want er zit geen return op
            //packageRepositoryMock.Setup(packageRepository => packageRepository.Remove(id));

            // Act
            var result = sut.Delete(id) as RedirectToActionResult;

            // Assert
            //Checkt of de tempdata leeg is, deze is vol als er al een reservering op het pakket zit
            Assert.Null(sut.TempData["PackageIsReserved"]);
            Assert.Equal("OverviewEmployee", result.ActionName);
        }
    }

    public class UserStory04Tests
    {
        //[Fact]
        //public void A_Product_Can_Contain_Alcohol()
        //{
        //    // Arrange
        //    // Act
        //    var result = new Product { Id = 1, Name = "Wijn", ContainsAlcohol = true };
        //    // Assert
        //    Assert.True(result.ContainsAlcohol);
        //}

        [Fact]
        public async void Package_Should_Be_Over_18_If_Any_Product_Contains_Alcohol()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Appel", ContainsAlcohol = false },
                new Product { Id = 2, Name = "Peer", ContainsAlcohol = false },
                new Product { Id = 3, Name = "Wijn", ContainsAlcohol = true },
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            productRepositoryMock.Setup(productRepository => productRepository.Get(new List<int> { 1, 2, 3 })).Returns(listWithProducts);
            //_productRepository.Get
            //packageRepositoryMock.Setup(packageRepository => packageRepository.GetByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());

            var newPackageInCreatePackageViewModel = new CreatePackageViewModel
            {
                Name = "Brood",
                idsOfPickedProducts = new List<int> { 1, 2, 3 },
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            // Act
            var result = await sut.Create(newPackageInCreatePackageViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(sut.TempData["PackageContainsAlcohol"]);
            Assert.Equal("Dit pakket bevat een product met alcohol", sut.TempData["PackageContainsAlcohol"]);
        }

        [Fact]
        public async void Student_Under_18_Can_Not_Reserve_A_Package_With_Contains_Alcohol()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 1, Name = "Wijn", ContainsAlcohol = true };

            var studentWithNoReservationThatDay = new Student
            {
                Name = "TestMinderjarigeStudentHenk",
                EmailAddress = "teststudenthenk@avans.nl",
                Birthday = DateTime.Now.AddYears(-17),
                PhoneNumber = "0611223344",
                City = City.Breda
            };

            var student1 = new IdentityUser
            {
                UserName = "TestMinderjarigeStudentHenk",
                Email = "teststudenthenk@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(student1));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(student1.Email)).Returns(studentWithNoReservationThatDay);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                    {
                        Name = "Fruit met Wijn",
                        Products = new List<Product> {product1, product2},
                        City = City.Breda,
                        Canteen = canteen,
                        PickupMoment = DateTime.Now,
                        PickupClosingTime = DateTime.Now.AddHours(4),
                        IsOver18 = true,
                        TypeOfMeal = TypeOfMeal.Bread,
                    }
            );;

            // Act
            var result = await sut.DetailStudent(id) as ViewResult;

            // Assert
            var packageDetailStudentViewModel = result.Model as PackageDetailStudentViewModel;
            Assert.False(packageDetailStudentViewModel.AllowedToReserve);
        }
    }

    public class UserStory05Tests
    {
        [Fact]
        public async void Student_Over_18_Can_Reserve_A_Package_With_Contains_Alcohol()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 1, Name = "Wijn", ContainsAlcohol = true };

            var studentWithNoReservationThatDay = new Student
            {
                Name = "TestMeerderjarigeStudentHenk",
                EmailAddress = "teststudenthans@avans.nl",
                Birthday = DateTime.Now.AddYears(-19),
                PhoneNumber = "0611223344",
                City = City.Breda
            };

            var student1 = new IdentityUser
            {
                UserName = "TestMeerderjarigeStudentHenk",
                Email = "teststudenthans@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(student1));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(student1.Email)).Returns(studentWithNoReservationThatDay);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                {
                    Name = "Fruit met Wijn",
                    Products = new List<Product> { product1, product2 },
                    City = City.Breda,
                    Canteen = canteen,
                    PickupMoment = DateTime.Now,
                    PickupClosingTime = DateTime.Now.AddHours(4),
                    IsOver18 = true,
                    TypeOfMeal = TypeOfMeal.Bread,
                }
            ); ;

            // Act
            var result = await sut.DetailStudent(id) as ViewResult;

            // Assert
            var packageDetailStudentViewModel = result.Model as PackageDetailStudentViewModel;
            Assert.True(packageDetailStudentViewModel.AllowedToReserve);
        }

        [Fact]
        public async void Student_Is_Not_Able_To_Reserve_More_Than_1_Package_Per_PickupDay()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Wijn", ContainsAlcohol = true };
            var product3 = new Product { Id = 3, Name = "Koekjes", ContainsAlcohol = false };

            var alreadyReservedPackage = new Package
            {
                Name = "Zakje koekjes",
                Products = new List<Product> { product3 },
                City = City.Breda,
                Canteen = canteen,
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                IsOver18 = false,
                TypeOfMeal = TypeOfMeal.Bread,
            };

            var studentWithAThatDay = new Student
            {
                Name = "TestMeerderjarigeStudentHenk",
                EmailAddress = "teststudenthans@avans.nl",
                Birthday = DateTime.Now.AddYears(-19),
                PhoneNumber = "0611223344",
                City = City.Breda,
                Packages = new List<Package> { alreadyReservedPackage }
            };

            var student1 = new IdentityUser
            {
                UserName = "TestMeerderjarigeStudentHenk",
                Email = "teststudenthans@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(student1));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(student1.Email)).Returns(studentWithAThatDay);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                {
                    Name = "Fruit met Wijn",
                    Products = new List<Product> { product1, product2 },
                    City = City.Breda,
                    Canteen = canteen,
                    PickupMoment = DateTime.Now,
                    PickupClosingTime = DateTime.Now.AddHours(4),
                    IsOver18 = true,
                    TypeOfMeal = TypeOfMeal.Bread,
                }
            ); ;

            // Act
            var result = await sut.DetailStudent(id) as ViewResult;

            // Assert
            var packageDetailStudentViewModel = result.Model as PackageDetailStudentViewModel;
            Assert.False(packageDetailStudentViewModel.AllowedToReserve);
        }
    }

    public class UserStory06Tests
    {
        [Fact]
        public async void Student_Is_Able_The_Product_Of_A_Package_In_The_Detail_Screen()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Wijn", ContainsAlcohol = true };
            var product3 = new Product { Id = 3, Name = "Koekjes", ContainsAlcohol = false };

            var studentWithAThatDay = new Student
            {
                Name = "TestMeerderjarigeStudentHenk",
                EmailAddress = "teststudenthans@avans.nl",
                Birthday = DateTime.Now.AddYears(-19),
                PhoneNumber = "0611223344",
                City = City.Breda,
            };

            var student1 = new IdentityUser
            {
                UserName = "TestMeerderjarigeStudentHenk",
                Email = "teststudenthans@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(student1));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(student1.Email)).Returns(studentWithAThatDay);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                {
                    Name = "Fruit met Wijn",
                    Products = new List<Product> { product1, product2 },
                    City = City.Breda,
                    Canteen = canteen,
                    PickupMoment = DateTime.Now,
                    PickupClosingTime = DateTime.Now.AddHours(4),
                    IsOver18 = true,
                    TypeOfMeal = TypeOfMeal.Bread,
                }
            ); ;

            // Lijst met producten die in de viewmodel zouden moeten zitten
            var listWithProductsThatShouldBeInPackageDetailStudentViewModel = new List<Product> { product1, product2 };

            // Act
            var result = await sut.DetailStudent(id) as ViewResult;

            // Assert
            var packageDetailStudentViewModel = result.Model as PackageDetailStudentViewModel;
            Assert.Equal(listWithProductsThatShouldBeInPackageDetailStudentViewModel, packageDetailStudentViewModel.package.Products);
        }
    }


    public class UserStory07Tests 
    {
        //Zichtbaarheid knop op detail
        [Fact]
        public async void Student_Is_Not_Able_To_Reserve_An_Already_Reserved_Package()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Wijn", ContainsAlcohol = true };
            var product3 = new Product { Id = 3, Name = "Koekjes", ContainsAlcohol = false };

            var studentWithReservation = new Student
            {
                Name = "TestStudetMetReservatieGerrit",
                EmailAddress = "teststudentmetreservatiegerrit@avans.nl",
                Birthday = DateTime.Now.AddYears(-20),
                PhoneNumber = "0611335577",
                City = City.Breda,
            };

            var studentStudent = new Student
            {
                Name = "TestMeerderjarigeStudentHenk",
                EmailAddress = "teststudenthans@avans.nl",
                Birthday = DateTime.Now.AddYears(-19),
                PhoneNumber = "0611223344",
                City = City.Breda,
            };

            var studentUser = new IdentityUser
            {
                UserName = "TestMeerderjarigeStudentHenk",
                Email = "teststudenthans@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(studentUser));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(studentUser.Email)).Returns(studentStudent);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                {
                    Name = "Fruit met Wijn",
                    Products = new List<Product> { product1, product2 },
                    City = City.Breda,
                    Canteen = canteen,
                    PickupMoment = DateTime.Now,
                    PickupClosingTime = DateTime.Now.AddHours(4),
                    IsOver18 = true,
                    TypeOfMeal = TypeOfMeal.Bread,
                    Reservation = studentWithReservation,
                }
            ); ;

            // Act
            var result = await sut.DetailStudent(id) as ViewResult;

            // Assert
            var packageDetailStudentViewModel = result.Model as PackageDetailStudentViewModel;
            Assert.False(packageDetailStudentViewModel.AllowedToReserve);
        }

        //Op de .reserve(id) in de controller
        [Fact]
        public async void Student_Is_Not_Able_To_Reserve_An_Already_Reserved_Package_Changing_Routes()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var id = 1;

            var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var product1 = new Product { Id = 1, Name = "Appel", ContainsAlcohol = false };
            var product2 = new Product { Id = 2, Name = "Wijn", ContainsAlcohol = true };
            var product3 = new Product { Id = 3, Name = "Koekjes", ContainsAlcohol = false };

            var studentWithReservation = new Student
            {
                Name = "TestStudetMetReservatieGerrit",
                EmailAddress = "teststudentmetreservatiegerrit@avans.nl",
                Birthday = DateTime.Now.AddYears(-20),
                PhoneNumber = "0611335577",
                City = City.Breda,
            };

            var studentStudent = new Student
            {
                Name = "TestMeerderjarigeStudentHenk",
                EmailAddress = "teststudenthans@avans.nl",
                Birthday = DateTime.Now.AddYears(-19),
                PhoneNumber = "0611223344",
                City = City.Breda,
            };

            var studentUser = new IdentityUser
            {
                UserName = "TestMeerderjarigeStudentHenk",
                Email = "teststudenthans@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(studentUser));

            studentRepositoryMock.Setup(studentRepository => studentRepository.GetByEmailAddress(studentUser.Email)).Returns(studentStudent);

            packageRepositoryMock.Setup(packageRepository => packageRepository.GetById(id)).Returns(

                new Package
                {
                    Name = "Fruit met Wijn",
                    Products = new List<Product> { product1, product2 },
                    City = City.Breda,
                    Canteen = canteen,
                    PickupMoment = DateTime.Now,
                    PickupClosingTime = DateTime.Now.AddHours(4),
                    IsOver18 = true,
                    TypeOfMeal = TypeOfMeal.Bread,
                    Reservation = studentWithReservation,
                }
            ); ;

            // Act
            var result = await sut.Reserve(id) as RedirectToActionResult;

            // Assert
            Assert.Equal("Sorry, er zit een reservatie op dit pakket.", sut.TempData["PackageIsReserved"]);
            Assert.Equal("OverviewStudent", result.ActionName);
        }
    }

    public class UserStory09Tests
    {
        //[Fact]
        //public void A_Package_Has_An_Indication_If_It_Is_A_Warm_Meal()
        //{
        //    // Arrange
        //    var product1 = new Product { Id = 1, Name = "Warme tomatensoep", ContainsAlcohol = false };
        //    var canteen = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            
        //    // Act
        //    var result = new Package
        //    {
        //        Name = "Warme tomatensoep",
        //        Products = new List<Product> { product1 },
        //        City = City.Breda,
        //        Canteen = canteen,
        //        PickupMoment = DateTime.Now,
        //        PickupClosingTime = DateTime.Now.AddHours(4),
        //        IsOver18 = true,
        //        TypeOfMeal = TypeOfMeal.WarmEveningMeal,
        //    };

        //    // Assert
        //    Assert.Equal(TypeOfMeal.WarmEveningMeal, result.TypeOfMeal);
        //}

        //[Fact]
        //public void In_An_Employee_Is_Stored_In_Which_Canteen_He_Works_And_If_That_Canteen_Serves_Warm_Meals()
        //{
        //    // Arrange
        //    var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            
        //    // Act
        //    var employee = new Employee
        //    {
        //        Name = "TestEmployeeThijs",
        //        EmailAddress = "testemployeethijs@avans.nl",
        //        EmployeeNumber = 12345,
        //        Canteen = canteen1,
        //    };

        //    // Assert
        //    Assert.True(employee.Canteen.ServesWarmMeals);
        //}

        [Fact]
        public async void Employee_Should_Only_Be_Able_To_Create_A_Warm_Meal_Package_If_His_Canteen_Allows_To_This_Time_It_Does()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Tomatensoep", ContainsAlcohol = false },
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen1,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            productRepositoryMock.Setup(productRepository => productRepository.Get(new List<int> {1})).Returns(listWithProducts);
            //_productRepository.Get
            //packageRepositoryMock.Setup(packageRepository => packageRepository.GetByCanteenSortedByDate(employeeOfCanteen.Canteen)).Returns(packages.Where(p => p.Canteen == employeeOfCanteen.Canteen).OrderBy(p => p.PickupMoment).ToList());

            var newPackageInCreatePackageViewModel = new CreatePackageViewModel
            {
                Name = "Tomatensoep",
                idsOfPickedProducts = new List<int> { 1 },
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.WarmEveningMeal,
            };

            // Act
            var result = await sut.Create(newPackageInCreatePackageViewModel) as RedirectToActionResult;

            // Assert
            Assert.Equal("OverviewEmployee", result.ActionName);
        }


        [Fact]
        public async void Employee_Should_Not_Be_Able_To_Create_A_Warm_Meal_Package_If_His_Canteen_Does_Not_Allow_To()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PackageController>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var packageRepositoryMock = new Mock<IPackageRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var studentRepositoryMock = new Mock<IStudentRepository>();
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var canteen1 = new Canteen { Id = 1, Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true };
            var canteen2 = new Canteen { Id = 2, Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false };

            var listWithProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Tomatensoep", ContainsAlcohol = false },
            };

            var employeeEmployee = new Employee
            {
                Name = "TestEmployeeThijs",
                EmailAddress = "testemployeethijs@avans.nl",
                EmployeeNumber = 12345,
                Canteen = canteen2,
            };

            var employeeUser = new IdentityUser
            {
                UserName = "TestEmployeeThijs",
                Email = "testemployeethijs@avans.nl",
                PasswordHash = "Henk123$qwertyuiasdfghjkzxcvbnm",
            };

            // system under test
            var sut = new PackageController(
                loggerMock.Object,
                userManagerMock.Object,
                packageRepositoryMock.Object,
                productRepositoryMock.Object,
                studentRepositoryMock.Object,
                canteenRepositoryMock.Object,
                employeeRepositoryMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "foo")
                    }))
                }
            };

            // Mock Tempdata
            sut.TempData = new TempDataDictionary(sut.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            userManagerMock.Setup(userManager => userManager.GetUserAsync(sut.ControllerContext.HttpContext.User)).Returns(Task.FromResult(employeeUser));

            // Mock employee uit database met employeerepository op email van user uit httpcontext
            employeeRepositoryMock.Setup(employeeRepository => employeeRepository.GetByEmailAddress(employeeUser.Email)).Returns(employeeEmployee);

            // Mock productrepostiry acties 
            productRepositoryMock.Setup(productRepository => productRepository.Get(new List<int> { 1 })).Returns(listWithProducts);
            productRepositoryMock.Setup(productRepository => productRepository.Get()).Returns(listWithProducts);

            var newPackageInCreatePackageViewModel = new CreatePackageViewModel
            {
                Name = "Tomatensoep",
                idsOfPickedProducts = new List<int> { 1 },
                PickupMoment = DateTime.Now,
                PickupClosingTime = DateTime.Now.AddHours(4),
                Price = (decimal)4.95,
                TypeOfMeal = TypeOfMeal.WarmEveningMeal,
            };

            // Act
            var result = await sut.Create(newPackageInCreatePackageViewModel) as ViewResult;

            // Assert
            var modelStateErrors = sut.ModelState.Select(x => x.Value.Errors).Where(x => x.Count > 0).ToList();
            Assert.Equal("Deze kantine verkoopt geen warme maaltijden", modelStateErrors[0][0].ErrorMessage);

            var createPackageViewModel = result.Model as CreatePackageViewModel;
            Assert.NotNull(createPackageViewModel);
        }
    }
}
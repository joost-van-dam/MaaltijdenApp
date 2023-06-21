
using Core.Domain;

namespace Infrastructure.Seeders
{
    public class DbSeeders
    {
        public static List<Employee> SeedEmpoyees(List<Canteen> canteens)
        {
            return new List<Employee> {
                new Employee { Name = "Thijs", EmailAddress = "thijs@avans.nl", EmployeeNumber = 12345, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() },
                new Employee { Name = "Frans", EmailAddress = "frans@avans.nl", EmployeeNumber = 98765, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() },
                new Employee { Name = "MedewerkerJohan", EmailAddress = "medewerkerjohan@avans.nl", EmployeeNumber = 34565, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() },
                new Employee { Name = "MedewerkerFrans", EmailAddress = "medewerkerfrans@avans.nl", EmployeeNumber = 34876, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() },
            };
        }

        public static List<Student> SeedStudents()
        {
            return new List<Student> {
                new Student { Name = "Joost", EmailAddress = "joost@avans.nl", Birthday = DateTime.Now.AddYears(-21), PhoneNumber = "0611223344", City = City.Breda},
                new Student { Name = "Henk", EmailAddress = "henk@avans.nl", Birthday = DateTime.Now.AddYears(-17), PhoneNumber = "0699887766", City = City.DenBosch},
                new Student { Name = "StudentJoost", EmailAddress = "studentjoost@avans.nl", Birthday = DateTime.Now.AddYears(-21), PhoneNumber = "0699554466", City = City.DenBosch},
                new Student { Name = "StudentHenk", EmailAddress = "studenthenk@avans.nl", Birthday = DateTime.Now.AddYears(-17), PhoneNumber = "0644557587", City = City.Breda},
                new Student { Name = "StudentGerrit", EmailAddress = "studentgerrit@avans.nl", Birthday = DateTime.Now.AddYears(-19), PhoneNumber = "0622117587", City = City.Breda},
                new Student { Name = "StudentJan", EmailAddress = "studentjan@avans.nl", Birthday = DateTime.Now.AddYears(-20), PhoneNumber = "0634657587", City = City.Breda},
            };
        }

        public static List<Product> SeedProducts()
        {
            return new List<Product>
            {
                new Product { Name = "Appel", ContainsAlcohol = false},
                new Product { Name = "Mandarijn", ContainsAlcohol = false},
                new Product { Name = "Druiven", ContainsAlcohol = false},
                new Product { Name = "Bier", ContainsAlcohol = true},
                new Product { Name = "Wijn", ContainsAlcohol = true},
                new Product { Name = "Stokbrood", ContainsAlcohol = false},
                new Product { Name = "Croissant", ContainsAlcohol = false},
                new Product { Name = "Pistolet", ContainsAlcohol = false},
                new Product { Name = "Yogurt", ContainsAlcohol = false},
                new Product { Name = "Peer", ContainsAlcohol = false},
                new Product { Name = "Melk", ContainsAlcohol = false},
                new Product { Name = "Kiwi", ContainsAlcohol = false},
                new Product { Name = "Friet", ContainsAlcohol = false},
                new Product { Name = "Frikandel", ContainsAlcohol = false},
                new Product { Name = "Krokent", ContainsAlcohol = false},
                new Product { Name = "Kipnugget", ContainsAlcohol = false},
                new Product { Name = "Aardbeien", ContainsAlcohol = false},
                new Product { Name = "Mango", ContainsAlcohol = false},
                new Product { Name = "Frambozen", ContainsAlcohol = false},
                new Product { Name = "Salade", ContainsAlcohol = false},
                new Product { Name = "Pizza", ContainsAlcohol = false},
                new Product { Name = "Tomatensoep", ContainsAlcohol = false},
                new Product { Name = "Noodlesoep", ContainsAlcohol = false},
                new Product { Name = "Eierkoek", ContainsAlcohol = false},
                new Product { Name = "Nootjes", ContainsAlcohol = false},
                new Product { Name = "Chocoladereep", ContainsAlcohol = false},
                new Product { Name = "Cola", ContainsAlcohol = false},
                new Product { Name = "Ice tea", ContainsAlcohol = false},
                new Product { Name = "Sinas", ContainsAlcohol = false},
                new Product { Name = "Water stil", ContainsAlcohol = false},
                new Product { Name = "Water bruisend", ContainsAlcohol = false},
                new Product { Name = "Koffie", ContainsAlcohol = false},
                new Product { Name = "Espresso", ContainsAlcohol = false},
                new Product { Name = "Capuccino", ContainsAlcohol = false},
                new Product { Name = "Chocolademelk", ContainsAlcohol = false},
            };
        }

        public static List<Canteen> SeedCanteens()
        {
            return new List<Canteen>
            {
                new Canteen { Name = "LA Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 61, ServesWarmMeals = true },
                new Canteen { Name = "LC Kantine", City = City.Breda, PostalCode = "4818 AJ", Street = "Lovensdijkstraat", HouseNumber = 63, ServesWarmMeals = false },
                new Canteen { Name = "HA Kantine", City = City.Breda, PostalCode = "4818 CR", Street = "Hogeschoollaan", HouseNumber = 1, ServesWarmMeals = true },
                new Canteen { Name = "HB Kantine", City = City.Breda, PostalCode = "4818 CR", Street = "Hogeschoollaan", HouseNumber = 1, ServesWarmMeals = false },
                new Canteen { Name = "BL Kantine", City = City.Breda, PostalCode = "4834 CR", Street = "Beukenlaan", HouseNumber = 1, ServesWarmMeals = false },
                new Canteen { Name = "HB Kantine", City = City.Tilburg, PostalCode = "5037 DA", Street = "Professor Cobbenhagenlaan", HouseNumber = 13, ServesWarmMeals = true },
                new Canteen { Name = "OB Kantine", City = City.DenBosch, PostalCode = "5223 DE", Street = "Onderwijsboulevard", HouseNumber = 215, ServesWarmMeals = true },
                new Canteen { Name = "HP Kantine", City = City.DenBosch, PostalCode = "5232 JE", Street = "Hervenplein", HouseNumber = 2, ServesWarmMeals = false },
                new Canteen { Name = "PW Kantine", City = City.DenBosch, PostalCode = "5223 AL", Street = "Parallelweg", HouseNumber = 64, ServesWarmMeals = false },
                new Canteen { Name = "ST Kantine", City = City.DenBosch, PostalCode = "5211 AP", Street = "Stationsplein", HouseNumber = 50, ServesWarmMeals = false },
            };
        }

        public static List<Package> SeedPackages(List<Product> products, List<Canteen> canteens)
        {
            return new List<Package>
            {
                //LA pakketten
                new Package 
                {
                    Name = "Fruitmandje", Products = products.Where(p => p.Name == "Appel" || p.Name == "Druiven").ToList(), 
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() , 
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },

                new Package
                {
                    Name = "Fruitschaaltje", Products = products.Where(p => p.Name == "Appel" || p.Name == "Mandarijn").ToList(), 
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() , 
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },

                new Package
                {
                    Name = "Drankpakket", Products = products.Where(p => p.Name == "Bier" || p.Name == "Wijn").ToList(), 
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() , 
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = true, TypeOfMeal = TypeOfMeal.Bread,
                },

                new Package
                {
                    Name = "Zakje brood", Products = products.Where(p => p.Name == "Stokbrood" || p.Name == "Croissant" || p.Name == "Pistolet").ToList(), 
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LA Kantine").First() , 
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },

                //LC pakketten
                new Package
                {
                    Name = "Mango framboos Yogurt", Products = products.Where(p => p.Name == "Mango" || p.Name == "Framboos" || p.Name == "Yogurt").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
                new Package
                {
                    Name = "Water pakket", Products = products.Where(p => p.Name == "Water stil" || p.Name == "Water bruisend").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
                new Package
                {
                    Name = "Koffie pakket", Products = products.Where(p => p.Name == "Koffie" || p.Name == "Espresso" || p.Name == "Capuccino").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
                new Package
                {
                    Name = "Pizza", Products = products.Where(p => p.Name == "Pizza").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
                new Package
                {
                    Name = "Soep", Products = products.Where(p => p.Name == "Tomatensoep").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
                new Package
                {
                    Name = "Chocolade", Products = products.Where(p => p.Name == "Chocoladereep" || p.Name == "Chocolamelk").ToList(),
                    City = City.Breda, Canteen = canteens.Where(c => c.Name == "LC Kantine").First() ,
                    PickupMoment = DateTime.Now, PickupClosingTime = DateTime.Now.AddHours(4), IsOver18 = false, TypeOfMeal = TypeOfMeal.Bread,
                },
            };
        }

        
    }
}

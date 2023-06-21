using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Seeders
{
    public class IdentitySeedData
    {

        private const string Employee1EmailAddress = "thijs@avans.nl";
        private const string Employee1Password = "Henk123$";

        private const string Employee2EmailAddress = "frans@avans.nl";
        private const string Employee2Password = "Henk123$";

        private const string Employee3EmailAddress = "medewerkerjohan@avans.nl";
        private const string Employee3Password = "Wachtwoord123$";

        private const string Employee4EmailAddress = "medewerkerfrans@avans.nl";
        private const string Employee4Password = "Wachtwoord123$";


        private const string Student1EmailAddress = "joost@avans.nl";
        private const string Student1Password = "Henk123$";

        private const string Student2EmailAddress = "henk@avans.nl";
        private const string Student2Password = "Henk123$";

        private const string Student3EmailAddress = "studentjoost@avans.nl";
        private const string Student3Password = "Wachtwoord123$";

        private const string Student4EmailAddress = "studenthenk@avans.nl";
        private const string Student4Password = "Wachtwoord123$";

        private const string Student5EmailAddress = "studentgerrit@avans.nl";
        private const string Student5Password = "Wachtwoord123$";

        private const string Student6EmailAddress = "studentjan@avans.nl";
        private const string Student6Password = "Wachtwoord123$";

        public static async Task EnsurePopulated(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            var employee1 = await userManager.FindByEmailAsync(Employee1EmailAddress);
            if (employee1 == null)
            {
                employee1 = new IdentityUser("Thijs");
                await userManager.CreateAsync(employee1, Employee1Password);
                await userManager.SetEmailAsync(employee1, Employee1EmailAddress);
                //await userManager.AddClaimAsync(employee1, new Claim("Employee", "true"));
                await userManager.AddToRoleAsync(employee1, "Employee");
            }

            var employee2 = await userManager.FindByEmailAsync(Employee2EmailAddress);
            if (employee2 == null)
            {
                employee2 = new IdentityUser("Frans");
                await userManager.CreateAsync(employee2, Employee2Password);
                await userManager.SetEmailAsync(employee2, Employee2EmailAddress);
                //await userManager.AddClaimAsync(employee2, new Claim("Employee", "true"));
                await userManager.AddToRoleAsync(employee2, "Employee");
            }

            var employee3 = await userManager.FindByEmailAsync(Employee3EmailAddress);
            if (employee3 == null)
            {
                employee3 = new IdentityUser("MedewerkerJohan");
                await userManager.CreateAsync(employee3, Employee3Password);
                await userManager.SetEmailAsync(employee3, Employee3EmailAddress);
                await userManager.AddToRoleAsync(employee3, "Employee");
            }

            var employee4 = await userManager.FindByEmailAsync(Employee4EmailAddress);
            if (employee4 == null)
            {
                employee4 = new IdentityUser("MedewerkerFrans");
                await userManager.CreateAsync(employee4, Employee4Password);
                await userManager.SetEmailAsync(employee4, Employee4EmailAddress);
                await userManager.AddToRoleAsync(employee4, "Employee");
            }

            var student1 = await userManager.FindByEmailAsync(Student1EmailAddress);
            if (student1 == null)
            {
                student1 = new IdentityUser("Joost");
                await userManager.CreateAsync(student1, Student1Password);
                await userManager.SetEmailAsync(student1, Student1EmailAddress);
                //await userManager.AddClaimAsync(student1, new Claim("Student", "true"));
                await userManager.AddToRoleAsync(student1, "Student");
            }

            var student2 = await userManager.FindByEmailAsync(Student2EmailAddress);
            if (student2 == null)
            {
                student2 = new IdentityUser("Henk");
                await userManager.CreateAsync(student2, Student2Password);
                await userManager.SetEmailAsync(student2, Student2EmailAddress);
                //await userManager.AddClaimAsync(student2, new Claim("Student", "true"));
                await userManager.AddToRoleAsync(student2, "Student");
            }

            var student3 = await userManager.FindByEmailAsync(Student3EmailAddress);
            if (student3 == null)
            {
                student3 = new IdentityUser("StudentJoost");
                await userManager.CreateAsync(student3, Student3Password);
                await userManager.SetEmailAsync(student3, Student3EmailAddress);
                await userManager.AddToRoleAsync(student3, "Student");
            }

            var student4 = await userManager.FindByEmailAsync(Student4EmailAddress);
            if (student4 == null)
            {
                student4 = new IdentityUser("StudentHenk");
                await userManager.CreateAsync(student4, Student4Password);
                await userManager.SetEmailAsync(student4, Student4EmailAddress);
                await userManager.AddToRoleAsync(student4, "Student");
            }

            var student5 = await userManager.FindByEmailAsync(Student5EmailAddress);
            if (student5 == null)
            {
                student5 = new IdentityUser("StudentGerrit");
                await userManager.CreateAsync(student5, Student5Password);
                await userManager.SetEmailAsync(student5, Student5EmailAddress);
                await userManager.AddToRoleAsync(student5, "Student");
            }

            var student6 = await userManager.FindByEmailAsync(Student6EmailAddress);
            if (student6 == null)
            {
                student6 = new IdentityUser("StudentGerrit");
                await userManager.CreateAsync(student6, Student6Password);
                await userManager.SetEmailAsync(student6, Student6EmailAddress);
                await userManager.AddToRoleAsync(student6, "Student");
            }
        }

    }
}

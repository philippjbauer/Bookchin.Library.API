using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Bookchin.Library.API.Data.Contexts;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Bookchin.Library.API.Data
{
    public class DatabaseSeeder
    {
        public List<Individual> Individuals { get; set; } = new List<Individual>();
        public List<Organization> Organizations { get; set; } = new List<Organization>();
        public Dictionary<string, string> Credentials { get; set; } = new Dictionary<string, string>();

        private AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public DatabaseSeeder(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            _context.Database.EnsureCreated();
            
            var faker = new Faker("en");
            var loggerFactory = new LoggerFactory();

            Randomizer.Seed = new Random(1612988783);

            var addressFaker = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.Country, f => f.Address.Country());

            var individualFaker = new Faker<Individual>()
                .RuleFor(i => i.Title, f => f.Random.Double(0, 1) > 0.8 ? f.Name.Prefix() : null)
                .RuleFor(i => i.FirstName, f => f.Name.FirstName())
                .RuleFor(i => i.MiddleName, f => f.Random.Double(0, 1) > 0.3 ? f.Name.FirstName() : null)
                .RuleFor(i => i.LastName, f => f.Name.LastName())
                .RuleFor(i => i.Suffix, f => f.Random.Double(0, 1) > 0.65 ? f.Name.Suffix() : null)
                .RuleFor(i => i.Address, addressFaker.Generate())
                .RuleFor(i => i.ApplicationUser, (f, i) =>  new ApplicationUser()
                {
                    Email = f.Internet.Email(i.FirstName, i.LastName),
                    UserName = f.Internet.UserName(i.FirstName, i.LastName),
                    SecurityStamp = Guid.NewGuid().ToString()
                });
            
            var organizationFaker = new Faker<Organization>()
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(i => i.Address, addressFaker.Generate())
                .RuleFor(i => i.ApplicationUser, (f, o) =>  new ApplicationUser()
                {
                    Email = f.Internet.Email(o.Name),
                    UserName = f.Internet.UserName(o.Name),
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            // Create Individuals
            if (_context.Individuals.Any() == false)
            {
                var individualsRepositoryLogger = new Logger<IndividualsRepository>(loggerFactory);
                var individualsRepository = new IndividualsRepository(individualsRepositoryLogger, _context);

                this.Individuals.AddRange(individualFaker.GenerateBetween(2, 5));

                individualsRepository.AddRange(this.Individuals);

                foreach (var individual in this.Individuals)
                {
                    string password = faker.Internet.Password(12);
                    await _userManager.CreateAsync(individual.ApplicationUser, password);

                    this.Credentials.Add(individual.ApplicationUser.UserName, password);
                    // Console.WriteLine($"Created new individual: {individual.ApplicationUser.UserName} with password {password}");
                }
            }

            // Create Organizations
            if (_context.Organizations.Any() == false)
            {
                var organizationsRepositoryLogger = new Logger<OrganizationsRepository>(loggerFactory);
                var organizationsRepository = new OrganizationsRepository(organizationsRepositoryLogger, _context);

                this.Organizations.AddRange(organizationFaker.GenerateBetween(2, 5));

                organizationsRepository.AddRange(this.Organizations);

                foreach (var organization in this.Organizations)
                {
                    string password = faker.Internet.Password(12);
                    await _userManager.CreateAsync(organization.ApplicationUser, password);

                    this.Credentials.Add(organization.ApplicationUser.UserName, password);
                    // Console.WriteLine($"Created new organization: {organization.ApplicationUser.UserName} with password {password}");
                }
            }

            if (File.Exists("./users.txt"))
            {
                File.Delete("./users.txt");
            }

            using (StreamWriter file = new StreamWriter("./users.txt"))
            {
                foreach ((string username, string password) in this.Credentials)
                {
                    file.WriteLine($"{username}:{password}");
                }
            }
        }
    }
}
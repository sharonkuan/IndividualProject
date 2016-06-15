using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using SupportApp.Models;
using System.Collections.Generic;

namespace SupportApp.Data
{
    public class SampleData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<ApplicationDbContext>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            // Ensure db
            db.Database.EnsureCreated();

            // Ensure Stephen (IsAdmin)
            var stephen = await userManager.FindByNameAsync("Stephen.Walther@CoderCamps.com");
            if (stephen == null)
            {
                // create user
                stephen = new ApplicationUser
                {
                    UserName = "Stephen.Walther@CoderCamps.com",
                    Email = "Stephen.Walther@CoderCamps.com",
                    FirstName = "Stephen",
                    LastName = "Walter",
                    PrimaryPhone = "111-222-3333",
                    IsProvider = false,
                    WillingToVolunteer = true,
                    Events = new List<Event>
                    {


                    }
                };
                await userManager.CreateAsync(stephen, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(stephen, new Claim("IsAdmin", "true"));
            }

            // Ensure Sharon (IsAdmin)
            var sharon = await userManager.FindByNameAsync("Sharon.Kuan@CoderCamps.com");
            if (sharon == null)
            {
                // create user
                sharon = new ApplicationUser
                {
                    UserName = "Sharon.Kuan@CoderCamps.com",
                    Email = "Sharon.Kuan@CoderCamps.com",
                    FirstName = "Sharon",
                    LastName = "Kuan",
                    PrimaryPhone = "888-888-8888",
                    IsProvider = true,
                    WillingToVolunteer = true
                };
                await userManager.CreateAsync(sharon, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(sharon, new Claim("IsAdmin", "true"));
            }

            // Ensure Mike (not IsAdmin)
            var mike = await userManager.FindByNameAsync("Mike@CoderCamps.com");
            if (mike == null)
            {
                // create user
                mike = new ApplicationUser
                {
                    UserName = "Mike@CoderCamps.com",
                    Email = "Mike@CoderCamps.com",
                    FirstName = "Mike",
                    LastName = "Johnson",
                    PrimaryPhone = "222-333-4444",
                    IsProvider = true,
                    WillingToVolunteer = true
                };
                await userManager.CreateAsync(mike, "Secret123!");
            }

            var tom = await userManager.FindByNameAsync("Tom@CoderCamps.com");
            if (tom == null)
            {
                // create user
                tom = new ApplicationUser
                {
                    UserName = "Tom@CoderCamps.com",
                    Email = "Tom@CoderCamps.com",
                    FirstName = "Tom",
                    LastName = "William",
                    PrimaryPhone = "444-555-6666",
                    IsProvider = false,
                    WillingToVolunteer = false
                };
                await userManager.CreateAsync(tom, "Secret123!");
            }
            db.SaveChanges();

            //-----------Custom sample data-------------------//
            #region seed Events
            if (!db.Events.Any())
            {
                var events = new Event[]
  {
                    //wants to limit this one as private 
                    //member and volunteer have to first connect in order to participate 
                    //only the connected can see the message
                 new Event {
                     EventType = "Visiting Nursing Homes",
                     EventTitle = "Bring happiness to the elders",
                     Details = "Please make sure that you are accepted by these nursing homes to participate. We will meet at Nursing Home 1 at the start time. We will spend about an hour there to keep them company and end the day by accompany them to their lunch room.",
                     EventStartDate = new DateTime(2016, 5, 29, 10, 45, 00).ToUniversalTime(),
                   EventEndDate = new DateTime(2016,5,29,12,00, 00).ToUniversalTime(),
                     DateCreated = new DateTime(2016, 5, 1).ToUniversalTime(),
                     IsActive = true,
                     IsComplete = true,
                     IsPrivate = false,
                     IsVolunteerRequired = "Yes",
                     PreferredNumberOfExpectedVolunteer = 8,
                     Locations = new List<Location> {
                         new Location {
                            NameOfLocation = "Nursing home 1",
                            Address = "address 1",
                            City = "Bellevue",
                            Zip = "98008"
                            }
                        },
                     Comments = new List<Comment> {
                         new Comment {
                             Message = "We appreciate your help!",
                             DateCreated = new DateTime(2016, 6, 1).ToUniversalTime() },
                         new Comment { Message = "It felt great helping, and thank you for the opportunity!",
                             DateCreated = new DateTime(2016, 6, 5).ToUniversalTime()},
                         new Comment { Message = "I am grateful to be part of it!",
                             DateCreated = new DateTime(2016, 6, 9).ToUniversalTime() }
                     }
                 },
                 new Event {
                     EventType = "Visiting Nursing Homes",
                     EventTitle = "Deliver gifts to the elders",
                     Details = "Please make sure that you are accepted by these nursing homes to participate. We will meet at Joanne Fabric located in Bellevue Cross Road     mall at the start time. We will bring the gifts     donated by members to the nursing homes",
                     EventStartDate = new DateTime(2016, 11, 19, 10, 30,00).ToUniversalTime(),
                   EventEndDate = new DateTime(2016,11,19,12,30,00).ToUniversalTime(),
                     DateCreated = new DateTime(2016, 5, 29).ToUniversalTime(),
                     IsActive = true,
                     IsComplete = false,
                     IsPrivate = false,
                     IsVolunteerRequired = "Yes",
                     PreferredNumberOfExpectedVolunteer = 5,
                     Locations = new List<Location> {
                         new Location {
                            NameOfLocation = "Nursing home 1",
                            Address = "address 1",
                            City = "Bellevue",
                            Zip = "98008"
                            },
                         new Location {
                            NameOfLocation = "Nursing home 2",
                            Address = "address 2",
                            City = "Bellevue",
                            Zip = "98008"
                            }
                        },
                     Comments = new List<Comment> {
                         new Comment {
                             Message = "I am sure it will be a very rewarding event for everyone!",
                             DateCreated = new DateTime(2016, 6, 1).ToUniversalTime() },
                         new Comment { Message = "I love it! See you guys there!",
                             DateCreated = new DateTime(2016, 6, 5).ToUniversalTime()
                         }
                     }
                 },
                 new Event {
                     EventType = "Sales",
                     EventTitle = "Garage Sale at a member's home",
                     Details = "We are helping an elder member clean up their unused items.",
                      EventStartDate = new DateTime(2016, 8, 27, 10,30,00).ToUniversalTime(),
                      EventEndDate = new DateTime(2016, 8,27,12,30,00).ToUniversalTime(),
                     DateCreated = DateTime.UtcNow,
                     IsActive = true,
                     IsComplete = false,
                     IsPrivate = false,
                     IsVolunteerRequired = "Maybe",
                     PreferredNumberOfExpectedVolunteer = 2,
                     Locations = new List<Location> {
                        new Location {
                            NameOfLocation = "Member B home",
                            Address = "address 2",
                            City = "New Castle",
                            Zip = "98005"
                            }
                        },
                      Comments = new List<Comment> {
                         new Comment { Message = "We will have fun, hope to see you there!",
                             DateCreated = new DateTime(2016, 6, 10).ToUniversalTime() }
                     }
                 },
                 //this one allows any volunteer to join, environment setup, cleanup, directing traffic, etc.
                 //future, we can invite lunch truck vendors to come and give discount to elderly members
                 new Event {
                     EventType = "Sales",
                     EventTitle = "Big Sales at Redmond Library Parking Lot",
                     Details = "Members have the opportunity to sell their handmade items",
                      EventStartDate = new DateTime(2016, 9, 24, 10, 00, 00).ToUniversalTime(),
                      EventEndDate = new DateTime(2016, 9, 24, 14, 00, 00).ToUniversalTime(),
                     DateCreated = DateTime.UtcNow,
                     IsActive = true,
                     IsComplete = false,
                     IsPrivate = false,
                     IsVolunteerRequired = "Yes",
                     PreferredNumberOfExpectedVolunteer = 10,
                     Locations = new List<Location> {
                        new Location {
                            NameOfLocation = "Redmond Library",
                            Address = "address 3",
                            City = "Redmond",
                            Zip = "9805p"
                            }
                        },
                      Comments = new List<Comment> {
                         new Comment { Message = "Let's help promote our members' art work!",
                             DateCreated = new DateTime(2016, 6, 10).ToUniversalTime() }
                        }
                    }
                };
                db.AddRange(events);
                db.SaveChanges();
            }
            #endregion

            #region seed EventUsers
            if (!db.EventUsers.Any())
            {
                var eventUsers = new EventUser[]
                {
                    new EventUser {
                        EventId = db.Events.FirstOrDefault(e => e.EventTitle == "Bring happiness to the elders").Id,
                        MemberId = db.Users.FirstOrDefault(e => e.FirstName == "Stephen").Id
                    },
                     new EventUser {
                        EventId = db.Events.FirstOrDefault(e => e.EventTitle == "Bring happiness to the elders").Id,
                        MemberId = db.Users.FirstOrDefault(e => e.FirstName == "Mike").Id
                    },
                     new EventUser {
                        EventId = db.Events.FirstOrDefault(e => e.EventTitle == "Garage Sale at a member's home").Id,
                        MemberId = db.Users.FirstOrDefault(e => e.FirstName == "Mike").Id
                    }
                };
                db.AddRange(eventUsers);
                db.SaveChanges();
            }
            #endregion
        }
    }
}


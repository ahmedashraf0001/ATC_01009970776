using event_booking_system.Common.Entites;
using Microsoft.EntityFrameworkCore;

namespace event_booking_system.Common.Utils
{
    public class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Music" },
                new Category { Id = 2, Name = "Sports" },
                new Category { Id = 3, Name = "Theater" },
                new Category { Id = 4, Name = "Comedy" },
                new Category { Id = 5, Name = "Conference" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    FirstName = "Ahmed",
                    LastName = "Ashraf",
                    UserName = "ahmedashraf",
                    Email = "ahmedashraf@example.com",
                    PhoneNumber = "01009970776",
                    Role = RoleType.User,
                    Address = "Cairo, Egypt",
                    DateJoined = baseDate
                },
                new User
                {
                    Id = "2",
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "johndoe",
                    Email = "johndoe@example.com",
                    PhoneNumber = "01234567890",
                    Role = RoleType.Admin,
                    Address = "New York, USA",
                    DateJoined = baseDate
                },
                new User
                {
                    Id = "3",
                    FirstName = "Sarah",
                    LastName = "Smith",
                    UserName = "sarahsmith",
                    Email = "sarah.smith@example.com",
                    PhoneNumber = "01122334455",
                    Role = RoleType.User,
                    Address = "London, UK",
                    DateJoined = baseDate.AddDays(-30)
                },
                new User
                {
                    Id = "4",
                    FirstName = "Emily",
                    LastName = "Brown",
                    UserName = "emilybrown",
                    Email = "emily.brown@example.com",
                    PhoneNumber = "01658748391",
                    Role = RoleType.User,
                    Address = "Los Angeles, USA",
                    DateJoined = baseDate.AddDays(-60)
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Rock Concert",
                    Description = "A thrilling rock concert with top bands.",
                    Date = baseDate.AddDays(30),
                    Location = "Cairo Arena",
                    VipPrice = 150,
                    AdmissionPrice = 50,
                    AdmissionTicketQty = 200,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Rock Concert.jpeg",
                    CreatedById = "1",
                    CategoryId = 1
                },
                new Event
                {
                    Id = 2,
                    Title = "Football Match",
                    Description = "A thrilling football match between the top teams.",
                    Date = baseDate.AddDays(60),
                    Location = "Football Stadium",
                    VipPrice = 100,
                    AdmissionPrice = 30,
                    AdmissionTicketQty = 500,
                    VipTicketQty = 300,
                    ImageUrl = "uploads/football.jpeg",
                    CreatedById = "2",
                    CategoryId = 2
                },
                new Event
                {
                    Id = 3,
                    Title = "Shakespeare Play",
                    Description = "A stunning rendition of Shakespeare's Hamlet.",
                    Date = baseDate.AddDays(90),
                    Location = "London Theater",
                    VipPrice = 120,
                    AdmissionPrice = 40,
                    AdmissionTicketQty = 150,
                    VipTicketQty = 50,
                    ImageUrl = "uploads/Shakespeare Play.jpg",
                    CreatedById = "3",
                    CategoryId = 3
                },
                new Event
                {
                    Id = 4,
                    Title = "Stand-up Comedy Show",
                    Description = "Laugh-out-loud comedy with top comedians.",
                    Date = baseDate.AddDays(120),
                    Location = "LA Comedy Club",
                    VipPrice = 80,
                    AdmissionPrice = 20,
                    AdmissionTicketQty = 300,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Stand-up Comedy Show.jpeg",
                    CreatedById = "4",
                    CategoryId = 4
                },
                new Event
                {
                    Id = 5,
                    Title = "Tech Conference",
                    Description = "Learn the latest in technology from industry leaders.",
                    Date = baseDate.AddDays(150),
                    Location = "Tech Convention Center",
                    VipPrice = 200,
                    AdmissionPrice = 75,
                    AdmissionTicketQty = 400,
                    VipTicketQty = 150,
                    ImageUrl = "uploads/Tech Conference.jpeg",
                    CreatedById = "2",
                    CategoryId = 5
                },
                new Event
                {
                    Id = 6,
                    Title = "Rock Concert",
                    Description = "A thrilling rock concert with top bands.",
                    Date = baseDate.AddDays(30),
                    Location = "Cairo Arena",
                    VipPrice = 150,
                    AdmissionPrice = 50,
                    AdmissionTicketQty = 200,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Rock Concert.jpeg",
                    CreatedById = "1",
                    CategoryId = 1
                },
                new Event
                {
                    Id = 7,
                    Title = "Football Match",
                    Description = "A thrilling football match between the top teams.",
                    Date = baseDate.AddDays(60),
                    Location = "Football Stadium",
                    VipPrice = 100,
                    AdmissionPrice = 30,
                    AdmissionTicketQty = 500,
                    VipTicketQty = 300,
                    ImageUrl = "uploads/football.jpeg",
                    CreatedById = "2",
                    CategoryId = 2
                },
                new Event
                {
                    Id = 8,
                    Title = "Shakespeare Play",
                    Description = "A stunning rendition of Shakespeare's Hamlet.",
                    Date = baseDate.AddDays(90),
                    Location = "London Theater",
                    VipPrice = 120,
                    AdmissionPrice = 40,
                    AdmissionTicketQty = 150,
                    VipTicketQty = 50,
                    ImageUrl = "uploads/Shakespeare Play.jpg",
                    CreatedById = "3",
                    CategoryId = 3
                },
                new Event
                {
                    Id = 9,
                    Title = "Stand-up Comedy Show",
                    Description = "Laugh-out-loud comedy with top comedians.",
                    Date = baseDate.AddDays(120),
                    Location = "LA Comedy Club",
                    VipPrice = 80,
                    AdmissionPrice = 20,
                    AdmissionTicketQty = 300,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Stand-up Comedy Show.jpeg",
                    CreatedById = "4",
                    CategoryId = 4
                }, new Event
                {
                    Id = 10,
                    Title = "Rock Concert",
                    Description = "A thrilling rock concert with top bands.",
                    Date = baseDate.AddDays(30),
                    Location = "Cairo Arena",
                    VipPrice = 150,
                    AdmissionPrice = 50,
                    AdmissionTicketQty = 200,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Rock Concert.jpeg",
                    CreatedById = "1",
                    CategoryId = 1
                },
                new Event
                {
                    Id = 11,
                    Title = "Football Match",
                    Description = "A thrilling football match between the top teams.",
                    Date = baseDate.AddDays(60),
                    Location = "Football Stadium",
                    VipPrice = 100,
                    AdmissionPrice = 30,
                    AdmissionTicketQty = 500,
                    VipTicketQty = 300,
                    ImageUrl = "uploads/football.jpeg",
                    CreatedById = "2",
                    CategoryId = 2
                },
                new Event
                {
                    Id = 12,
                    Title = "Shakespeare Play",
                    Description = "A stunning rendition of Shakespeare's Hamlet.",
                    Date = baseDate.AddDays(90),
                    Location = "London Theater",
                    VipPrice = 120,
                    AdmissionPrice = 40,
                    AdmissionTicketQty = 150,
                    VipTicketQty = 50,
                    ImageUrl = "uploads/Shakespeare Play.jpg",
                    CreatedById = "3",
                    CategoryId = 3
                },
                new Event
                {
                    Id = 13,
                    Title = "Stand-up Comedy Show",
                    Description = "Laugh-out-loud comedy with top comedians.",
                    Date = baseDate.AddDays(120),
                    Location = "LA Comedy Club",
                    VipPrice = 80,
                    AdmissionPrice = 20,
                    AdmissionTicketQty = 300,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Stand-up Comedy Show.jpeg",
                    CreatedById = "4",
                    CategoryId = 4
                },
                new Event
                {
                    Id = 14,
                    Title = "Tech Conference",
                    Description = "Learn the latest in technology from industry leaders.",
                    Date = baseDate.AddDays(150),
                    Location = "Tech Convention Center",
                    VipPrice = 200,
                    AdmissionPrice = 75,
                    AdmissionTicketQty = 400,
                    VipTicketQty = 150,
                    ImageUrl = "uploads/Tech Conference.jpeg",
                    CreatedById = "2",
                    CategoryId = 5
                },
                new Event
                {
                    Id = 15,
                    Title = "Rock Concert",
                    Description = "A thrilling rock concert with top bands.",
                    Date = baseDate.AddDays(30),
                    Location = "Cairo Arena",
                    VipPrice = 150,
                    AdmissionPrice = 50,
                    AdmissionTicketQty = 200,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Rock Concert.jpeg",
                    CreatedById = "1",
                    CategoryId = 1
                },
                new Event
                {
                    Id = 16,
                    Title = "Football Match",
                    Description = "A thrilling football match between the top teams.",
                    Date = baseDate.AddDays(60),
                    Location = "Football Stadium",
                    VipPrice = 100,
                    AdmissionPrice = 30,
                    AdmissionTicketQty = 500,
                    VipTicketQty = 300,
                    ImageUrl = "uploads/football.jpeg",
                    CreatedById = "2",
                    CategoryId = 2
                },
                new Event
                {
                    Id = 17,
                    Title = "Shakespeare Play",
                    Description = "A stunning rendition of Shakespeare's Hamlet.",
                    Date = baseDate.AddDays(90),
                    Location = "London Theater",
                    VipPrice = 120,
                    AdmissionPrice = 40,
                    AdmissionTicketQty = 150,
                    VipTicketQty = 50,
                    ImageUrl = "uploads/Shakespeare Play.jpg",
                    CreatedById = "3",
                    CategoryId = 3
                },
                new Event
                {
                    Id = 18,
                    Title = "Stand-up Comedy Show",
                    Description = "Laugh-out-loud comedy with top comedians.",
                    Date = baseDate.AddDays(120),
                    Location = "LA Comedy Club",
                    VipPrice = 80,
                    AdmissionPrice = 20,
                    AdmissionTicketQty = 300,
                    VipTicketQty = 100,
                    ImageUrl = "uploads/Stand-up Comedy Show.jpeg",
                    CreatedById = "4",
                    CategoryId = 4
                },
                new Event
                {
                    Id = 19,
                    Title = "Shakespeare Play",
                    Description = "A stunning rendition of Shakespeare's Hamlet.",
                    Date = baseDate.AddDays(90),
                    Location = "London Theater",
                    VipPrice = 120,
                    AdmissionPrice = 40,
                    AdmissionTicketQty = 150,
                    VipTicketQty = 50,
                    ImageUrl = "uploads/Shakespeare Play.jpg",
                    CreatedById = "3",
                    CategoryId = 3
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    UserId = "1",
                    EventId = 1,
                    AdmissionTicketQty = 2,
                    VipTicketQty = 1,
                    TicketType = TicketType.Both,
                    BookingDate = baseDate,
                    Status = BookingStatus.Pending
                },
                new Booking
                {
                    Id = 2,
                    UserId = "2",
                    EventId = 2,
                    AdmissionTicketQty = 4,
                    VipTicketQty = 0,
                    TicketType = TicketType.Admission,
                    BookingDate = baseDate.AddDays(-5),
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    Id = 3,
                    UserId = "3",
                    EventId = 3,
                    AdmissionTicketQty = 1,
                    VipTicketQty = 0,
                    TicketType = TicketType.Admission,
                    BookingDate = baseDate.AddDays(-10),
                    Status = BookingStatus.Canceled
                },
                new Booking
                {
                    Id = 4,
                    UserId = "4",
                    EventId = 4,
                    AdmissionTicketQty = 3,
                    VipTicketQty = 0,
                    TicketType = TicketType.Admission,
                    BookingDate = baseDate.AddDays(-3),
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    Id = 5,
                    UserId = "1",
                    EventId = 5,
                    AdmissionTicketQty = 2,
                    VipTicketQty = 1,
                    TicketType = TicketType.Both,
                    BookingDate = baseDate.AddDays(-7),
                    Status = BookingStatus.Confirmed
                }
            );
        }
    }
}
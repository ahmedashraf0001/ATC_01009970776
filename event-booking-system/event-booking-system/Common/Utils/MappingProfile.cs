using AutoMapper;
using event_booking_system.Common.DTOs.Bookings;
using event_booking_system.Common.DTOs.Events;
using event_booking_system.Common.DTOs.Users;
using event_booking_system.Common.Entites;

namespace event_booking_system.Common.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();   
            CreateMap<Event, EventDTO>();  
            CreateMap<Booking, BookingDTO>();

            CreateMap<UserDTO, User>();
            CreateMap<EventDTO, Event>();
            CreateMap<BookingDTO, Booking>();

        }
    }
}

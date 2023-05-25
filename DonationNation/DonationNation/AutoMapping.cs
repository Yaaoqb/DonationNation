using AutoMapper;
using DonationNation.Areas.Admin.Models;
using DonationNation.Data.Models;

namespace DonationNation
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Event model mapping
            CreateMap<EventCreate, Event>();
            CreateMap<EventEdit, Event>();
            CreateMap<Event, EventEdit>();

            //Badge model mapping
            CreateMap<BadgeCreate, Badge>();
        }
    }
}

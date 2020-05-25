using AutoMapper;
using SharedServices.BL.Domain;

namespace SharedServices.BL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DAL.Entities.Discussion, Discussion>().ForMember(dest => dest.DateHour, opt => opt.MapFrom(s => s.DateHour.ToLocalTime()));
            CreateMap<Discussion, DAL.Entities.Discussion>().ForMember(dest => dest.DateHour, opt => opt.MapFrom(s => s.DateHour.ToUniversalTime()));

            CreateMap<DAL.Entities.Request, Request>();
            CreateMap<Request, DAL.Entities.Request> ();

            CreateMap<DAL.Entities.Service, Service>();
            CreateMap<DAL.Entities.Service, Service>().ReverseMap();

            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>();
            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>().ReverseMap();

            CreateMap<DAL.Entities.Feedback, Feedback>();
            CreateMap<DAL.Entities.Feedback, Feedback>().ReverseMap();

            CreateMap<DAL.Entities.Picture, Picture>();
            CreateMap<DAL.Entities.Picture, Picture>().ReverseMap();
        }
    }
}
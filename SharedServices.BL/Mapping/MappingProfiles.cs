using AutoMapper;
using SharedServices.BL.Domain;

namespace SharedServices.BL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DAL.Entities.Discussion, Discussion>();
            CreateMap<DAL.Entities.Discussion, Discussion>().ReverseMap();

            CreateMap<DAL.Entities.Request, Request>();
            CreateMap<DAL.Entities.Request, Request>().ReverseMap();

            CreateMap<DAL.Entities.Service, Service>();
            CreateMap<DAL.Entities.Service, Service>().ReverseMap();

            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>();
            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>().ReverseMap();

            CreateMap<DAL.Entities.Feedback, Feedback>();
            CreateMap<DAL.Entities.Feedback, Feedback>().ReverseMap();
        }
    }
}
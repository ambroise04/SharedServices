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
            CreateMap<Request, DAL.Entities.Request>();

            CreateMap<DAL.Entities.RequestMulticast, RequestMulticast>()
                .ForMember(dest => dest.DateOfRequest, opt => opt.MapFrom(s => s.DateOfRequest.ToLocalTime()))
                .ForMember(dest => dest.DateOfAddition, opt => opt.MapFrom(s => s.DateOfAddition.ToLocalTime()));
            CreateMap<RequestMulticast, DAL.Entities.RequestMulticast>()
                .ForMember(dest => dest.DateOfRequest, opt => opt.MapFrom(s => s.DateOfRequest.ToUniversalTime()))
                .ForMember(dest => dest.DateOfAddition, opt => opt.MapFrom(s => s.DateOfAddition.ToUniversalTime()));

            CreateMap<DAL.Entities.ResponseMulticastRequest, ResponseMulticastRequest>();
            CreateMap<ResponseMulticastRequest, DAL.Entities.ResponseMulticastRequest>();

            CreateMap<DAL.Entities.Service, Service>();
            CreateMap<DAL.Entities.Service, Service>().ReverseMap();

            CreateMap<DAL.Entities.Place, Place>();
            CreateMap<DAL.Entities.Place, Place>().ReverseMap();

            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>();
            CreateMap<DAL.Entities.ServiceGroup, ServiceGroup>().ReverseMap();

            CreateMap<DAL.Entities.Feedback, Feedback>();
            CreateMap<DAL.Entities.Feedback, Feedback>().ReverseMap();

            CreateMap<DAL.Entities.Picture, Picture>();
            CreateMap<DAL.Entities.Picture, Picture>().ReverseMap();

            CreateMap<DAL.Entities.Notification, Notification>()
                .ForMember(dest => dest.DateOfAddition, opt => opt.MapFrom(s => s.DateOfAddition.ToLocalTime()));
            CreateMap<Notification, DAL.Entities.Notification>()
                .ForMember(dest => dest.DateOfAddition, opt => opt.MapFrom(s => s.DateOfAddition.ToUniversalTime()));

            CreateMap<DAL.Entities.FaqQuestion, FaqQuestion>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(s => s.Date.ToLocalTime()));
            CreateMap<FaqQuestion, DAL.Entities.FaqQuestion>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(s => s.Date.ToUniversalTime()));

            CreateMap<DAL.Entities.FaqResponse, FaqResponse>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(s => s.Date.ToLocalTime()));
            CreateMap<FaqResponse, DAL.Entities.FaqResponse>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(s => s.Date.ToUniversalTime()));

            CreateMap<DAL.Entities.NotificationType, NotificationType>();
            CreateMap<DAL.Entities.NotificationType, NotificationType>().ReverseMap();
        }
    }
}
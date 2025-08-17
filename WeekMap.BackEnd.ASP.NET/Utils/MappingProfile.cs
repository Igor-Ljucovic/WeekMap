using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Models;
using Models = WeekMap.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Automatically map properties with the same name from DTO to EF core model object (except for the ignored ones)
        // AutoMapper is slightly slower then manual mapping, but it is significantly faster then reflection
        // (ActivityCategoryID and ActivityTemplateID can be changed, so they are commented out)

        CreateMap<UserDTO, User>().ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<WeekMapDTO, Models.WeekMap>().ForMember(dest => dest.WeekMapID, opt => opt.Ignore())
                                                      .ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<WeekMapActivityDTO, WeekMapActivity>().ForMember(dest => dest.WeekMapID, opt => opt.Ignore())
                                                                    //.ForMember(dest => dest.ActivityTemplateID, opt => opt.Ignore())
                                                                      .ForMember(dest => dest.WeekMapActivityID, opt => opt.Ignore());
        CreateMap<ActivityTemplateDTO, ActivityTemplate>().ForMember(dest => dest.ActivityTemplateID, opt => opt.Ignore())
                                        //.ForMember(dest => dest.ActivityCategoryID, opt => opt.Ignore())
                                          .ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<ActivityCategoryDTO, ActivityCategory>().ForMember(dest => dest.ActivityCategoryID, opt => opt.Ignore());
        CreateMap<UserSettingsDTO, UserSettings>().ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<UserDefaultWeekMapSettingsDTO, UserDefaultWeekMapSettings>().ForMember(dest => dest.UserID, opt => opt.Ignore());
    }
}
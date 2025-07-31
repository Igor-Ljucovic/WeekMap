using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Automatically map properties with the same name from DTO to EF core model object (except for the ignored ones)
        // AutoMapper is slightly slower then manual mapping, but it is significantly faster then reflection
        // (ActivityCategoryID and ActivityID can be changed, so they are commented out)
        CreateMap<UserDTO, User>().ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<PlannedWeekMapDTO, PlannedWeekMap>().ForMember(dest => dest.PlannedWeekMapID, opt => opt.Ignore())
                                                      .ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<RealisedWeekMapDTO, RealisedWeekMap>().ForMember(dest => dest.PlannedWeekMapID, opt => opt.Ignore())
                                                        .ForMember(dest => dest.RealisedWeekMapID, opt => opt.Ignore());
        CreateMap<PlannedWeekMapActivityDTO, PlannedWeekMapActivity>().ForMember(dest => dest.PlannedWeekMapID, opt => opt.Ignore())
                                                                    //.ForMember(dest => dest.ActivityID, opt => opt.Ignore())
                                                                      .ForMember(dest => dest.PlannedWeekMapActivityID, opt => opt.Ignore());
        CreateMap<RealisedWeekMapActivityDTO, RealisedWeekMapActivity>().ForMember(dest => dest.RealisedWeekMapID, opt => opt.Ignore())
                                                                        .ForMember(dest => dest.ActivityID, opt => opt.Ignore())
                                                                        .ForMember(dest => dest.RealisedWeekMapActivityID, opt => opt.Ignore());
        CreateMap<ActivityDTO, Activity>().ForMember(dest => dest.ActivityID, opt => opt.Ignore())
                                        //.ForMember(dest => dest.ActivityCategoryID, opt => opt.Ignore())
                                          .ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<ActivityCategoryDTO, ActivityCategory>().ForMember(dest => dest.ActivityCategoryID, opt => opt.Ignore());
        CreateMap<UserSettingsDTO, UserSettings>().ForMember(dest => dest.UserID, opt => opt.Ignore());
        CreateMap<UserDefaultWeekMapSettingsDTO, UserDefaultWeekMapSettings>().ForMember(dest => dest.UserID, opt => opt.Ignore());
    }
}
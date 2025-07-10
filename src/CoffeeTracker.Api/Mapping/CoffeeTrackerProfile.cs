using AutoMapper;
using CoffeeTracker.Api.DTOs;
using CoffeeTracker.Api.Models;

namespace CoffeeTracker.Api.Mapping;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs
/// </summary>
public class CoffeeTrackerProfile : Profile
{
    public CoffeeTrackerProfile()
    {
        CreateMap<CoffeeEntry, CoffeeEntryResponse>()
            .ForMember(dest => dest.FormattedTimestamp,
                opt => opt.MapFrom(src => src.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")));

        CreateMap<CreateCoffeeEntryRequest, CoffeeEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
            .ForMember(dest => dest.SessionId, opt => opt.Ignore());
    }
}

namespace Rtl.Assignment.Scraper.Profiles
{
    using AutoMapper;
    using Rtl.Assignment.Domain.Entities;
    using Rtl.Assignment.Scraper.Dtos;

    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            this.CreateMap<ShowDto, ShowWithCastEntity>();
            this.CreateMap<CastDto, PersonEntity>()
                .ForMember(dest => dest.Name, src => src.MapFrom(_ => _.Person.Name))
                .ForMember(dest => dest.Birthday, src => src.MapFrom(_ => _.Person.Birthday))
                .ForMember(dest => dest.Id, src => src.MapFrom(_ => _.Person.Id));
        }
    }
}
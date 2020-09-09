namespace Rtl.Assignment.Api.Profiles
{
    using AutoMapper;
    using Rtl.Assignment.Api.Abstractions.Response;
    using Rtl.Assignment.Domain.Entities;

    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            this.CreateMap<ShowWithCastEntity, ShowWithCastResource>();
            this.CreateMap<PersonEntity, PersonResource>();
        }
    }
}
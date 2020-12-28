using AU.GetSession.Domain.Entities;
using AutoMapper;

namespace AU.GetSession.Function.Profiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Services.Repositories.Player.DataModel.Player, Player>()
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.RowKey));
        }
    }
}

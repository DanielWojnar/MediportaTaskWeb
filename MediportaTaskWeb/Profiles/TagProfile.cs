using AutoMapper;
using MediportaTaskWeb.Dto;
using MediportaTaskWeb.Models;

namespace MediportaTaskWeb.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile() {
            CreateMap<TagDTO, Tag>();
        }
    }
}

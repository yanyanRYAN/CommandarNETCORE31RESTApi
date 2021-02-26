using AutoMapper;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // map between source object to destination object
            CreateMap<Command, CommandReadDto>(); //map command to CommandReadDto -- reading
            CreateMap<CommandCreateDto, Command>(); //map CommandCreateDto to command -- writing
            CreateMap<CommandUpdateDto, Command>(); //map CommandUpdateDto to command -- update
            CreateMap<Command, CommandUpdateDto>(); //map command to CommandUpdateDto -- Patch
        }
    }
}
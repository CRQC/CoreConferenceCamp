using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile() 
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember(C => C.VenueName, O => O.MapFrom(m => m.Location.VenueName));

            this.CreateMap<Talk, TalkModel>();

            this.CreateMap<Speaker, SpeakerModel>();
        }
    }
}

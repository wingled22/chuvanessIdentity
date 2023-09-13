using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using sampleApp.Entities;
using sampleApp.Identity;

namespace sampleApp.Profiles
{
    public class SampleAppProfile : Profile
    {
        public SampleAppProfile()
        {
            CreateMap<Aspnetuser, ApplicationUser>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RPS.Domain.ProfileCompleteness;
using RPS.Presentation.Server.Models;

namespace RPS.Presentation.Server.Infrastructure
{
    public class GraphQLProfiles : Profile
    {
        public GraphQLProfiles()
        {
            CreateMap< Scoring,ScoreResult>(MemberList.Destination);
        }

    }
}

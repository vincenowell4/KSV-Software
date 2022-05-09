using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface ITimeZoneRepo
    {
        public VoteTimeZone GetById(int id);
        public List<VoteTimeZone> GetAllTimeZones();
    }
}
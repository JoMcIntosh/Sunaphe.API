using System;
using System.Collections.Generic;
using Sunaphe.API.Models;

namespace Sunaphe.API.Ephemeris {
    public interface IEphemeris {
        public IEnumerable<Placement<Planet>> GetPlanetaryPlacements(DateTime? gnomon = null);
        public Placement<Planet> GetPlanetaryPlacement(Planet planet, DateTime? gnomon = null);
        public Placement<Angle> GetAngularPlacement(Angle angle, Coordinate place, DateTime? gnomon = null);
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Sunaphe.API.Ephemeris;
using Sunaphe.API.Models;

namespace Sunaphe.API.Controllers {

    [ApiController]
    [Route("/current")]
    public class CurrentPlacementsController : ControllerBase {
        private IEphemeris _ephemeris = new SwissEphemerisWrapper();
        private ILogger<CurrentPlacementsController> _logger;

        public CurrentPlacementsController(ILogger<CurrentPlacementsController> logger) => _logger = logger;

        /// <summary>
        /// Returns the current placements (all planets + ascendant) for Seattle WA at the current time.
        /// </summary>
        /// <returns>A list of placements</returns>
        [Produces("application/json")]
        [HttpGet]
        public CurrentPlacementsResponse GetCurrentPlacements() {
            var seattle = new Coordinate(47.608013, -122.335167, "Seattle");
            var planets = _ephemeris.GetPlanetaryPlacements();
            var ascendant = _ephemeris.GetAngularPlacement(Angle.Ascendant, seattle);
            return new CurrentPlacementsResponse {
                Ascendant = ascendant,
                Planets = planets.ToArray()
            };
        }

        public class CurrentPlacementsResponse {
            public Placement<Angle> Ascendant { get; init; }
            public Placement<Planet>[] Planets { get; init; }
        }
    }
}



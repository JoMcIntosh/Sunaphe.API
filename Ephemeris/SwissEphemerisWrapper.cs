using System;
using System.Collections.Generic;
using System.Linq;
using SwissEphNet;
using Sunaphe.API.Models;

namespace Sunaphe.API.Ephemeris {
    public class SwissEphemerisWrapper : IEphemeris {

        private static SwissEph swissEphemeris = new SwissEph();

        private static Planet[] planets = {
            Planet.Sol, Planet.Luna, Planet.Mercury, Planet.Venus, Planet.Jupiter,
            Planet.Saturn, Planet.Uranus, Planet.Neptune, Planet.Pluto
        };

        public Placement<Angle> GetAngularPlacement(Angle angle, Coordinate place, DateTime? gnomon = null) {
            gnomon ??= DateTime.UtcNow;

            var hour = gnomon.Value.Hour + gnomon.Value.Minute / 60.0;

            var julianDay = swissEphemeris.swe_julday(
                year: gnomon.Value.Year,
                mon: gnomon.Value.Month,
                mday: gnomon.Value.Day,
                hour: hour,
                gregflag: SwissEph.SE_GREG_CAL
            );

            var cusps = new double[13];
            var angularLongitudes = new double[10];

            var result = swissEphemeris.swe_houses(
                tjd_ut: julianDay,
                geolat: place.Latitude,
                geolon: place.Longitude,
                hsys: 'W',
                cusps: cusps,
                ascmc: angularLongitudes
            );

            if (angle == Angle.Ascendant) {
                return new Placement<Angle>(angle, angularLongitudes[0]);
            }
            else {
                throw new NotImplementedException("Sorry I'm too lazy :(");
            }
        }

        public Placement<Planet> GetPlanetaryPlacement(Planet planet, DateTime? gnomon = null) {
            gnomon ??= DateTime.UtcNow;

            var hour = gnomon.Value.Hour + gnomon.Value.Minute / 60.0;

            var julianDay = swissEphemeris.swe_julday(
                year: gnomon.Value.Year,
                mon: gnomon.Value.Month,
                mday: gnomon.Value.Day,
                hour: hour,
                gregflag: SwissEph.SE_GREG_CAL
            );

            double[] placementArray = new double[6];
            string error = String.Empty;

            var result = swissEphemeris.swe_calc_ut(
                tjd_ut: julianDay,
                ipl: (int)planet,
                iflag: SwissEph.SEFLG_SWIEPH,
                xx: placementArray,
                serr: ref error
            );

            return new Placement<Planet>(planet, placementArray[0]);
        }

        public IEnumerable<Placement<Planet>> GetPlanetaryPlacements(DateTime? gnomon = null) {
            return planets.Select(planet => GetPlanetaryPlacement(planet, gnomon));
        }
    }
}
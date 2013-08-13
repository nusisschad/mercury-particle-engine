﻿namespace Mercury.ParticleEngine.Profiles
{
    using System;

    public class RingProfile : Profile
    {
        public float Radius { get; set; }
        public bool Radiate { get; set; }

        public override unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading)
        {
            FastRand.NextUnitVector((float*)heading);

            *offset = new Coordinate(heading->_x * Radius, heading->_y * Radius);

            if (!Radiate)
                FastRand.NextUnitVector((float*)heading);
        }
    }
}
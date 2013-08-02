﻿namespace Mercury.ParticleEngine.Modifiers
{
    using System;
    using FluentAssertions;

    internal class AssertionModifier : Modifier
    {
        readonly Predicate<Particle> _predicate;

        public AssertionModifier(Predicate<Particle> predicate)
        {
            _predicate = predicate;
        }

        internal override unsafe void Update(ref ParticleIterator iterator)
        {
            var particle = iterator.First;

            do
            {
                _predicate(*particle).Should().BeTrue();
            }
            while (iterator.MoveNext(&particle));
        }
    }
}
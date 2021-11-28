using System;
using System.Collections.Generic;

namespace TarsOffice.DayFeatures.Abstractions
{
    public interface IDayFeature
    {
        string Name { get; }
        public bool IsSatisfiedBy(DateTime date);
        IEnumerable<Tag> Render(DateTime date);
    }
}

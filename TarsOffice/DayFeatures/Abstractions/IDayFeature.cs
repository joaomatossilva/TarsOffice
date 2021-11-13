using System;
using System.Collections.Generic;

namespace TarsOffice.DayFeatures.Abstractions
{
    public interface IDayFeature
    {
        string Name { get; }
        public bool IsSatifiedBy(DateTime date);
        IEnumerable<Tag> Render(DateTime date);
    }
}

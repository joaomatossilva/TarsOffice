using System;
using System.Collections.Generic;
using TarsOffice.DayFeatures.Abstractions;

namespace TarsOffice.DayFeatures
{
    public class CakeDayFeature : IDayFeature
    {
        public string Name => "Cake";

        public bool IsSatisfiedBy(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Thursday;
        }

        public IEnumerable<Tag> Render(DateTime date)
        {
            return new Tag[] {
                new Tag
                {
                    Type= "img",
                    Properties =
                    {
                        { "src" , "/img/icons8-cake-64.png" },
                        { "style", "width: 20px" }
                    }
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using TarsOffice.DayFeatures.Abstractions;

namespace TarsOffice.DayFeatures
{
    public class CupCakeDayFeature : IDayFeature
    {
        private static DateTime FirstOccourrence = new DateTime(2021, 11, 29);
        public string Name => "CupCake";

        public bool IsSatisfiedBy(DateTime date)
        {
            var days = date.Subtract(FirstOccourrence);
            return days.Days % 14 == 0; // occurs every 15 days
        }

        public IEnumerable<Tag> Render(DateTime date)
        {
            return new Tag[] {
                new Tag
                {
                    Type= "img",
                    Properties =
                    {
                        { "src" , "/img/icons8-cupcake-30.png" },
                        { "style", "width: 20px" }
                    }
                }
            };
        }
    }
}

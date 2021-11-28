using System;
using System.Collections.Generic;
using TarsOffice.DayFeatures.Abstractions;

namespace TarsOffice.DayFeatures
{
    public class PastelNataDayFeature : IDayFeature
    {
        private static DateTime FirstOccourrence = new DateTime(2021, 11, 17);
        public string Name => "PastelNata";

        public bool IsSatisfiedBy(DateTime date)
        {
            var days = date.Subtract(FirstOccourrence);
            return days.Days % 14 == 0 || (days.Days + 5) % 14 == 0; // occurs every 15 days
        }

        public IEnumerable<Tag> Render(DateTime date)
        {
            return new Tag[] {
                new Tag
                {
                    Type= "img",
                    Properties =
                    {
                        { "src" , "/img/icons8-pastel-de-nata-40.png" },
                        { "style", "width: 20px" }
                    }
                }
            };
        }
    }
}

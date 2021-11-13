using System;
using System.Collections.Generic;
using TarsOffice.DayFeatures.Abstractions;

namespace TarsOffice.DayFeatures
{
    public class CroissantDayFeature : IDayFeature
    {
        private static DateTime FirstOccourrence = new DateTime(2021, 11, 24);
        public string Name => "Croissant";

        public bool IsSatifiedBy(DateTime date)
        {
            var days = date.Subtract(FirstOccourrence);
            return days.Days % 14 == 0; // occours every 15 days
        }

        public IEnumerable<Tag> Render(DateTime date)
        {
            return new Tag[] {
                new Tag
                {
                    Type= "img",
                    Properties =
                    {
                        { "src" , "/img/icons8-croissant-48.png" },
                        { "style", "width: 20px" }
                    }
                }
            };
        }
    }
}

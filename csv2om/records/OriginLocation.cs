using System;

namespace csv2om
{
    class OriginLocation : LocationRecord
    {
        public string ScheduledDeparture;
        public int PublicDeparture;
        public string Platform;
        public string Line;
        public string EngineeringAllowance;
        public string PathingAllowance;
        public string Activity;
        public string PerformanceAllowance;
        public string ReservedField;
        public string Spare;

        public override string ToString()
        {
            return Location;
        }

        public static OriginLocation FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            OriginLocation lo = new OriginLocation
            {
                RecordIdentity = values[0],
                Location = values[1],
                ScheduledDeparture = values[2],
                //WARNING! PublicDeparture in values[3] added at the end
                Platform = values[4],
                Line = values[5],
                EngineeringAllowance = values[6],
                PathingAllowance = values[7],
                Activity = values[8],
                PerformanceAllowance = values[9],
                ReservedField = values[10],
                Spare = values[11]
            };

            Int32.TryParse(values[3], out lo.PublicDeparture);

            return lo;
        }
    }
}

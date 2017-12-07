using System;

namespace csv2om
{
    class IntermediateLocation : LocationRecord
    {       
        public string ScheduledArrival;
        public string ScheduledDeparture;
        public string ScheduledPass;
        public int PublicArrival;
        public int PublicDeparture;
        public string Platform;
        public string Line;
        public string Path;
        public string Activity;
        public string EngineeringAllowance;
        public string PathingAllowance;
        public string PerformanceAllowance;
        public string ReservedField;
        public string Spare;

        public override string ToString()
        {
            return Location;
        }

        public static IntermediateLocation FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            IntermediateLocation li = new IntermediateLocation
            {
                RecordIdentity = values[0],
                Location = values[1],
                ScheduledArrival = values[2],
                ScheduledDeparture = values[3],
                ScheduledPass = values[4],
                //WARNING! PublicArrival in values[5] added at the end
                //WARNING! PublicDeparture in values[6] added at the end
                Platform = values[7],
                Line = values[8],
                Path = values[9],
                Activity = values[10],
                EngineeringAllowance = values[11],
                PathingAllowance = values[12],
                PerformanceAllowance = values[13],
                ReservedField = values[14],
                Spare = values[15]
            };

            Int32.TryParse(values[5], out li.PublicArrival);
            Int32.TryParse(values[6], out li.PublicDeparture);

            return li;
        }
    }
}

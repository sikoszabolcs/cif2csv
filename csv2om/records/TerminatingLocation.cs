using System;

namespace csv2om
{
    class TerminatingLocation : LocationRecord
    {
        public string ScheduledArrival;
        public int PublicArrival;
        public string Platform;
        public string Path;
        public string Activity;
        public string ReservedField;
        public string Spare;

        public override string ToString()
        {
            return Location;
        }

        public static TerminatingLocation FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            TerminatingLocation lt = new TerminatingLocation
            {
                RecordIdentity = values[0],
                Location = values[1],
                ScheduledArrival = values[2],
                //WARNING! PublicArrival in values[3] added at the end
                Platform = values[4],
                Path = values[5],
                Activity = values[6],
                ReservedField = values[7],
                Spare = values[8]
            };

            Int32.TryParse(values[3], out lt.PublicArrival);

            return lt;
        }
    }
}

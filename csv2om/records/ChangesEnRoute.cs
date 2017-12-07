using System;

namespace csv2om
{
    internal class ChangesEnRoute : LocationRecord
    {
        public string TrainCategory;
        public string TrainIdentity;
        public string Headcode;
        public string CourseIndicator;
        public string TrainServiceCode;
        public string PortionId;
        public string PowerType;
        public string TimingLoad;
        public int Speed;
        public string OperatingCharacteristics;
        public string SeatingClass;
        public string Sleepers;
        public string Reservations;
        public string ConnectionIndicator;
        public string CateringCode;
        public string ServiceBranding;
        public string TractionClass;
        public string UicCode;
        public string ReservedField;
        public string Spare;

        public override string ToString()
        {
            return Location;
        }

        public static ChangesEnRoute FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            ChangesEnRoute cer = new ChangesEnRoute
            {
                RecordIdentity = values[0],
                Location = values[1],
                TrainCategory = values[2],
                TrainIdentity = values[3],
                Headcode = values[4],
                CourseIndicator = values[5],
                TrainServiceCode = values[6],
                PortionId = values[7],
                PowerType = values[8],
                TimingLoad = values[9],
                //WARNING! Speed in values[10] added at the end
                OperatingCharacteristics = values[11],
                SeatingClass = values[12],
                Sleepers = values[13],
                Reservations = values[14],
                ConnectionIndicator = values[15],
                CateringCode = values[16],
                ServiceBranding = values[17],
                TractionClass = values[18],
                UicCode = values[19],
                ReservedField = values[20],
                Spare = values[21]
            };

            Int32.TryParse(values[10], out cer.Speed);

            return cer;
        }
    }
}
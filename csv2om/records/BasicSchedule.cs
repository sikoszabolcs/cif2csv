using System;
using System.Globalization;

namespace csv2om
{
    internal class BasicSchedule : CifRecord
    {
        string TransactionType;
        string TrainUid;
        DateTime DateRunsFrom;
        DateTime DateRunsTo;
        int DaysRun;
        string BankHolidayRunning;
        string TrainStatus;
        string TrainCategory;
        string TrainIdentity;
        int Headcode;
        int CourseIndicator;
        int TrainServiceCode;
        string PortionId;
        string PowerType;
        string TimingLoad;
        int Speed;
        string OperatingCharacteristics;
        string SeatingClass;
        string Sleepers;
        string Reservations;
        string ConnectionIndicator;
        string CateringCode;
        string ServiceBranding;
        string Spare;
        string StpIndicator;

        public override string ToString()
        {
            return String.Format("{0} from: {1} to: {2}", TrainIdentity, DateRunsFrom, DateRunsTo);
        }

        public static BasicSchedule FromCsv(string csvLine)
        {
            string pattern = "yyMMdd";

            string[] values = csvLine.Split(',');
            BasicSchedule bs = new BasicSchedule
            {
                RecordIdentity = values[0],
                TransactionType = values[1],
                TrainUid = values[2],
                DateRunsFrom = DateTime.ParseExact(values[3], pattern, null, DateTimeStyles.None),
                DateRunsTo = DateTime.ParseExact(values[4], pattern, null, DateTimeStyles.None),
                //WARNING! DaysRun in values[5] added at the end
                BankHolidayRunning = values[6],
                TrainStatus = values[7],
                TrainCategory = values[8],
                TrainIdentity = values[9],
                //WARNING! Headcode in values[10] added at the end
                //WARNING! CourseIndicator in values[11] added at the end
                //WARNING! TrainServiceCode in values[12] added at the end
                PortionId = values[13],
                PowerType = values[14],
                TimingLoad = values[15],
                //WARNING! Speed in values[16] added at the end
                OperatingCharacteristics = values[17],
                SeatingClass = values[18],
                Sleepers = values[19],
                Reservations = values[20],
                ConnectionIndicator = values[21],
                CateringCode = values[22],
                ServiceBranding = values[23],
                Spare = values[24],
                StpIndicator = values[25]
            };

            Int32.TryParse(values[5], out bs.DaysRun);
            Int32.TryParse(values[10], out bs.Headcode);
            Int32.TryParse(values[11], out bs.CourseIndicator);
            Int32.TryParse(values[12], out bs.TrainServiceCode);
            Int32.TryParse(values[16], out bs.Speed);

            return bs;
        }
    }
}
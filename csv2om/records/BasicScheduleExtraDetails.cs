using System;

namespace csv2om
{
    internal class BasicScheduleExtraDetails : CifRecord
    {
        public string TractionClass;
        public int UicCode;
        public string AtocCode;
        public string ApplicableTimetableCode;
        public string ReservedField1;
        public string ReservedField2;
        public string Spare;

        public override string ToString()
        {
            return String.Format("{0} - {1}", TractionClass, AtocCode);
        }

        public static BasicScheduleExtraDetails FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');          

            BasicScheduleExtraDetails bx = new BasicScheduleExtraDetails
            {
                RecordIdentity = values[0],
                TractionClass = values[1],
                //WARNING! UicCode in values[2] added at the end
                AtocCode = values[3],
                ApplicableTimetableCode = values[4],
                ReservedField1 = values[5],
                ReservedField2 = values[6],
                Spare = values[7]
            };

            Int32.TryParse(values[2], out bx.UicCode);

            return bx;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace csv2om
{
    partial class Program
    {
        static void Main(string[] args)
        {
            List<CifRecord> values = File.ReadAllLines("testDRBY.csv")
                .Select(v => Parse(v))
                .ToList();

            List<CifRecord> derby = new List<CifRecord>();

            CifRecord prev = null;

            foreach (var item in values)
            {
                if (item != null)
                {
                    if (item is BasicSchedule)
                    {
                        derby.Add(item);
                        prev = null;
                    }

                    if (item is LocationRecord)
                    {
                        var lo = item as LocationRecord;
                        if(lo.Location.Contains("DRBY"))
                        {
                            if (prev is LocationRecord)
                            {
                                var plr = prev as LocationRecord;
                                if (plr != null && !plr.Location.Contains("DRBY"))
                                {
                                    derby.Add(plr);
                                }
                            }

                            derby.Add(lo);
                        }
                        else
                        {
                            if (prev is LocationRecord)
                            {
                                var plr = prev as LocationRecord;
                                if (plr != null && plr.Location.Contains("DRBY"))
                                {
                                    derby.Add(item);
                                }
                            }
                        }
                        prev = item;
                    }
                }
            }

            foreach (var item in derby)
            {
                Console.WriteLine(item.ToString());
            }
        }
        
        static CifRecord Parse(string record)
        {
            CifRecord cifRecord = null;
            switch (record.Substring(0,2))
            {
                case "BS":
                    cifRecord = BasicSchedule.FromCsv(record);
                    break;
                case "BX":
                    cifRecord = BasicScheduleExtraDetails.FromCsv(record);
                    break;
                case "LO":
                    cifRecord = OriginLocation.FromCsv(record);
                    break;
                case "LI":
                    cifRecord = IntermediateLocation.FromCsv(record);
                    break;
                case "CR":
                    cifRecord = ChangesEnRoute.FromCsv(record);
                    break;
                case "LT":
                    cifRecord = TerminatingLocation.FromCsv(record);
                    break;
                default:
                    break;
            }
            return cifRecord;
        }
    }
}

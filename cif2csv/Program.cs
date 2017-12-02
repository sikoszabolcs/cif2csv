using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace cif2csv
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Usage();
                return;
            }

            var inputFile = "";
            var outputFile = "";

            foreach (var arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-cif":
                        inputFile = args[1];
                        break;
                    case "-csv":
                        outputFile = args[3];
                        break;
                }
            }

            var sw = new Stopwatch();
            sw.Start();
            ReadInput(inputFile, outputFile);
            sw.Stop();

            Console.Write("Done. Took {0}s", sw.Elapsed.Seconds);
        }

        private static StringBuilder _derbyTimetables = new StringBuilder();
        private static StringBuilder _derbyTimetableCandidate = new StringBuilder();
        private static bool _keep = false;

        private static string ParseRecord(string cifRecord)
        {
            var recordId = cifRecord.Substring(0, RecordIdentitySize);

            var csvOutput = new StringBuilder(cifRecord.Length);
            

            switch (recordId)
            {
                case "LI":
                    string li = ParseIntermediateLocation(cifRecord);
                    csvOutput.Append(li);

                    _derbyTimetableCandidate.Append(li);

                    if (li.ToLower().Contains("drby"))
                    {
                        _keep = true;
                    }

                    break;
                case "LO":
                    string lo = ParseOriginLocation(cifRecord);
                    csvOutput.Append(lo);

                    _derbyTimetableCandidate.Append(lo);

                    if (lo.ToLower().Contains("drby"))
                    {
                        _keep = true;
                    }

                    break;
                case "LT":
                    string lt = ParseTerminatingLocation(cifRecord);
                    csvOutput.Append(lt);

                    _derbyTimetableCandidate.Append(lt);

                    if (lt.ToLower().Contains("drby"))
                    {
                        _keep = true;
                    }

                    break;
                case "CR":
                    string cr = ParseChangesEnRoute(cifRecord);
                    csvOutput.Append(cr);
                    _derbyTimetableCandidate.Append(cr);
                    break;
                case "BS":
                    string bs = ParseBasicSchedule(cifRecord);
                    csvOutput.Append(bs);

                    if (_keep)
                    {
                        _derbyTimetables.Append(_derbyTimetableCandidate);
                        _keep = false;
                    }

                    _derbyTimetableCandidate = new StringBuilder(cifRecord.Length);
                    _derbyTimetableCandidate.Append(bs);
                    break;
                case "BX":
                    string bx = ParseBasicScheduleExtraDetails(cifRecord);
                    csvOutput.Append(bx);

                    _derbyTimetableCandidate.Append(bx);
                    break;
                default:
                    csvOutput.Append("ERROR! Unknown Record Identifier: ");
                    csvOutput.Append(recordId);
                    break;
            }
            csvOutput.AppendLine();
            _derbyTimetableCandidate.AppendLine();

            return csvOutput.ToString();
        }

        private static string ParseBasicScheduleExtraDetails(string record)
        {
            return "BX";
        }

        private static string ParseBasicSchedule(string record)
        {
            return "BS";
        }

        private static string ParseChangesEnRoute(string record)
        {
            return "CR";
        }

        private const int ScheduledArrivalSize = 5;
        private const int PublicArrivalSize = 4;
        private const int PathSize = 3;
        private const int LtReservedFieldSize = 3;
        private const int LtSpareSize = 40;

        private static string ParseTerminatingLocation(string record)
        {
            var liCsv = new StringBuilder(record.Length);
            int index = 0;

            liCsv.Append(record.Substring(index, RecordIdentitySize));
            index += RecordIdentitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LocationSize));
            index += LocationSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ScheduledArrivalSize));
            index += ScheduledArrivalSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PublicArrivalSize));
            index += PublicArrivalSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PlatformSize));
            index += PlatformSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PathSize));
            index += PathSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ActivitySize));
            index += ActivitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LtReservedFieldSize));
            index += LtReservedFieldSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LtSpareSize));

            return liCsv.ToString();
        }

        private const int RecordIdentitySize = 2;
        private const int LocationSize = 8;
        private const int ScheduledDepartureSize = 5;
        private const int ScheduledPassSize = 5;
        private const int PublicDepartureSize = 4;
        private const int PlatformSize = 3;
        private const int LineSize = 3;
        private const int EngineeringAllowanceSize = 2;
        private const int PathingAllowanceSize = 2;
        private const int ActivitySize = 12;
        private const int PerformanceAllowanceSize = 2;
        private const int LoReservedFieldSize = 3;
        private const int LoSpareSize = 34;
        private const int LiReservedFieldSize = 5;
        private const int LiSpareSize = 15;

        private static string ParseOriginLocation(string record)
        {
            var liCsv = new StringBuilder(record.Length);
            int index = 0;

            liCsv.Append(record.Substring(index, RecordIdentitySize));
            index += RecordIdentitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LocationSize));
            index += LocationSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ScheduledDepartureSize));
            index += ScheduledDepartureSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PublicDepartureSize));
            index += PublicDepartureSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PlatformSize));
            index += PlatformSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LineSize));
            index += LineSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, EngineeringAllowanceSize));
            index += EngineeringAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PathingAllowanceSize));
            index += PathingAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ActivitySize));
            index += ActivitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PerformanceAllowanceSize));
            index += PerformanceAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LoReservedFieldSize));
            index += LoReservedFieldSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LoSpareSize));
            index += LoSpareSize;
            liCsv.Append(",");

            return liCsv.ToString();
        }

        private static string ParseIntermediateLocation(string record)
        {
            var liCsv = new StringBuilder(record.Length);
            int index = 0;

            liCsv.Append(record.Substring(index, RecordIdentitySize));
            index += RecordIdentitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LocationSize)); // Location
            index += LocationSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ScheduledArrivalSize)); // Scheduled Arrival
            index += ScheduledArrivalSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ScheduledDepartureSize)); // Scheduled Departure
            index += ScheduledDepartureSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ScheduledPassSize)); // Scheduled Pass
            index += ScheduledPassSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PublicArrivalSize)); // Public Arrival
            index += PublicArrivalSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PublicDepartureSize)); // Public Departure
            index += PublicDepartureSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PlatformSize)); // Platform
            index += PlatformSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LineSize)); // Line
            index += LineSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PathSize)); // Path
            index += PathSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, ActivitySize)); // Activity
            index += ActivitySize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, EngineeringAllowanceSize)); // Engineering Allowance
            index += EngineeringAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PathingAllowanceSize)); // Pathing Allowance
            index += PathingAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, PerformanceAllowanceSize)); // Performance Allowance
            index += PerformanceAllowanceSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LiReservedFieldSize)); // Reserved Field
            index += LiReservedFieldSize;
            liCsv.Append(",");

            liCsv.Append(record.Substring(index, LiSpareSize)); // Spare

            return liCsv.ToString();
        }

        private static void ReadInput(string inputFile, string outputFile)
        {
            // does file exist?
            if (File.Exists(inputFile))
            {
                using (var outputStream = new StreamWriter(outputFile, false, Encoding.UTF8))
                using (var inputStream = new StreamReader(inputFile))
                {
                    var input = "";
                    while ((input = inputStream.ReadLine()) != null)
                    {
                        var csvLine = ParseRecord(input);
                        
                    }
                }

                using (var outputStream = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    outputStream.WriteLine(_derbyTimetables.ToString());
                }
                
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage: cif2csv -cif <filename> -csv <filename>");
        }
    }
}

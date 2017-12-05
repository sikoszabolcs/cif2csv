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
            if (args.Length < 4)
            {
                Usage();
                return;
            }

            var inputFile = "";
            var outputFile = "";
            var filter = "";

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
                    case "-f":
                        filter = args[5];
                        break;
                }
            }

            MemoryMapped mm = new MemoryMapped(inputFile, outputFile);
            mm.Convert();

            //var sw = new Stopwatch();
            //sw.Start();
            //ReadInput(inputFile, outputFile, filter);
            //sw.Stop();

            //Console.Write("Done. Took {0}s", sw.Elapsed.Seconds);
        }

        private static StringBuilder _filteredSchedules = new StringBuilder();
        private static StringBuilder _filteredScheduleCandidate = new StringBuilder();
        private static bool _keep = false;

        public static string ParseRecord(string cifRecord, string filter)
        {
            var recordId = cifRecord.Substring(0, RecordIdentitySize);

            var csvOutput = new StringBuilder(cifRecord.Length);
            

            switch (recordId)
            {
                case "LI":
                    string li = ParseIntermediateLocation(cifRecord);
                    csvOutput.Append(li);

                    _filteredScheduleCandidate.Append(li);

                    if (li.ToLower().Contains(filter.ToLower()))
                    {
                        _keep = true;
                    }

                    break;
                case "LO":
                    string lo = ParseOriginLocation(cifRecord);
                    csvOutput.Append(lo);

                    _filteredScheduleCandidate.Append(lo);

                    if (lo.ToLower().Contains(filter.ToLower()))
                    {
                        _keep = true;
                    }

                    break;
                case "LT":
                    string lt = ParseTerminatingLocation(cifRecord);
                    csvOutput.Append(lt);

                    _filteredScheduleCandidate.Append(lt);

                    if (lt.ToLower().Contains(filter.ToLower()))
                    {
                        _keep = true;
                    }

                    break;
                case "CR":
                    string cr = ParseChangesEnRoute(cifRecord);
                    csvOutput.Append(cr);
                    _filteredScheduleCandidate.Append(cr);
                    break;
                case "BS":
                    string bs = ParseBasicSchedule(cifRecord);
                    csvOutput.Append(bs);

                    if (_keep)
                    {
                        _filteredSchedules.Append(_filteredScheduleCandidate);
                        _keep = false;
                    }

                    _filteredScheduleCandidate = new StringBuilder(cifRecord.Length);
                    _filteredScheduleCandidate.Append(bs);
                    break;
                case "BX":
                    string bx = ParseBasicScheduleExtraDetails(cifRecord);
                    csvOutput.Append(bx);

                    _filteredScheduleCandidate.Append(bx);
                    break;
                default:
                    csvOutput.Append("ERROR! Unknown Record Identifier: ");
                    csvOutput.Append(recordId);
                    break;
            }
            csvOutput.AppendLine();
            _filteredScheduleCandidate.AppendLine();

            return csvOutput.ToString();
        }

        private const int TractionClassSize = 4;
        private const int UicCodeSize = 5;
        private const int AtocCodeSize = 2;
        private const int ApplicableTimetableCodeSize = 1;
        private const int BxReservedFieldSize1 = 8;
        private const int BxReservedFieldSize2 = 1;
        private const int BxSpareSize = 57;

        private static string ParseBasicScheduleExtraDetails(string record)
        {
            var bxCsv = new StringBuilder(record.Length);
            int index = 0;

            bxCsv.Append(record.Substring(index, RecordIdentitySize));
            index += RecordIdentitySize;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, TractionClassSize));
            index += TractionClassSize;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, UicCodeSize));
            index += UicCodeSize;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, AtocCodeSize));
            index += AtocCodeSize;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, ApplicableTimetableCodeSize));
            index += ApplicableTimetableCodeSize;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, BxReservedFieldSize1));
            index += BxReservedFieldSize1;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, BxReservedFieldSize2));
            index += BxReservedFieldSize2;
            bxCsv.Append(",");

            bxCsv.Append(record.Substring(index, BxSpareSize));

            return bxCsv.ToString();
        }
        private const int TransactionTypeSize = 1;
        private const int TrainUidSize = 6;
        private const int DateRunsFromSize = 6;
        private const int DateRunsToSize = 6;
        private const int DaysRunSize = 7;
        private const int BankHolidayRunningSize = 1;
        private const int TrainStatusSize = 1;
        private const int TrainCategorySize = 2;
        private const int TrainIdentitySize = 4;
        private const int HeadcodeSize = 4;
        private const int CourseIndicatorSize = 1;
        private const int TrainServiceCodeSize = 8;
        private const int PortionIdSize = 1;
        private const int PowerTypeSize = 3;
        private const int TimingLoadSize = 4;
        private const int SpeedSize = 3;
        private const int OperatingCharacteristicsSize = 6;
        private const int SeatingClassSize = 1;
        private const int SleepersSize = 1;
        private const int ReservationsSize = 1;
        private const int ConnectionIndicatorSize = 1;
        private const int CateringCodeSize = 4;
        private const int ServiceBrandingSize = 4;
        private const int BsSpareSize = 1;
        private const int StpIndicatorSize = 1;

        private static string ParseBasicSchedule(string record)
        {
            var bsCsv = new StringBuilder(record.Length);
            int index = 0;

            bsCsv.Append(record.Substring(index, RecordIdentitySize));
            index += RecordIdentitySize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TransactionTypeSize));
            index += TransactionTypeSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TrainUidSize));
            index += TrainUidSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, DateRunsFromSize));
            index += DateRunsFromSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, DateRunsToSize));
            index += DateRunsToSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, DaysRunSize));
            index += DaysRunSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, BankHolidayRunningSize));
            index += BankHolidayRunningSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TrainStatusSize));
            index += TrainStatusSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TrainCategorySize));
            index += TrainCategorySize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TrainIdentitySize));
            index += TrainIdentitySize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, HeadcodeSize));
            index += HeadcodeSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, CourseIndicatorSize));
            index += CourseIndicatorSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TrainServiceCodeSize));
            index += TrainServiceCodeSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, PortionIdSize));
            index += PortionIdSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, PowerTypeSize));
            index += PowerTypeSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, TimingLoadSize));
            index += TimingLoadSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, SpeedSize));
            index += SpeedSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, OperatingCharacteristicsSize));
            index += OperatingCharacteristicsSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, SleepersSize));
            index += SleepersSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, ReservationsSize));
            index += ReservationsSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, ConnectionIndicatorSize));
            index += ConnectionIndicatorSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, CateringCodeSize));
            index += CateringCodeSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, ServiceBrandingSize));
            index += ServiceBrandingSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, BsSpareSize));
            index += BsSpareSize;
            bsCsv.Append(",");

            bsCsv.Append(record.Substring(index, StpIndicatorSize));

            return bsCsv.ToString();
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

        private static void ReadInput(string inputFile, string outputFile, string filter)
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
                        var csvLine = ParseRecord(input, filter);
                        
                    }
                }

                using (var outputStream = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    outputStream.WriteLine(_filteredSchedules.ToString());
                }
                
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage: cif2csv -cif <filename> -csv <filename> [-f <filter>]");
        }
    }
}

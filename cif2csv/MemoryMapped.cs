using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cif2csv
{
    internal class MemoryMapped
    {
        private string _inputCifFile;
        private string _outputCsvFile;
        //private MemoryMappedFile _mmCifFile;

        internal MemoryMapped(string inputCifFile, string outputCsvFile)
        {
            _inputCifFile = inputCifFile;
            _outputCsvFile = outputCsvFile;
        }

        internal void Convert()
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            using (var mmCifFile = MemoryMappedFile.CreateFromFile(_inputCifFile, FileMode.Open, "Cif"))
            using (MemoryMappedFile mmCsvFile = MemoryMappedFile.CreateFromFile(_outputCsvFile, FileMode.Create, "Csv", 561000000))
            {
                using (var accessor = mmCifFile.CreateViewAccessor(0, 0))
                using (var csvAccessor = mmCsvFile.CreateViewAccessor(0, 0))
                {
                    int readPos = 0;
                    int writePos = 0;
                    int length = 80;
                    byte[] inBuffer = new byte[length];

                    while (accessor.ReadArray(readPos, inBuffer, 0, length) > 0)
                    {
                        string cifLine = Encoding.ASCII.GetString(inBuffer);
                        string csvLine = Program.ParseRecord(cifLine, "DRBY");

                        var csvBytes = Encoding.ASCII.GetBytes(csvLine);
                        csvAccessor.WriteArray(writePos, csvBytes, 0, csvBytes.Length);

                        readPos += length;
                        writePos += csvBytes.Length;
                    }

                    csvAccessor.Flush();
                }
            }

            sw.Stop();
            Console.WriteLine("Convert took {0}s", sw.Elapsed.Seconds);
        }
    }
}

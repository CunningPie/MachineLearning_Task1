using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning_Task1
{
    static class DataFileWorker
    {
        public static void Normalize(this List<List<double>> Data)
        {
            for (int i = 0; i < Data.Count(); i++)
            {
                var Max = Data[i].Max();
                var Min = Data[i].Min();

                Max = Max == Min ? Max + 1 : Max;

                Data[i] = Data[i].Select(x => (x - Min) / (Max - Min)).ToList();
            }
        }

        public static List<List<double>> AppendData(this List<List<double>> Data, List<List<double>> newData)
        {
            if (Data.Count == 0)
            {
                for (int i = 0; i < newData.Count; i++)
                {
                    Data.Add(new List<double>(newData[i].Select(x => x).ToList()));
                }
            }
            else
                for (int i = 0; i < Data.Count; i++)
                    Data[i].AddRange(newData[i]);

            return Data;
        }
        public static List<List<double>> OpenCSVFile(string FileName)
        {
            var Data = new List<List<double>>();

            using (var reader = new StreamReader(FileName))
            {
                var line = reader.ReadLine();

                int fnum = line.Split(',').Count();

                for (int i = 0; i < fnum; i++)
                    Data.Add(new List<double>());

                while (!reader.EndOfStream)
                {
                    var values = line.Split(',').Select(x => Convert.ToDouble(x.Replace('.', ','))).ToArray();

                    int i = 0;

                    foreach (double val in values)
                    {
                        Data[i++].Add(val);
                    }

                    line = reader.ReadLine();
                }
            }
      
            return Data;
        }

        public static void SaveCSVFile(List<List<double>> Data, string FileName)
        {
            using (var writer = new StreamWriter(FileName))
            {
                for (int i = 0; i < Data[0].Count; i++)
                {
                    string str = "";
                    for (int j = 0; j < Data.Count; j++)
                    {
                        str += (Data[j][i].ToString().Replace(',', '.') + ',');
                    }
                    writer.WriteLine(str.Substring(0, str.Length- 1));
                }
            }
        }

        public static void SaveCSVFile(List<double> Data, string FileName)
        {
            using (var writer = new StreamWriter(FileName))
            {
                string str = "";
                for (int j = 0; j < Data.Count; j++)
                {
                    str += (Data[j].ToString().Replace(',', '.') + ',');
                }
                writer.WriteLine(str.Substring(0, str.Length - 1));
            }
        }

        public static void SaveFile(string Data, string FileName)
        {
            using (var writer = new StreamWriter(FileName))
            {
                writer.Write(Data);
            }
        }

    }
}

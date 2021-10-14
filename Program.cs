using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning_Task1
{
    class Program
    {
        static double [] GenerateWeigths(int Length)
        {
            var res = new double[Length];
            var rand = new Random();

            for (int i = 0; i < Length; i++)
                res[i] = rand.NextDouble() * 0.001;

            return res;
        }

        static double GetSTD(double [] Data)
        {
            double mean = Data.Average();

            double temp = 0;

            foreach (double a in Data)
                temp += (a - mean) * (a - mean);
            return Math.Sqrt(temp / (Data.Length - 1));
        }

        static void Main(string[] args)
        {
            /*
            var Data2 = CSV.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\Features_Variant_2.csv");
            var Data3 = CSV.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\Features_Variant_3.csv");
            var Data4 = CSV.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\Features_Variant_4.csv");
            var Data5 = CSV.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\Features_Variant_5.csv");
            
            var DataSet1 = new List<List<double>>().AppendData(Data2).AppendData(Data3).AppendData(Data4).AppendData(Data5);
            var DataSet2 = new List<List<double>>().AppendData(Data1).AppendData(Data3).AppendData(Data4).AppendData(Data5);
            var DataSet3 = new List<List<double>>().AppendData(Data1).AppendData(Data2).AppendData(Data4).AppendData(Data5);
            var DataSet4 = new List<List<double>>().AppendData(Data1).AppendData(Data2).AppendData(Data3).AppendData(Data5);
            var DataSet5 = new List<List<double>>().AppendData(Data1).AppendData(Data2).AppendData(Data3).AppendData(Data4);

            DataSet1.Normalize();
            DataSet2.Normalize();
            DataSet3.Normalize();
            DataSet4.Normalize();
            DataSet5.Normalize();

            CSV.SaveCSVFile(DataSet1, @"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_1.csv");
            CSV.SaveCSVFile(DataSet2, @"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_2.csv");
            CSV.SaveCSVFile(DataSet3, @"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_3.csv");
            CSV.SaveCSVFile(DataSet4, @"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_4.csv");
            CSV.SaveCSVFile(DataSet5, @"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_5.csv");

            */
            double[] RSquaredTrain = new double[5];
            double[] RMSETrain = new double[5];
            double[] RSquaredTest = new double[5];
            double[] RMSETest = new double[5];

            string Res = "";

            for (int k = 1; k < 6; k++)
            {
                Res += "FOLD" + k + "\n";
                var Data = DataFileWorker.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\DataSet_" + k + ".csv");
                var TestData = DataFileWorker.OpenCSVFile(@"F:\GitHub\MachineLearning_Task1\Dataset\Training\Features_Variant_" + k + ".csv");

                MachineLearning ML = new MachineLearning(Data[Data.Count - 1]);
                Data.RemoveAt(Data.Count - 1);
                Data.Insert(0, Enumerable.Repeat(1.0, Data[0].Count).ToList());

                var defaultWeights = GenerateWeigths(Data.Count);

                var AccurateWeights = ML.AntigradientDescent(ML.RMSE, Data, defaultWeights);
                RMSETrain[k - 1] = ML.RMSE(Data, AccurateWeights);
                Res += "RMSE_TRAIN " + RMSETrain[k-1].ToString() + "\n";

                RSquaredTrain[k - 1] = ML.RSquared(Data, AccurateWeights);
                Res += "RSQUARED_TRAIN " + RSquaredTrain[k - 1].ToString() + "\n";

                TestData.Normalize();
                ML.TargetValues = TestData[TestData.Count - 1];
                TestData.RemoveAt(TestData.Count - 1);
                TestData.Insert(0, Enumerable.Repeat(1.0, TestData[0].Count).ToList());

                RMSETest[k - 1] = ML.RMSE(TestData, AccurateWeights);
                Res += "RMSE_TEST " + RMSETest[k - 1].ToString() + "\n";
                RSquaredTest[k - 1] = ML.RSquared(TestData, AccurateWeights);
                Res += "RSQUARED_TEST " + RSquaredTest[k - 1].ToString() + "\n";

                DataFileWorker.SaveCSVFile(AccurateWeights.ToList(), "Weights_Dataset" + k + ".csv");

            }
            Res += "RMSE_TRAIN Mean " + RMSETrain.Average() + "\n";
            Res += "RSQUARED_TRAIN Mean " + RSquaredTrain.Average() + "\n";
            Res += "RMSE_TEST Mean " + RMSETest.Average() + "\n";
            Res += "RSQUARED_TEST Mean " + RSquaredTest.Average() + "\n";

            Res += "RMSE_TRAIN STD " + GetSTD(RMSETrain) + "\n";
            Res += "RSQUARED_TRAIN STD " + GetSTD(RSquaredTrain) + "\n";
            Res += "RMSE_TEST STD " + GetSTD(RMSETest) + "\n";
            Res += "RSQUARED_TEST STD " + GetSTD(RSquaredTest) + "\n";

            DataFileWorker.SaveFile(Res, "Result.txt");
        }
    }
}

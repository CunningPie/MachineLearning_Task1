using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning_Task1
{
    class MachineLearning
    {

        public List<double> TargetValues;

        public MachineLearning(List<double> Values)
        {
            TargetValues = new List<double>(Values);


            /*
            for (int i = 0; i < Data.Count(); i++)
            {
                for (int j = 0; j < Data.Count(); j++)
                {
                    var corr = FeatureCorrelation(Data[i], Data[j]);

                    if (i != j && (corr > 0.9 || corr < -0.9))
                    {
                        Data.RemoveAt(j);
                        j--;
                        i--;
                    }
                }
            }
            */
        }


        public double MeanSquareDeviation(List<double> list)
        {
            var average = list.Average();
            return Math.Sqrt(Enumerable.Sum(list.Select(x => Math.Pow((x - average), 2))) / (list.Count - 1));
        }

        public double FeatureCorrelation(List<double> FeatureA, List<double> FeatureB)
        {
            double averageA = FeatureA.Average(), averageB = FeatureB.Average();
            var r = (Enumerable.Sum(FeatureA.Select((x, index) => x * FeatureB[index])) - FeatureA.Count * averageA * averageB)
                / ((FeatureA.Count - 1) * MeanSquareDeviation(FeatureA) * MeanSquareDeviation(FeatureB));

            return r;
        }


        public double RMSE(List<List<double>> Data, double [] Weights)
        {
            var predict = LinearRegression(Data, Weights);

            double y = Enumerable.Sum(TargetValues.Select((x, index) => Math.Pow(x - predict[index], 2)).ToList());

            return Math.Sqrt((y / TargetValues.Count));
        }

        public double RSquared(List<List<double>> Data, double [] Weights)
        {
            var predict = LinearRegression(Data, Weights);

            double numer = 0, denom = 0, average = TargetValues.Average();

            for (int i = 0; i < TargetValues.Count; i++)
            {
                numer += Math.Pow((TargetValues[i] - predict[i]), 2);
                denom += Math.Pow((TargetValues[i] - average), 2);
            }

            return 1 - numer / denom;
        }


        public double[] LinearRegression(List<List<double>> Data, double[] Weights)
        {
            var result = new double [Data[0].Count];

            double sum;

            for (int i = 0; i < Data[0].Count; i++)
            {
                sum = 0;

                for (int j = 0; j < Data.Count; j++)
                    sum += Data[j][i] * Weights[j];

                result[i] = sum;
            }

            return result;
        }

        public double[] Gradient(Func<List<List<double>>, double[], double> func, List<List<double>> Data, double [] point, double h)
        {
            double[] result = new double[point.Length];
            double[] delta = new double[point.Length];
            
            for (int i = 0; i < point.Length; ++i)
            {
                point.CopyTo(delta, 0);
                delta[i] += h;
                result[i] = ((func(Data, delta) - func(Data, point)) / h);
            }

            return result;
        }

        public double[] AntigradientDescent(Func<List<List<double>>, double [], double> func, List<List<double>> Data,  double[] Weights)
        {
            int k = 1;
            double eps = 10E-5;
            var newWeights = Weights;

            var prev = func(Data, Weights);

            while (true)
            {
                var grad = Gradient(func, Data, newWeights, eps / 10000);
                newWeights = Enumerable.ToArray(newWeights.Select((x, index) => x - grad[index] * eps / k ));

                k++;

                if (Math.Abs(func(Data, newWeights) - prev) < eps)
                  return Enumerable.ToArray(newWeights.Select(x => Math.Round(x, 4)));
                else
                    prev = func(Data, newWeights);
            }
            
        }
    }
}

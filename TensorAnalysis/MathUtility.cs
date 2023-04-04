using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorAnalysis {
    public static class MathUtility {

        public static int Pow(int x, int power) {
            if(power == 0) {
                return 1;
            } else {
                return x * Pow(x, --power);
            }
        }

        public static double Pow(double x, int power) {
            if (power == 0) {
                return 1.0;
            } else {
                return x * Pow(x, --power);
            }
        }

        public static Tensor1 Abs(Tensor1 x) {
            for(int i = 0; i < x.M; ++i) {
                x[i] = Math.Abs(x[i]);
            }
            return x;
        }

        public static Tensor2 Abs(Tensor2 x) {
            Tensor2 t = Tensor2.Zeros(x.M, x.N);
            for (int i = 0; i < x.M; ++i) {
                for(int j = 0; j < x.N; ++j) {
                    t[i,j] = Math.Abs(x[i,j]);
                }
            }
            return t;
        }

        public static double Max(Tensor1 x) {
            double max = double.MinValue;
            for (int i = 0; i < x.M; ++i) {
                double m = x[i];
                if (max < m) {
                    max = m;
                }
            }
            return max;
        }

        public static double Min(Tensor1 x) {
            double min = double.MaxValue;
            for (int i = 0; i < x.M; ++i) {
                double m = x[i];
                if (min > m) {
                    min = m;
                }
            }
            return min;
        }
    }
}

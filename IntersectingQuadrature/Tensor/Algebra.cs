using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Tensor {
    public static class Algebra {

        public static double Sum(Tensor1 t) {
            double s = 0;
            for(int i = 0; i < t.M; ++i) {
                s += t[i];
            }
            return s;
        }

        public static double Determinant(Tensor2 t) {
            double det;
            if(t.N == 3 && t.M ==3) {
                det = t[0, 0] * t[1, 1] * t[2, 2];
                det += t[0, 1] * t[1, 2] * t[2, 0];
                det += t[0, 2] * t[1, 0] * t[2, 1];
                det -= t[0, 2] * t[1, 1] * t[2, 0];
                det -= t[0, 0] * t[1, 2] * t[2, 1];
                det -= t[0, 1] * t[1, 0] * t[2, 2];
            }else if(t.N == 2 && t.M == 2) {
                det = t[0, 0] * t[1, 1];
                det -= t[1, 0] * t[0, 1];
            }else if(t.N == 1 && t.M == 1) {
                det = t[0, 0];
            } else if (t.N == 0 && t.M == 0) {
                det = 1;
            } else {
                throw new NotImplementedException();
            }
            return det;
        }

        public static void Scale(Tensor1 t, double a) {
            for (int i = 0; i < t.M; ++i) {
                t[i] *= a;
            }
        }

        public static void Scale(Tensor2 t, double a) {
            for (int i = 0; i < t.M; ++i) {
                for (int j = 0; j < t.N; ++j) {
                    t[i, j] *= a;
                }
            }
        }

        public static void Scale(Tensor3 t, double a) {
            for (int i = 0; i < t.M; ++i) {
                for (int j = 0; j < t.N; ++j) {
                    for (int k = 0; k < t.O; ++k) {
                        t[i, j, k] *= a;
                    }
                }
            }
        }

        public static Tensor2 Transpose(Tensor2 t) {
            Tensor2 clone = Tensor2.Zeros(t.N, t.M) ;
            for (int i = 0; i < t.M; ++i) {
                for (int j = 0; j < t.N; ++j) {
                    clone[j, i] = t[i, j];
                }
            }
            return clone;
        }

        
        public static Tensor2 Dyadic(Tensor1 a, Tensor1 b) {
            int m = a.M;
            int n = b.M;
            Tensor2 c = Tensor2.Zeros(m, n);
            for (int i = 0; i < m; ++i) {
                for (int j = 0; j < n; ++j) {
                    c[i,j] = a[i] * b[j];
                }
            }
            return c;
        }

        public static int Pow(int x, int power) {
            if (power == 0) {
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
            for (int i = 0; i < x.M; ++i) {
                x[i] = Math.Abs(x[i]);
            }
            return x;
        }

        public static Tensor2 Abs(Tensor2 x) {
            Tensor2 t = Tensor2.Zeros(x.M, x.N);
            for (int i = 0; i < x.M; ++i) {
                for (int j = 0; j < x.N; ++j) {
                    t[i, j] = Math.Abs(x[i, j]);
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

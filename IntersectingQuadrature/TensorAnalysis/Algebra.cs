using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.TensorAnalysis {
    public static class Algebra {

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
    }
}

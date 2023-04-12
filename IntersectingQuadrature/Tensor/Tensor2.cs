using System.Diagnostics;

namespace IntersectingQuadrature.Tensor {
    public class Tensor2 {
        
        double[,] values;

        public int M { get; private set; }

        public int N { get; private set; }

        private Tensor2(int m, int n) {
            values = new double[m, n];
            M = m;
            N = n;
        }

        public static Tensor2 Zeros(int m) {
            return new Tensor2(m, m);
        }

        public static Tensor2 Zeros(int m, int n) {
            return new Tensor2(m, n);
        }

        public static Tensor2 Unit(int m) {
            Tensor2 unit = new Tensor2(m, m);
            for(int i = 0; i < m; ++i){
                unit[i,i] = 1;
            }
            return unit;
        }

        public double this[int i, int j] {
            get { return values[i, j]; }
            set { values[i, j] = value; }
        }

        public Tensor2 Clone() {
            Tensor2 clone = Zeros(M, N);
            for(int i = 0; i < M; ++i) {
                for(int j = 0; j < N; ++j) {
                    clone[i, j] = this[i, j];
                }
            }
            return clone;
        }

        public static Tensor2 operator *(Tensor2 a, Tensor2 b) {
            Debug.Assert(a.N == b.M);
            Tensor2 c = new Tensor2(a.M, b.N);
            for(int i = 0; i < c.M; ++i) {
                for(int j = 0; j < b.N; ++j) {
                    for(int k = 0; k < a.N; ++k) {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return c;
        }

        public static Tensor2 operator +(Tensor2 a, Tensor2 b) {
            Debug.Assert(a.N == b.N && a.M == b.M);
            Tensor2 c = new Tensor2(a.M, a.N);
            for (int i = 0; i < a.M; ++i) {
                for (int j = 0; j < a.N; ++j) {
                    c[i, j] = a[i, j] + b[i, j];
                }
            }
            return c;
        }

        public static Tensor2 operator -(Tensor2 a, Tensor2 b) {
            Debug.Assert(a.N == b.N && a.M == b.M);
            Tensor2 c = new Tensor2(a.M, a.N);
            for (int i = 0; i < a.M; ++i) {
                for (int j = 0; j < a.N; ++j) {
                    c[i, j] = a[i, j] - b[i, j];
                }
            }
            return c;
        }
    }

}

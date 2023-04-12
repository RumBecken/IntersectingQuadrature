using System.Diagnostics;

namespace IntersectingQuadrature.Tensor {
    public class Tensor3 {
        
        double[,,] values;

        public int M { get; private set; }

        public int N { get; private set; }

        public int O { get; private set; }
        
        private Tensor3(int m, int n, int o) {
            values = new double[m, n, o];
            M = m;
            N = n;
            O = o;
        }

        public static Tensor3 Zeros(int m) {
            return new Tensor3(m,m,m);
        }

        public static Tensor3 Zeros(int m, int n, int o) {
            return new Tensor3(m, n, o);
        }

        public double this[int i, int j, int k] {
            get { return values[i, j, k]; }
            set { values[i, j, k] = value; }
        }

        public Tensor3 Clone() {
            Tensor3 clone = Zeros(M, N, O);
            for (int i = 0; i < M; ++i) {
                for (int j = 0; j < N; ++j) {
                    for(int k = 0; k < O; ++k) {
                        clone[i, j, k] = this[i, j, k];
                    }
                }
            }
            return clone;
        }

        public static Tensor3 operator *(double a, Tensor3 b) {
            Tensor3 c = Tensor3.Zeros(b.M, b.N, b.O);
            for (int i = 0; i < b.M; ++i) {
                for (int j = 0; j < b.N; ++j) {
                    for (int k = 0; k < b.O; ++k) {
                        c[i, j, k] = a * b[i, j, k];
                    }
                }
            }
            return c;
        }

        public static Tensor2 operator *(Tensor1 a, Tensor3 b) {
            Debug.Assert(a.M == b.M);
            Tensor2 c = Tensor2.Zeros(b.N, b.O);
            for (int i = 0; i < b.N; ++i) {
                for (int j = 0; j < b.O; ++j) {
                    for (int k = 0; k < a.M; ++k) {
                        c[i, j] += a[k] * b[k, i, j];
                    }
                }
            }
            return c;
        }

        public static Tensor2 operator *(Tensor3 a, Tensor1 b) {
            Debug.Assert(a.O == b.M);
            Tensor2 c = Tensor2.Zeros(a.M, a.N);
            for (int i = 0; i < a.M; ++i) {
                for (int j = 0; j < a.N; ++j) {
                    for (int k = 0; k < a.O; ++k) {
                        c[j, k] += a[i, j, k] * b[k] ;
                    }
                }
            }
            return c;
        }

        public static Tensor3 operator *(Tensor2 a, Tensor3 b) {
            Debug.Assert(a.N == b.M);
            Tensor3 c = Tensor3.Zeros(a.M, b.N, b.O) ;
            for (int i = 0; i < c.M; ++i) {
                for (int j = 0; j < c.N; ++j) {
                    for (int k = 0; k < c.O; ++k) {
                        for(int l = 0; l <a.N; ++l) {
                            c[i, j, k] += a[i,l] * b[l, j, k];
                        }
                    }
                }
            }
            return c;
        }

        public static Tensor3 operator *(Tensor3 a, Tensor2 b) {
            Debug.Assert(a.O == b.M);
            Tensor3 c = Tensor3.Zeros(a.M, a.N, b.N);
            for (int i = 0; i < c.M; ++i) {
                for (int j = 0; j < c.N; ++j) {
                    for (int k = 0; k < c.O; ++k) {
                        for (int l = 0; l < a.O; ++l) {
                            c[i, j, k] += a[i, j, l] * b[l, k];
                        }
                    }
                }
            }
            return c;
        }

        public static Tensor3 operator +(Tensor3 a, Tensor3 b) {
            Debug.Assert(a.M == b.M && a.N == b.N && a.O == b.O);
            Tensor3 c = Zeros(a.M, a.N, a.O);
            for (int i = 0; i < c.M; ++i) {
                for (int j = 0; j < c.N; ++j) {
                    for (int k = 0; k < c.O; ++k) {
                        c[i, j, k] = a[i, j, k] + b[i, j, k];
                    }
                }
            }
            return c;
        }

        public static Tensor3 operator -(Tensor3 a, Tensor3 b) {
            Debug.Assert(a.M == b.M && a.N == b.N && a.O == b.O);
            Tensor3 c = Zeros(a.M, a.N, a.O);
            for (int i = 0; i < c.M; ++i) {
                for (int j = 0; j < c.N; ++j) {
                    for (int k = 0; k < c.O; ++k) {
                        c[i, j, k] = a[i, j, k] - b[i, j, k];
                    }
                }
            }
            return c;
        }
    }

}

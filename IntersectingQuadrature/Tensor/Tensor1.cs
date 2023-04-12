using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Tensor {

    public class Tensor1 {
        
        double[] values;
        
        public int M { get; private set; }
        
        private Tensor1(int m) {
            values = new double[m];
            M = m;
        }

        public static Tensor1 Zeros(int m) {
            return new Tensor1(m);
        }

        public static Tensor1 Ones(int m) {
            Tensor1 t =  new Tensor1(m);
            for(int i = 0; i < m; ++i) {
                t[i] = 1;
            }
            return t;
        }

        public static Tensor1 Vector(double x) {
            Tensor1 c = new Tensor1(1);
            c[0] = x;
            return c;
        }

        public static Tensor1 Vector(double x, double y) {
            Tensor1 c = new Tensor1(2);
            c[0] = x;
            c[1] = y;
            return c;
        }

        public static Tensor1 Vector(double x, double y, double z) {
            Tensor1 c = new Tensor1(3);
            c[0] = x;
            c[1] = y;
            c[2] = z;
            return c;
        }

        public double this[int index] {
            get { return values[index]; }
            set { values[index] = value; }
        }

        public Tensor1 Clone() {
            Tensor1 clone = Zeros(M);
            for (int i = 0; i < M; ++i) {
                  clone[i] = this[i];
            }
            return clone;
        }

        public static Tensor1 operator +(Tensor1 a, Tensor1 b) {
            Debug.Assert(a.M == b.M);
            int m = a.M;
            Tensor1 c = new Tensor1(m);
            for(int i = 0; i < m; ++i) {
                c[i] = a[i] + b[i];
            }
            return c;
        }

        public static Tensor1 operator -(Tensor1 a, Tensor1 b) {
            Debug.Assert(a.M == b.M);
            int m = a.M;
            Tensor1 c = new Tensor1(m);
            for (int i = 0; i < m; ++i) {
                c[i] = a[i] - b[i];
            }
            return c;
        }

        public static Tensor1 operator *(double a, Tensor1 b) {
            int m = b.M;
            Tensor1 c = new Tensor1(m);
            for (int i = 0; i < m; ++i) {
                c[i] = a * b[i];
            }
            return c;
        }

        public static Tensor1 operator *(Tensor1 a, double b) {
            return b * a;
        }

        public static double operator *(Tensor1 a, Tensor1 b) {
            Debug.Assert(a.M == b.M);
            int m = b.M;
            double c = 0;
            for (int i = 0; i < m; ++i) {
                c += a[i] * b[i];
            }
            return c;
        }

        public static Tensor1 operator *(Tensor2 a, Tensor1 b) {
            Debug.Assert(a.N == b.M);
            int m = a.M;
            Tensor1 c = new Tensor1(m);
            for (int d = 0; d < m; ++d) {
                for (int i = 0; i < a.N; ++i) {
                    c[d] += a[d, i] * b[i];
                }
            }
            return c;
        }

        public static Tensor1 operator *(Tensor1 a, Tensor2 b) {
            Debug.Assert(a.M == b.M);
            int m = b.N;
            Tensor1 c = new Tensor1(m);
            for (int d = 0; d < m; ++d) {
                for (int i = 0; i < a.M; ++i) {
                    c[d] += a[i] * b[i, d];
                }
            }
            return c;
        }

        public static Tensor1 operator /(Tensor1 a, double b) {
            int m = a.M;
            Tensor1 c = new Tensor1(m);
            for (int i = 0; i < m; ++i) {
                c[i] = a[i] / b;
            }
            return c;
        }
    }

}

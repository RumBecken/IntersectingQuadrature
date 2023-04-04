using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Mapper {
    internal class Line : IHeightFunctionY {
        double m;

        public Line(double m) {
            this.m = m;
        }

        public double Y(double x) {
            return m;
        }

        public (double Y, double DxY) YdY(double x) {
            return (m, 0);
        }

        public (double Y, double DxY, double DxxY) YdYddY(double x) {
            return (m, 0, 0);
        }
    }
}

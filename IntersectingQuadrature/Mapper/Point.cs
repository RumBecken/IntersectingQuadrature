using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Mapper {
    internal class Point : IHeightFunctionX {
        double m;

        public Point(double m) {
            this.m = m;
        }

        public double X() {
            return m;
        }
    }
}

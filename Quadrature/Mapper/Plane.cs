using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Mapper {
    internal class Plane : IHeightFunctionZ {
        double m;

        public Plane(double m) {
            this.m = m;
        }

        public double Z(double x, double y) {
            return m;
        }

        public (double Z, double DxZ, double DyZ) ZdZ(double x, double y) {
            return (m, 0, 0);
        }

        public (double Z, double DxZ, double DyZ, double DxxZ, double DxyZ, double DyyZ) ZdZddZ(double x, double y) {
            return (m, 0, 0, 0, 0, 0);
        }
    }
}

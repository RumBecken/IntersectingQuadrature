using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Mapper {
    interface IHeightFunctionZ {
        double Z(double x, double y);

        (double Z, double DxZ, double DyZ) ZdZ(double x, double y);

        (double Z, double DxZ, double DyZ, double DxxZ, double DxyZ, double DyyZ) ZdZddZ(double x, double y);
    }
}

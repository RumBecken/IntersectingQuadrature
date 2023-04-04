using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Mapper {
    internal class Heights {

        public IHeightFunctionX x;
        public IHeightFunctionY y;
        public IHeightFunctionZ z;

        public void SetX(IHeightFunctionX x) {
            this.x = x;
        }

        public void SetY(IHeightFunctionY y) {
            this.y = y;
        }

        public void SetZ(IHeightFunctionZ z) {
            this.z = z;
        }
    }
}

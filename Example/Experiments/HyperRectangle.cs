using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Experiments {
    internal class HyperRectangle : IHyperRectangle {
        public int Dimension { get; private set; }

        public Tensor1 Center { get; set; }

        public Tensor1 Diameters { get; set; }

        public HyperRectangle(int dim) {
                Dimension = dim;
                Center = Tensor1.Zeros(dim);
                Diameters = Tensor1.Zeros(dim);
            }

        public static HyperRectangle UnitCube(int dim) {
            HyperRectangle cube = new HyperRectangle(dim);
            for(int i = 0; i < dim; ++i) {
                cube.Diameters[i] = 2;
            }
            return cube;
        }
    }
}

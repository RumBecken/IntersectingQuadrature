using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Map;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {

    public class EmbeddedHyperRectangle : HyperRectangle, IEmbeddedHyperRectangle {

        public int SpaceDimension { get; set; }

        public BitArray ActiveDimensions { get; set; }

        public EmbeddedHyperRectangle(int spaceDimension) : base(spaceDimension){
            SpaceDimension = spaceDimension;
            ActiveDimensions = new BitArray(spaceDimension);
        }

        public static EmbeddedHyperRectangle UnitCube(int dim) {
            EmbeddedHyperRectangle cube = new EmbeddedHyperRectangle(dim);
            for (int i = 0; i < dim; ++i) {
                cube.Diameters[i] = 2;
                cube.ActiveDimensions[i] = true;
            }
            return cube;
        }

    }
}

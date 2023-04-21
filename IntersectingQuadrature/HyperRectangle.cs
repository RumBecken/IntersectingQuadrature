using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {

    public class HyperRectangle : IHyperRectangle {

        public int SpaceDimension { get; set; }

        public int BodyDimension { get; set; }

        public Tensor1 Center { get; set; }

        public Tensor1 Diameters { get; set; }

        public BitArray ActiveDimensions { get; set; }

        public HyperRectangle(int spaceDimension) {
            this.SpaceDimension = spaceDimension;
            ActiveDimensions = new BitArray(spaceDimension);
            Center = Tensor1.Zeros(spaceDimension);
            Diameters = Tensor1.Zeros(spaceDimension);
        }

    }
}

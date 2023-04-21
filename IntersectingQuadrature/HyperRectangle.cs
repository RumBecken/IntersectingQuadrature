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

        public HyperRectangle Face(int direction, Symbol sign) {
            if (ActiveDimensions[direction] == false) {
                throw new ArgumentException("Height direction not active.");
            }
            HyperRectangle face = Clone();
            --face.BodyDimension;
            face.Center[direction] = (int)sign * Diameters[direction] / 2.0 + Center[direction];
            face.Diameters[direction] = 0;
            face.ActiveDimensions[direction] = false;

            return face;
        }

        public void Resize(double coordinate, int direction, Symbol side) {
            if (ActiveDimensions[direction] == true) {
                double a = Center[direction] - (int)side * Diameters[direction] / 2.0;
                double b = coordinate;

                Center[direction] = (a + b) / 2;
                Diameters[direction] = Math.Abs(a - b);
            } else {
                throw new ArgumentException("Only active dimension can be resized");
            }
        }

        public void Relocate(double coordinate, int direction) {
            Center[direction] = coordinate;
        }

        public HyperRectangle Clone() {
            HyperRectangle clone = new HyperRectangle(SpaceDimension);
            clone.BodyDimension = BodyDimension;
            for(int i = 0; i < clone.SpaceDimension; ++i) {
                clone.ActiveDimensions[i] = ActiveDimensions[i];
                clone.Center[i] = Center[i];
                clone.Diameters[i] = Diameters[i];
            }
            return clone;
        }
    }
}

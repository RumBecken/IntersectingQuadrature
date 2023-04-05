using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {

    public class HyperRectangle {

        public int Codimension;

        public int Dimension;
        
        public Tensor1 Center;

        public Tensor1 Diameters;

        public BitArray ActiveDimensions;

        public HyperRectangle(int codimension) {
            this.Codimension = codimension;
            ActiveDimensions = new BitArray(codimension);
            Center = Tensor1.Zeros(codimension);
            Diameters = Tensor1.Zeros(codimension);
        }

        public HyperRectangle Face(int direction, Symbol sign) {
            if (ActiveDimensions[direction] == false) {
                throw new ArgumentException("Height direction not active.");
            }
            HyperRectangle face = Clone();
            --face.Dimension;
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
            HyperRectangle clone = new HyperRectangle(Codimension);
            clone.Dimension = Dimension;
            for(int i = 0; i < clone.Codimension; ++i) {
                clone.ActiveDimensions[i] = ActiveDimensions[i];
                clone.Center[i] = Center[i];
                clone.Diameters[i] = Diameters[i];
            }
            return clone;
        }
    }
}

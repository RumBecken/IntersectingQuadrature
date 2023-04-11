using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature {

    public class UnitCube : HyperRectangle {

        public UnitCube(int dimension) : base(dimension){
            BodyDimension = dimension;
            for(int d = 0; d < dimension; ++d) {
                ActiveDimensions[d] = true;
                Diameters[d] = 2; 
            }
        }
    } 
}

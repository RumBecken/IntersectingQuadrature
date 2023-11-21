using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature {

    /// <summary>
    /// A simple multidimensional box
    /// </summary>
    public class HyperRectangle : IHyperRectangle {
        
        /// <inheritdoc/>
        public int Dimension { get; set; }

        /// <inheritdoc/>
        public Tensor1 Center { get; set; }

        /// <inheritdoc/>
        public Tensor1 Diameters { get; set; }

        /// <summary>
        /// Initializes a HyperactAngle with Center = 0 and Diameters = 0.
        /// </summary>
        /// <param name="dim">Dimension of the Hyperrectangle</param>
        public HyperRectangle(int dim) {
                Dimension = dim;
                Center = Tensor1.Zeros(dim);
                Diameters = Tensor1.Zeros(dim);
            }

        /// <summary>
        /// Initializes a UnitCube [-1,1]^ <paramref name="dim"/>
        /// </summary>
        /// <param name="dim"></param>
        /// <returns>HyperactAngle with Center = 0 and Diameters = 2.</returns>
        public static HyperRectangle UnitCube(int dim) {
            HyperRectangle cube = new HyperRectangle(dim);
            for(int i = 0; i < dim; ++i) {
                cube.Diameters[i] = 2;
            }
            return cube;
        }
    }
}

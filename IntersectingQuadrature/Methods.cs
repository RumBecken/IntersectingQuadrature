using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature {

    /// <summary>
    /// Methods to construct tensorized quadrature rules on subdomains of a hyperrectangle bounded by one or two level sets.
    /// The quadrature methods are documented in the paper https://arxiv.org/abs/2308.10698 .
    /// </summary>
    public static class Methods {

        /// <summary>
        /// Creates a quadrater that subdivides the hyperrectangle when it can not find a graph of the level set isocontour.
        /// On the deepest level of subdivisions, the level set will be approximated by a linear function if necessary. 
        /// The deepest level of subdivisions is 4.
        /// </summary>
        /// <returns>A quadrater with a bounded level of subdivisions</returns>
        public static IQuadrater Create() {
            return new Quadrater();
        }

        /// <summary>
        /// Creates a quadrater that subdivides the hyperrectangle when it can not find a graph of the level set isocontour.
        /// On the deepest level of subdivisions, the level set will be approximated by a linear function if necessary.
        /// </summary>
        /// <param name="maxSubdivisions"> Deepest level of subdivisions</param>
        /// <returns>A quadrater with a bounded level of subdivisions.</returns>
        public static IQuadrater Create(int maxSubdivisions) {
            return new Quadrater(maxSubdivisions);
        }

        /// <summary>
        /// A quadrater with a bounded level of subdivisions.
        /// The quadrater subdivides the hyperrectangle when it can not find a graph of the level set isocontour.
        /// On the deepest level of subdivisions, the level set will be approximated by a linear function if necessary.
        /// </summary>
        /// <param name="tau"></param>
        /// <returns></returns>
        public static IQuadrater Adaptive(double tau) {
            return new AdaptiveQuadrater(tau);
        }
    }
}

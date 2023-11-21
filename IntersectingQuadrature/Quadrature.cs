using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    
    /// <summary>
    /// Methods to numerically evaluate definite integrals
    /// </summary>
    public static class Quadrature {

        /// <summary>
        /// Numerically evaluates a definite integral of a function
        /// </summary>
        /// <param name="f">Function of integral</param>
        /// <param name="rule">Quadrature rule covering the domain of the integral</param>
        /// <returns>Value of integral</returns>
        public static double Evaluate(IScalarFunction f, QuadratureRule rule) {
            //Kahan Summation
            double s = 0;
            double c = 0;
            foreach (QuadratureNode node in rule) {
                double y = f.Evaluate(node.Point) * node.Weight - c;
                double t = s + y;
                double z = t - s;
                c = z - y;
                s = t;
            }
            return s;
        }

        /// <summary>
        /// Numerically evaluates a definite integral of a function embedded in a 2 dimensional grid
        /// </summary>
        /// <param name="f">Function of integral</param>
        /// <param name="rules">2 dimensional grid of quadrature rules covering the domain of the integral</param>
        /// <returns>Value of integral</returns>
        public static double Evaluate(IScalarFunction f, QuadratureRule[,] rules) {
            //Kahan Summation
            double s = 0;
            double c = 0;
            foreach (QuadratureRule rule in rules) {
                foreach (QuadratureNode node in rule) {
                    double y = f.Evaluate(node.Point) * node.Weight - c;
                    double t = s + y;
                    double z = t - s;
                    c = z - y;
                    s = t;
                }
            }
            return s;
        }

        /// <summary>
        /// Numerically evaluates a definite integral of a function embedded in a 3 dimensional grid
        /// </summary>
        /// <param name="f">Function of integral</param>
        /// <param name="rules">3 dimensional grid of quadrature rules covering the domain of the integral</param>
        /// <returns>Value of integral</returns>
        public static double Evaluate(IScalarFunction f, QuadratureRule[,,] rules) {
            //Kahan Summation
            double s = 0;
            double c = 0;
            foreach (QuadratureRule rule in rules) {
                foreach (QuadratureNode node in rule) {
                    double y = f.Evaluate(node.Point) * node.Weight - c;
                    double t = s + y;
                    double z = t - s;
                    c = z - y;
                    s = t;
                }
            }
            return s;
        }
    }
}

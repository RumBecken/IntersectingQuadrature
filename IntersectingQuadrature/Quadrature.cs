using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    public static class Quadrature {
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

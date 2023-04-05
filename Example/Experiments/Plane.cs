using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature;
using IntersectingQuadrature.TensorAnalysis;

namespace Example.Experiments {
    
    internal static class Plane3D {
        public static Plane XZ(double distanceToOrigin = 0) {
            Tensor1 normal = Tensor1.Vector( 0, 1, 0 );
            Tensor1 point = Tensor1.Vector(0, distanceToOrigin,0);
            return new Plane(normal, point);
        }

        public static Plane XY(double distanceToOrigin = 0) {
            Tensor1 normal = Tensor1.Vector( 0, 0, 1);
            Tensor1 point = Tensor1.Vector( 0, 0, distanceToOrigin);
            return new Plane(normal, point);
        }

        public static Plane YZ(double distanceToOrigin = 0) {
            Tensor1 normal = Tensor1.Vector(1, 0, 0);
            Tensor1 point = Tensor1.Vector(distanceToOrigin, 0, 0);
            return new Plane(normal, point);
        }
    } 
    
    internal class Plane : IScalarFunction {

        Tensor1 normal;

        Tensor1 point;

        public int M { get; private set; }

        public Plane(Tensor1 normal, Tensor1 point) {
            this.normal = normal;
            this.point = point;
            M = normal.M;
        }

        public double Evaluate(Tensor1 x) {
            Tensor1 y = x - point;
            double phi = normal * y; 
            return phi;
        }

        Tensor1 Gradient(Tensor1 x) {
            return normal;
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
            double evaluation = Evaluate(x);
            Tensor1 gradient = Gradient(x);
            return (evaluation, gradient);
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(x);
            Tensor2 hessian = Tensor2.Zeros(M,M);
            return (evaluation, gradient, hessian);
        }
    }
}

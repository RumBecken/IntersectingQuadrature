using IntersectingQuadrature.Tensor;

namespace Example.Experiments.Shapes
{
    class Volcano : IScalarFunction
    {
        public int M => 3;

        int p = 10;

        double r;
        public Volcano(double r)
        {
            this.r = r;
        }

        public Volcano()
        {
            r = 2.2;
        }

        public double Evaluate(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            return x * x + y * y - 1.0/ ((z + r) * (z + r));
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            double dx = 2*x;
            double dy = 2*y;
            double dz = 2.0 / ((z + r) * (z + r) * (z + r));
            return (Evaluate(X), Tensor1.Vector(dx, dy, dz));
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            Tensor2 hessian = Tensor2.Zeros(3);
            hessian[0, 0] = 2;
            hessian[0, 1] = 0;
            hessian[0, 2] = 0;

            hessian[1, 0] = hessian[0, 1];
            hessian[1, 1] = 2;
            hessian[1, 2] = 0;

            hessian[2, 0] = hessian[0, 2];
            hessian[2, 1] = hessian[1, 2];
            hessian[2, 2] = - 6.0 / ((z + r) * (z + r) * (z + r) * (z + r));

            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(X);
            return (evaluation, gradient, hessian);
        }
    }
}


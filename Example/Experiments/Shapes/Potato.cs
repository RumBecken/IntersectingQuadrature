using IntersectingQuadrature.Tensor;

namespace Example.Experiments.Shapes
{
    class Potato : IScalarFunction
    {
        public int M => 3;

        int p = 10;

        double r;
        public Potato(double r)
        {
            this.r = r;
        }

        public Potato()
        {
            this.r = 0.5;
        }

        public double Evaluate(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            return x * x + y * y + z * z - Algebra.Pow(r + 0.1 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z), 2);
        }

        public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            double dx = 2 * x - 2 * (r + 0.1 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z)) * p * 0.1 * Math.Cos(p * x) * Math.Sin(p * y) * Math.Sin(p * z);
            double dy = 2 * y - 2 * (r + 0.1 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z)) * p * 0.1 * Math.Sin(p * x) * Math.Cos(p * y) * Math.Sin(p * z);
            double dz = 2 * z - 2 * (r + 0.1 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z)) * p * 0.1 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Cos(p * z);
            return (Evaluate(X), Tensor1.Vector(dx, dy, dz));
        }

        public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 X)
        {
            double x = X[0];
            double y = X[1];
            double z = X[2];
            Tensor2 hessian = Tensor2.Zeros(3);
            hessian[0, 0] = 2
                - 0.02 * p * p * Math.Cos(p * x) * Math.Sin(p * y) * Math.Sin(p * z) * Math.Cos(p * x) * Math.Sin(p * y) * Math.Sin(p * z)
                + p * p * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z) * (0.2 + 0.02 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z));
            hessian[0, 1] = p * p * Math.Cos(p * x) * Math.Cos(p * y) * Math.Sin(p * z) * (-0.2 - 0.04 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z));
            hessian[0, 2] = p * p * Math.Cos(p * x) * Math.Sin(p * y) * Math.Cos(p * z) * (-0.2 - 0.04 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z));

            hessian[1, 0] = hessian[0, 1];
            hessian[1, 1] = 2 + p * p * Math.Sin(p * x) * Math.Sin(p * z) * (0.2 * Math.Sin(p * y)
                - 0.02 * Math.Cos(2 * p * y) * Math.Sin(p * x) * Math.Sin(p * z));
            hessian[1, 2] = p * p * Math.Sin(p * x) * Math.Cos(p * y) * Math.Cos(p * z) * (-0.2 - 0.04 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Sin(p * z)); ;

            hessian[2, 0] = hessian[0, 2];
            hessian[2, 1] = hessian[1, 2];
            hessian[2, 2] = 2 + p * p * Math.Sin(p * x) * Math.Sin(p * y) * (0.2 * Math.Sin(p * z)
                - 0.02 * Math.Sin(p * x) * Math.Sin(p * y) * Math.Cos(2 * p * z)); ;

            (double evaluation, Tensor1 gradient) = EvaluateAndGradient(X);
            return (evaluation, gradient, hessian);
        }
    }
}


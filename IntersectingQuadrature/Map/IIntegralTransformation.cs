using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map
{
    internal interface IIntegralTransformation : IVectorFunction
    {
        (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 tilde);
    }
}

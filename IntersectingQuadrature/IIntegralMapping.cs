﻿using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    internal interface IIntegralMapping : IVectorFunction {
        (double J, Tensor1 X) EvaluateAndDeterminant(Tensor1 tilde);
    }
}

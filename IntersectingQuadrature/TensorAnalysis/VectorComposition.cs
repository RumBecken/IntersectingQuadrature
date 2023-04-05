using System.Diagnostics;

namespace IntersectingQuadrature.TensorAnalysis {
    public class VectorComposition : IVectorFunction {

        IVectorFunction injection;

        IVectorFunction f;

        public int M { get; private set; }

        public int N { get; private set; }

        public VectorComposition(IVectorFunction f, IVectorFunction injection) {
            this.f = f;
            this.injection = injection;
            
            Debug.Assert(injection.N == f.M);
            M = injection.M;
            N = f.N;
        }

        public Tensor1 Evaluate(Tensor1 x) {
            Tensor1 tilde = injection.Evaluate(x);
            return f.Evaluate(tilde);
        }

        public (Tensor1 evaluation, Tensor2 jacobian) EvaluateAndJacobian(Tensor1 x) {
            (Tensor1 tilde , Tensor2 jinjection ) = injection.EvaluateAndJacobian(x);
            (Tensor1 evaluation, Tensor2 jf) = f.EvaluateAndJacobian(tilde);
            Tensor2 jacobian = jf * jinjection;
            return (evaluation, jacobian);
        }

        public (Tensor1 evaluation, Tensor2 jacobian, Tensor3 hessian) EvaluateAndJacobianAndHessian(Tensor1 x) {
            (Tensor1 tilde, Tensor2 jinjection, Tensor3 hinjection) = injection.EvaluateAndJacobianAndHessian(x);
            (Tensor1 evaluation, Tensor2 jf, Tensor3 hf) = f.EvaluateAndJacobianAndHessian(tilde);
            Tensor2 jacobian = jf * jinjection;
            Tensor3 hessian = D(hf, jinjection) + jf * hinjection;
            return (evaluation, jacobian, hessian);
        }

        static Tensor3 D(Tensor3 hf, Tensor2 jI) {
            Tensor2 hif = Tensor2.Zeros(jI.M);
            Tensor2 jIT = Algebra.Transpose(jI);
            Tensor3 d = Tensor3.Zeros(hf.M, jIT.M, jI.N);
            for(int i = 0; i < d.M; ++i) {
                for(int j = 0; j < d.N; ++j) {
                    for(int k = 0; k < d.O; ++k) {
                        hif[j, k] = hf[i, j, k];
                    }
                }
                Tensor2 di = jIT * hif * jI;
                for (int j = 0; j < d.N; ++j) {
                    for (int k = 0; k < d.O; ++k) {
                        d[i, j, k] = di[j, k];
                    }
                }
            }
            return d;
        }
    }
}

using IntersectingQuadrature.Tensor;
using System.Collections.Generic;

namespace IntersectingQuadrature {

    /// <summary>
    /// List of <see cref="QuadratureNode"/>s which can be used to numerically evaluate an integral. 
    /// </summary>
    public class QuadratureRule : List<QuadratureNode>{

        /// <summary>
        /// Initializes a <see cref="QuadratureRule"/> that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity </param>
        public QuadratureRule(int capacity) : base(capacity){

        }

        /// <summary>
        /// Creates a <see cref="QuadratureRule"/> containing new <see cref="QuadratureNode"/>s.
        /// </summary>
        /// <param name="count">Number of <see cref="QuadratureNode"/>s</param>
        /// <param name="dim">Dimension of the <see cref="QuadratureNode"/>s</param>
        /// <returns></returns>
        public static QuadratureRule Allocate(int count, int dim) {
            QuadratureRule rule = new QuadratureRule(count);
            for(int i = 0; i < count; ++i) {
                rule.Add(new QuadratureNode() {
                    Point = Tensor1.Zeros(dim),
                    Weight = 0
                });
            }
            return rule;
        }
    }
}

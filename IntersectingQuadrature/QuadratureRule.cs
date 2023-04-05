using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {

    public class QuadratureRule : List<QuadratureNode>{

        public QuadratureRule(int capacity) : base(capacity){

        }

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

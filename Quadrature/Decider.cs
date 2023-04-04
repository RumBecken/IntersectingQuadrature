using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorAnalysis;

namespace IntersectingQuadrature {
    class Decider {
        
        double epsilon = 1e-12;

        public Decider(double epsilon) {
            this.epsilon = epsilon;
        }

        public Symbol Sign(double x) {
            if (x > epsilon) {
                return Symbol.Plus;
            } else if (x < -epsilon) {
                return Symbol.Minus;
            } else {
                return Symbol.Zero;
            }
        }

        public Symbol Sign(Tensor1 x) {
            if(x.M == 0) {
                throw new ArgumentException("Length must be greater than 0");
            }
            Symbol sign = Sign(x[0]);
            for(int i = 1; i < x.M; ++i) {
                Symbol s = Sign(x[i]);
                if(s != sign) {
                    return Symbol.None;
                }
            }
            return sign;
        }

        public (Symbol max, Symbol min) MaxMinSign(Tensor1 x) {
            if (x.M == 0) {
                throw new ArgumentException("Length must be greater than 0");
            }
            double max = MathUtility.Max(x);
            double min = MathUtility.Min(x);

            return (Sign(max), Sign(min));
        }
    }
}

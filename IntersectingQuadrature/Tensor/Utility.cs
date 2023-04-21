using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Tensor {
    internal static class Utility {

        public static int IndexOfMaxEntry(Tensor1 x) {
            double max = double.MinValue;
            int index = -1;
            for (int i = 0; i < x.M; ++i) {
                double abs = Math.Abs(x[i]);
                if (max < abs) {
                    max = abs;
                    index = i;
                }
            }
            return index;
        }

        public static int IndexOfMaxEntry(BitArray mask, Tensor1 x) {
            double max = double.MinValue;
            int index = -1;
            for (int i = 0; i < x.M; ++i) {
                if (mask[i]) {
                    double abs = Math.Abs(x[i]);
                    if (max < abs) {
                        max = abs;
                        index = i;
                    }
                }
            }
            return index;
        }
    }
}

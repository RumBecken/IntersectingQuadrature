using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature {
    internal class DomainComparer : IComparer<Set> {
        public int Compare(Set x, Set y) {
            if (Equals(x.Geometry.ActiveDimensions, y.Geometry.ActiveDimensions)) {
                return 0;
            } else {
                Axis direction = y.HeightDirection;
                if (x.Geometry.Center[(int)direction] < y.Geometry.Center[(int)direction]) {
                    return -1;
                } else {
                    return +1;
                }
            }
        }

        bool Equals(BitArray a, BitArray b) {
            bool equal = true;
            for (int i = 0; i < a.Count; ++i) {
                equal &= a[i] == b[i];
                if (!equal) {
                    return false;
                }
            }
            return true;
        }
    }
}

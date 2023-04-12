using System.Collections.Generic;
using System.Diagnostics;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature {
    internal class Organizer {

        IRootFinder rooter;

        static Decider decider = new Decider(Environment.Epsilon);

        public Organizer(IRootFinder rooter) {
            this.rooter = rooter;
        }

        public LinkedList<NestedSet> Sort(NestedSet space) {
            LinkedList<NestedSet> domains = new LinkedList<NestedSet>();
            domains.AddFirst(space.Clone());

            LinkedListNode<NestedSet> node = domains.First;
            while (node != null) {
                NestedSet domain = node.Value;
                Iterate(domain, domains);
                node = node.Next;
            }
            return domains;
        }

        void Iterate(NestedSet space, LinkedList<NestedSet> spaces) {
            int height = space.Root.Height();
            for (int h = height - 1; h > -1; --h) {
                LinkedList<BinaryNode<Set>> level = space.Root.Descendants(h); 
                foreach(BinaryNode<Set> subspace in level) {
                    Sort(subspace, space, spaces);
                }
            }
        }

        private void Sort(BinaryNode<Set> subspace, NestedSet space, LinkedList<NestedSet> spaces) {
            Set subdomain = subspace.Value;
            if (subdomain.Sign == Symbol.None) {
                Debug.Assert(subspace.FirstChild != null || subspace.SecondChild != null);
                
                Set bottom = subspace.FirstChild.Value;
                Set top = subspace.SecondChild.Value;

                Debug.Assert(bottom.Sign != Symbol.None && top.Sign != Symbol.None);
                Debug.Assert(!(bottom.Sign == Symbol.Zero && top.Sign == Symbol.Zero));
                
                if ((bottom.Sign == Symbol.Plus && top.Sign == Symbol.Minus)
                    || (bottom.Sign == Symbol.Minus && top.Sign == Symbol.Plus)) {

                    double insertion = Separator(space.Alpha, bottom.Geometry, top.Geometry, subdomain.HeightDirection);
                    
                    Symbol temp = top.Sign;

                    NestedSet split = Split(space, subspace, insertion);
                    split.TryFind(subdomain, out BinaryNode<Set> splitSubspace);
                    
                    splitSubspace.Value.Sign = temp;
                    splitSubspace.FirstChild.Value.Sign = Symbol.Zero;

                    subdomain.Sign = bottom.Sign;
                    top.Sign = Symbol.Zero;

                    spaces.AddLast(split);
                } else if(bottom.Sign != Symbol.Zero) {
                    subdomain.Sign = bottom.Sign;
                } else {
                    subdomain.Sign = top.Sign;
                }
            }
        }

        static void SetSign(IScalarFunction alpha, Set domain) {
            double a = alpha.Evaluate(domain.Geometry.Center);
            Symbol sign = decider.Sign(a);
            domain.Sign = sign;
        }

        double Separator(IScalarFunction alpha, HyperRectangle a, HyperRectangle b, Axis direction) {
            Tensor1 root = rooter.Root(alpha, a.Center, b.Center);
            return root[(int)direction];
        }

        static NestedSet Split(NestedSet space, BinaryNode<Set> subSpace, double coordinate) {
            int depth = subSpace.Depth();
            Axis direction = subSpace.Value.HeightDirection;
            NestedSet clone = space.Clone();

            foreach (BinaryNode<Set> sibling in space.Root.Descendants(depth)) {
                if(sibling.SecondChild != null) {
                    if (sibling.Value.Sign != Symbol.None) {
                        sibling.SecondChild.Value.Sign = sibling.Value.Sign;
                    } else {
                        sibling.SecondChild.Value.Geometry.Center[(int)direction] = coordinate;
                        SetSign(space.Alpha, sibling.SecondChild.Value);
                    }
                }
                
            }
            foreach (BinaryNode<Set> ancestor in space.Root.Descendants(0, depth)) {
                ancestor.Value.Resize(coordinate, direction, Symbol.Plus);
            };

            foreach (BinaryNode<Set> sibling in clone.Root.Descendants(depth)) {
                if (sibling.FirstChild != null) {
                    sibling.FirstChild.Value.Geometry.Center[(int)direction] = coordinate;
                    if (sibling.Value.Sign != Symbol.None) {
                        sibling.FirstChild.Value.Sign = sibling.Value.Sign;
                    } else {
                        SetSign(space.Alpha, sibling.FirstChild.Value);
                    }
                }
            }
            foreach (BinaryNode<Set> ancestor in clone.Root.Descendants(0, depth)) {
                ancestor.Value.Resize(coordinate, direction, Symbol.Minus);
            };
            return clone;
        }
    }
}
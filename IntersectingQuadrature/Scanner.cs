using IntersectingQuadrature.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {
    internal static class Scanner {

        static Decider decider = new Decider(Environment.Epsilon);

        public static double C = 0.1;

        public static LinkedList<NestedSet> FindSets(IScalarFunction alpha, HyperRectangle geometry, int maxSubdivisions) {
            LinkedList<NestedSet> spaces = new LinkedList<NestedSet>();
            NestedSet cube = new NestedSet(alpha, geometry);
            FindSubSets(cube, spaces, maxSubdivisions);
            return spaces;
        }

        public static int BruteForceCount;

        public static int SplitCount;

        static void FindSubSets(NestedSet set, LinkedList<NestedSet> sets, int subdivisions) {
            SetList subSets = set.LowestLeafs();
            bool success = FindFaces(subSets, set.Alpha);
            if (success) {
                sets.AddLast(set);
            }
            else {
                ++SplitCount;
                if (subdivisions > 0){
                    Split(set, out NestedSet half);
                    --subdivisions;
                    FindSubSets(set, sets, subdivisions);
                    FindSubSets(half, sets, subdivisions);
                } else {
                    BruteForceCount++;
                    Tensor1 center = set.Root.Value.Geometry.Center;
                    (double a, Tensor1 b) = set.Alpha.EvaluateAndGradient(center);
                    a -= b * center;
                    set.Alpha = new LinearPolynomial(a, b);
                    set.Root.RemoveChildren();
                    set.Root.Value.Sign = Symbol.None;
                    set.Root.Value.HeightDirection = Axis.None;
                    FindSubSets(set, sets, subdivisions);
                }
            }
        }

        public static void Split(NestedSet source, out NestedSet target) {
            //SetList leafs = source.LowestLeafs();
            //Set leaf = leafs.First.Value.Value;
            //int direction = IndexOfMaxEntry(leaf.Geometry.Diameters);
            int direction = IndexOfMaxEntry(source.Root.Value.Geometry.Diameters);
            double coordinate = source.Root.Value.Geometry.Center[direction];
            target = Split(source, (Axis)direction, coordinate);
        }

        static NestedSet Split(NestedSet set, Axis direction, double coordinate) {
            NestedSet half = set.Clone();
            Resizer.Resize(set.Root, direction, coordinate, Symbol.Plus);
            Resizer.Resize(half.Root, direction, coordinate, Symbol.Minus);
            return half;
        }

        static int IndexOfMaxEntry(Tensor1 x) {
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

        static bool FindFaces(SetList faces, IScalarFunction alpha) {
            if (faces.Dimension > 0) {
                foreach (BinaryNode<Set> face in faces) {
                    
                    Tensor1 center = face.Value.Geometry.Center;
                    (double a, Tensor1 b, Tensor2 c) = alpha.EvaluateAndGradientAndHessian(center);
                    face.Value.Alpha = new QuadraticPolynomial(a, b, c);
                    TrySetSign(face.Value, alpha);
                }
                RemoveFacesWithSignFrom(faces);

                if (faces.Count > 0) {
                    Axis heightDirection = FindHeightDirection(faces);
                    foreach (BinaryNode<Set> face in faces) {
                        if (!IsMonotoneIn(alpha, heightDirection, face.Value)) {
                            return false;
                        }
                    }
                    foreach (BinaryNode<Set> face in faces) {
                        face.Value.HeightDirection = heightDirection;
                    }
                    SetList subspace = AddFaceLayerTo(faces, heightDirection);
                    return FindFaces(subspace, alpha);
                } else {
                    return true;
                }
            } else {
                foreach (BinaryNode<Set> face in faces) {
                    SetSign(alpha, face.Value);
                }
                return true;
            }
        }

        static void RemoveFacesWithSignFrom(SetList space) {
            LinkedListNode<BinaryNode<Set>> node = space.First;
            while (node != null) {
                LinkedListNode<BinaryNode<Set>> nextNode = node.Next;
                if (node.Value.Value.Sign != Symbol.None) {
                    space.Remove(node);
                }
                node = nextNode;
            }
        }

        static SetList AddFaceLayerTo(SetList space, Axis heightDirection) {
            SetList subspace = new SetList(space.Dimension - 1); 
            foreach(BinaryNode<Set> face in space) {
                Set topDomain = face.Value.Face( heightDirection, Symbol.Plus);
                BinaryNode<Set> top = new BinaryNode<Set>(topDomain);
                top.Parent = face;
                face.SecondChild =  top;
                subspace.AddLast(top);

                Set bottomDomain = face.Value.Face( heightDirection, Symbol.Minus);
                BinaryNode<Set> bottom = new BinaryNode<Set>(bottomDomain);
                face.FirstChild = bottom;
                bottom.Parent = face;
                subspace.AddLast(bottom);
            }
            return subspace;
        }

        static Axis FindHeightDirection(SetList space) {
            int heightDirection = -1;
            double maxAbsSquared = double.MinValue;
            foreach (BinaryNode<Set> face in space) {
                (double e, Tensor1 g) = face.Value.Alpha.EvaluateAndGradient(Tensor1.Zeros(face.Value.Alpha.M));
                Tensor1 gradient = ProjectGradientTo(face.Value.Geometry, g);
                double absSquared = gradient * gradient;
                if (absSquared > maxAbsSquared) {
                    maxAbsSquared = absSquared;
                    heightDirection = IndexOfMaxEntryOn(face.Value.Geometry, gradient);
                }
            }
            return (Axis) heightDirection;
        }

        static Tensor1 ProjectGradientTo(HyperRectangle face, Tensor1 gradient) {
            for(int i = 0; i < gradient.M; ++i) {
                if (!face.ActiveDimensions[i]) {
                    gradient[i] = 0;
                }
            }
            return gradient;
        }

        static int IndexOfMaxEntryOn(HyperRectangle face, Tensor1 x) {
            double max = double.MinValue;
            int index = -1;
            for (int i = 0; i < x.M; ++i) {
                if (face.ActiveDimensions[i]) {
                    double abs = Math.Abs(x[i]);
                    if (max < abs) {
                        max = abs;
                        index = i;
                    }
                }
            }
            return index;
        }

        static void SetSign(IScalarFunction alpha, Set domain) {
            double a = alpha.Evaluate(domain.Geometry.Center);
            Symbol sign = decider.Sign(a);
            domain.Sign = sign;
        }

        static void TrySetSign(Set face, IScalarFunction alpha){
            Interpolation.Bezier bz = Interpolation.Interpolator.Quadratic(alpha, face.Geometry);
            Tensor1 P = bz.P;
            face.Sign = decider.Sign(P);
        }

        static void TrySetSign(Set face) {
            if (TryGetSign(face, out Symbol sign)) {
                face.Sign = sign;
            }
        }

        static bool TryGetSign(Set face, out Symbol sign) {
            (double eval, Tensor1 grad, Tensor2 hessian) = face.Alpha.EvaluateAndGradientAndHessian(Tensor1.Zeros(face.Alpha.M));
            Tensor1 d = face.Geometry.Diameters / 2;
            double delta = MathUtility.Abs(grad) * d + (0.5 + C) * d * MathUtility.Abs(hessian) * d;
            double min = eval - delta;
            double max = eval + delta;

            Symbol minSign = decider.Sign(min); 
            Symbol maxSign = decider.Sign(max);

            if (minSign == Symbol.Plus && maxSign == Symbol.Plus) {
                sign = Symbol.Plus;
                return true;
            } else if (minSign == Symbol.Minus &&  maxSign == Symbol.Minus) {
                sign = Symbol.Minus;
                return true;
            } else if (minSign == Symbol.Zero && maxSign == Symbol.Zero) {
                sign = Symbol.Zero;
                return true;
            } else {
                sign = Symbol.None;
                return false;
            }
        }

        class GradientComponent : IScalarFunction{
            
            int h;

            IScalarFunction alpha; 

            public GradientComponent(IScalarFunction alpha, int h) {
                this.alpha = alpha;
                this.h = h;
            }

            public int M => alpha.M;

            public double Evaluate(Tensor1 x) {
                (double evaluation, Tensor1 gradient) = alpha.EvaluateAndGradient(x);
                return gradient[h];
            }

            public (double evaluation, Tensor1 gradient) EvaluateAndGradient(Tensor1 x) {
                throw new NotImplementedException();
            }

            public (double evaluation, Tensor1 gradient, Tensor2 hessian) EvaluateAndGradientAndHessian(Tensor1 x) {
                throw new NotImplementedException();
            }
        }

        static bool IsMonotoneIn(IScalarFunction alpha, Axis heightDirection, Set face) {

            GradientComponent grad_h = new GradientComponent(alpha, (int)heightDirection);
            Interpolation.Bezier bz = Interpolation.Interpolator.Quadratic(grad_h, face.Geometry);
            Tensor1 P = bz.P;
            (Symbol maxSign, Symbol minSign)= decider.MaxMinSign(P);

            if ((minSign == Symbol.Plus || minSign == Symbol.Zero) && maxSign == Symbol.Plus) {
                return true;
            } else if (minSign == Symbol.Minus && (maxSign == Symbol.Minus || maxSign == Symbol.Zero)) {
                return true;
            } else if (minSign == Symbol.Zero && maxSign == Symbol.Zero) {
                return true;
            } else {
                return false;
            }
        }

        static bool IsMonotoneIn(Axis heightDirection, Set face) {
            //return true;
            (double a, Tensor1 grad, Tensor2 hessian) = face.Alpha.EvaluateAndGradientAndHessian(Tensor1.Zeros(face.Alpha.M));
            Tensor1 d = face.Geometry.Diameters / 2;
            Tensor1 n = Tensor1.Zeros(face.Alpha.M);
            n[(int)heightDirection] = 1;
            double delta = (1 + 2 * C) * (MathUtility.Abs(hessian) * d * n);
            double f = Math.Abs( grad[(int)heightDirection]);
            
            Symbol minSign = decider.Sign(f - delta);

            if(minSign == Symbol.Plus || minSign == Symbol.Zero) {
                return true;
            } else {
                return false;
            }
        }
    }

    internal static class Scanner1 {

        static Decider decider = new Decider(Environment.Epsilon);

        public static bool TryDecompose(IScalarFunction alpha, HyperRectangle geometry, out NestedSet body) {
            body = new NestedSet(alpha, geometry);
            bool graphable = FindFaces(body.LowestLeafs(), alpha);
            return graphable;
        }

        static bool FindFaces(SetList faces, IScalarFunction alpha) {
            if (faces.Dimension > 0) {
                foreach (BinaryNode<Set> face in faces) {
                    TrySetSign(face.Value, alpha);
                }
                RemoveFacesWithSignFrom(faces);

                if (faces.Count > 0) {
                    Axis heightDirection = FindHeightDirection(alpha, faces);
                    foreach (BinaryNode<Set> face in faces) {
                        face.Value.HeightDirection = heightDirection;
                    }
                    foreach (BinaryNode<Set> face in faces) {
                        if (!IsMonotoneIn(alpha, heightDirection, face.Value)) {
                            face.Value.Graphable = false;
                            return false;
                        }
                    }
                    SetList subspace = AddFaceLayerTo(faces, heightDirection);
                    return FindFaces(subspace, alpha);
                } else {
                    return true;
                }
            } else {
                foreach (BinaryNode<Set> face in faces) {
                    SetSign(alpha, face.Value);
                }
                return true;
            }
        }

        static void RemoveFacesWithSignFrom(SetList space) {
            LinkedListNode<BinaryNode<Set>> node = space.First;
            while (node != null) {
                LinkedListNode<BinaryNode<Set>> nextNode = node.Next;
                if (node.Value.Value.Sign != Symbol.None) {
                    space.Remove(node);
                }
                node = nextNode;
            }
        }

        static SetList AddFaceLayerTo(SetList space, Axis heightDirection) {
            SetList subspace = new SetList(space.Dimension - 1);
            foreach (BinaryNode<Set> face in space) {
                Set topDomain = face.Value.Face(heightDirection, Symbol.Plus);
                BinaryNode<Set> top = new BinaryNode<Set>(topDomain);
                top.Parent = face;
                face.SecondChild = top;
                subspace.AddLast(top);

                Set bottomDomain = face.Value.Face(heightDirection, Symbol.Minus);
                BinaryNode<Set> bottom = new BinaryNode<Set>(bottomDomain);
                face.FirstChild = bottom;
                bottom.Parent = face;
                subspace.AddLast(bottom);
            }
            return subspace;
        }

        static Axis FindHeightDirection(IScalarFunction alpha, SetList space) {
            int heightDirection = -1;
            double maxAbsSquared = double.MinValue;
            foreach (BinaryNode<Set> face in space) {
                (double e, Tensor1 g) = alpha.EvaluateAndGradient(Tensor1.Zeros(alpha.M));
                Tensor1 gradient = ProjectGradientTo(face.Value.Geometry, g);
                double absSquared = gradient * gradient;
                if (absSquared > maxAbsSquared) {
                    maxAbsSquared = absSquared;
                    heightDirection = IndexOfMaxEntryOn(face.Value.Geometry, gradient);
                }
            }
            return (Axis)heightDirection;
        }

        static Tensor1 ProjectGradientTo(HyperRectangle face, Tensor1 gradient) {
            for (int i = 0; i < gradient.M; ++i) {
                if (!face.ActiveDimensions[i]) {
                    gradient[i] = 0;
                }
            }
            return gradient;
        }

        static int IndexOfMaxEntryOn(HyperRectangle face, Tensor1 x) {
            double max = double.MinValue;
            int index = -1;
            for (int i = 0; i < x.M; ++i) {
                if (face.ActiveDimensions[i]) {
                    double abs = Math.Abs(x[i]);
                    if (max < abs) {
                        max = abs;
                        index = i;
                    }
                }
            }
            return index;
        }

        static void SetSign(IScalarFunction alpha, Set domain) {
            double a = alpha.Evaluate(domain.Geometry.Center);
            Symbol sign = decider.Sign(a);
            domain.Sign = sign;
        }

        static void TrySetSign(Set face, IScalarFunction alpha) {
            Interpolation.Bezier bz = Interpolation.Interpolator.Quadratic(alpha, face.Geometry);
            Tensor1 P = bz.P;
            face.Sign = decider.Sign(P);
        }

        static bool IsMonotoneIn(IScalarFunction alpha, Axis heightDirection, Set face) {

            GradientComponent grad_h = new GradientComponent(alpha, (int)heightDirection);
            Interpolation.Bezier bz = Interpolation.Interpolator.Quadratic(grad_h, face.Geometry);
            Tensor1 P = bz.P;
            (Symbol maxSign, Symbol minSign) = decider.MaxMinSign(P);

            if ((minSign == Symbol.Plus || minSign == Symbol.Zero) && maxSign == Symbol.Plus) {
                return true;
            } else if (minSign == Symbol.Minus && (maxSign == Symbol.Minus || maxSign == Symbol.Zero)) {
                return true;
            } else if (minSign == Symbol.Zero && maxSign == Symbol.Zero) {
                return true;
            } else {
                return false;
            }
        }

    }
}

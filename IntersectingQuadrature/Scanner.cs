using IntersectingQuadrature.Interpolation;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;
using System.Collections;
using NUnit.Framework.Internal.Commands;

namespace IntersectingQuadrature {

    internal static class Scanner {

        static Decider decider = new Decider(Environment.Epsilon);

        public static bool TryDecompose(IScalarFunction alpha, HyperRectangle geometry, out NestedSet body) {
            body = new NestedSet(alpha, geometry);
            bool graphable = FindFaces(body.LowestLeafs(), alpha);
            return graphable;
        }

        public static bool TryDecompose(NestedSet body) {
            bool graphable = FindFaces(body.LowestLeafs(), body.Alpha);
            return graphable;
        }

        static Axis FindHeightDirection(SetList space, IScalarFunction alpha) {
            Tensor1 averageGradient = Tensor1.Zeros(alpha.M);
            foreach (BinaryNode<Set> face in space) {
                (double e, Tensor1 g) = alpha.EvaluateAndGradient(face.Value.Geometry.Center);
                averageGradient += g / space.Count;
            }
            int heightDirection = IndexOfMaxEntryOn(space.First.Value.Value.Geometry.ActiveDimensions, averageGradient);
            return (Axis)heightDirection;
        }

        static int IndexOfMaxEntryOn(BitArray activeDimensions, Tensor1 x) {
            double max = double.MinValue;
            int index = -1;
            for (int i = 0; i < x.M; ++i) {
                if (activeDimensions[i]) {
                    double abs = Math.Abs(x[i]);
                    if (max < abs) {
                        max = abs;
                        index = i;
                    }
                }
            }
            return index;
        }

        static bool FindFaces(SetList faces, IScalarFunction alpha) {
            if (faces.Dimension > 0) {
                foreach (BinaryNode<Set> faceNode in faces) {
                    Set face = faceNode.Value;
                    face.Alpha = Interpolation.Interpolator.Cubic(alpha, face.Geometry);
                    TrySetSign(face);
                }
                RemoveFacesWithSignFrom(faces);

                if (faces.Count > 0) {
                    Axis heightDirection = FindHeightDirection(faces, alpha);
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

        static Axis FindHeightDirection( SetList faces) {
            int dim = faces.First.Value.Value.Geometry.SpaceDimension;
            BitArray activeDimension = faces.First.Value.Value.Geometry.ActiveDimensions;
            Axis heightDirection = Axis.None;
            //Choose max
            double max = double.MinValue;

            int bezierDim = 0;
            for(int h = 0; h < dim; ++h) {
                if (activeDimension[h]) {
                    double m = 0;
                    foreach (BinaryNode<Set> faceNode in faces) {
                        Set face = faceNode.Value;
                        Tensor1 Pi = ProjectMax(face.Alpha, bezierDim);
                        m += Tensor.Algebra.Sum(Pi) / faces.Count;
                    }
                    if(m > max) {
                        heightDirection = (Axis)h;
                        max = m;
                    }
                    ++bezierDim;
                }
            }
            return heightDirection;
        }

        static Tensor1 ProjectMax(Bezier c, int direction) {
            Tensor1 a = Tensor1.Zeros(MathUtility.Pow(c.M, c.Dimension - 1));
            for(int i = 0; i < a.M; ++i) {
                a[i] = double.MinValue;
            }

            for (int i = 0; i < c.P.M; ++i) {
                int l = 1;
                int ak = 0;
                int al = 1;
                for (int d = 0; d < c.Dimension; ++d) {
                    int k = (i / l) % c.M;
                    l *= c.M;
                    if(d != direction) {
                        ak += k * al;
                        al *= c.M;
                    }
                }
                a[ak] = Math.Max(a[ak], c.P[i]);
            }
            return a;
        }

        static void SetSign(IScalarFunction alpha, Set domain) {
            double a = alpha.Evaluate(domain.Geometry.Center);
            Symbol sign = decider.Sign(a);
            domain.Sign = sign;
        }

        static void TrySetSign(Set face) {
            Tensor1 P = face.Alpha.P;
            face.Sign = decider.Sign(P);
        }

        static bool IsMonotoneIn(IScalarFunction alpha, Axis heightDirection, Set face) {

            GradientComponent grad_h = new GradientComponent(alpha, (int)heightDirection);
            Interpolation.Bezier bz = Interpolation.Interpolator.Cubic(grad_h, face.Geometry);
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

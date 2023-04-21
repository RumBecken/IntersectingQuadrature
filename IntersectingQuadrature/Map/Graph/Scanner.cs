using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Graph
{

    internal static class Scanner
    {

        static Decider decider = new Decider(Environment.Epsilon);

        public static bool TryDecompose(IScalarFunction alpha, HyperRectangle geometry, out NestedSet body)
        {
            body = new NestedSet(alpha, geometry);
            bool graphable = FindFaces(body.LowestLeafs(), alpha);
            return graphable;
        }

        public static bool TryDecompose(NestedSet body)
        {
            bool graphable = FindFaces(body.LowestLeafs(), body.Alpha);
            return graphable;
        }

        static Axis FindHeightDirection(SetList space, IScalarFunction alpha)
        {
            Tensor1 averageGradient = Tensor1.Zeros(alpha.M);
            foreach (BinaryNode<Set> face in space)
            {
                (double e, Tensor1 g) = alpha.EvaluateAndGradient(face.Value.Geometry.Center);
                averageGradient += g / space.Count;
            }
            int heightDirection = Utility.IndexOfMaxEntry(space.First.Value.Value.Geometry.ActiveDimensions, averageGradient);
            return (Axis)heightDirection;
        }

        static bool FindFaces(SetList faces, IScalarFunction alpha)
        {
            if (faces.Dimension > 0)
            {
                foreach (BinaryNode<Set> faceNode in faces)
                {
                    Set face = faceNode.Value;
                    Bezier alphaPolynomial = Interpolator.Cubic(alpha, face.Geometry);
                    TrySetSign(face, alphaPolynomial);
                }
                RemoveFacesWithSignFrom(faces);

                if (faces.Count > 0)
                {
                    Axis heightDirection = FindHeightDirection(faces, alpha);
                    foreach (BinaryNode<Set> face in faces)
                    {
                        face.Value.HeightDirection = heightDirection;
                    }
                    foreach (BinaryNode<Set> face in faces)
                    {
                        if (!IsMonotoneIn(alpha, heightDirection, face.Value))
                        {
                            face.Value.Graphable = false;
                            return false;
                        }
                    }
                    SetList subspace = AddFaceLayerTo(faces, heightDirection);
                    return FindFaces(subspace, alpha);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                foreach (BinaryNode<Set> face in faces)
                {
                    SetSign(alpha, face.Value);
                }
                return true;
            }
        }

        static void RemoveFacesWithSignFrom(SetList space)
        {
            LinkedListNode<BinaryNode<Set>> node = space.First;
            while (node != null)
            {
                LinkedListNode<BinaryNode<Set>> nextNode = node.Next;
                if (node.Value.Value.Sign != Symbol.None)
                {
                    space.Remove(node);
                    node.Value.Value.HeightDirection = Axis.None;
                }
                node = nextNode;
            }
        }

        static SetList AddFaceLayerTo(SetList space, Axis heightDirection)
        {
            SetList subspace = new SetList(space.Dimension - 1);
            foreach (BinaryNode<Set> face in space)
            {
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

        static void SetSign(IScalarFunction alpha, Set domain)
        {
            double a = alpha.Evaluate(domain.Geometry.Center);
            Symbol sign = decider.Sign(a);
            domain.Sign = sign;
        }

        static void TrySetSign(Set face, Bezier alpha)
        {
            Tensor1 P = alpha.P;
            face.Sign = decider.Sign(P);
        }

        static bool IsMonotoneIn(IScalarFunction alpha, Axis heightDirection, Set face)
        {
            GradientComponent grad_h = new GradientComponent(alpha, (int)heightDirection);
            Bezier bz = Interpolator.Cubic(grad_h, face.Geometry);
            Tensor1 P = bz.P;
            (Symbol maxSign, Symbol minSign) = decider.MaxMinSign(P);

            if ((minSign == Symbol.Plus || minSign == Symbol.Zero) && maxSign == Symbol.Plus)
            {
                return true;
            }
            else if (minSign == Symbol.Minus && (maxSign == Symbol.Minus || maxSign == Symbol.Zero))
            {
                return true;
            }
            else if (minSign == Symbol.Zero && maxSign == Symbol.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

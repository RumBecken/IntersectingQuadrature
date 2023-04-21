using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Map.Nested;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map
{
    internal class Converter
    {

        IRootFinder rooter;

        public Converter(IRootFinder rooter)
        {
            this.rooter = rooter;
        }

        public IIntegralTransformation ExtractMapping(NestedSet set1)
        {
            Debug.Assert(set1.Alpha.M == set1.Root.Value.Geometry.BodyDimension);
            NestedSet set = set1.Clone();

            Axis[] order = Order(set.Root);
            Tensor2 perturbationMatrix = Tensor2.Zeros(set.Root.Value.Geometry.BodyDimension);
            for (int i = 0; i < order.Length; ++i)
            {
                perturbationMatrix[i, (int)order[i]] = 1;
            }
            Tensor2 dePerturbationMatrix = Algebra.Transpose(perturbationMatrix);

            LinearMapping perturbation = new LinearMapping(perturbationMatrix);
            LinearMapping dePerturbation = new LinearMapping(dePerturbationMatrix);

            IScalarFunction dePerturbatedAlpha = new ScalarComposition(set.Alpha, dePerturbation);

            Perturbate(set, perturbation);

            IIntegralTransformation map = Extract(set, dePerturbatedAlpha, rooter);

            IIntegralTransformation pertubatedMap = new MappingComposition(map, perturbation);
            IIntegralTransformation finalMap = new MappingComposition(dePerturbation, pertubatedMap);
            return finalMap;
        }

        static void Perturbate(NestedSet box, LinearMapping perturbation)
        {
            foreach (BinaryNode<Set> subspace in box.Root.Descendants())
            {
                subspace.Value.Geometry.Center = perturbation.Evaluate(subspace.Value.Geometry.Center);
                subspace.Value.Geometry.Diameters = perturbation.Evaluate(subspace.Value.Geometry.Diameters);
                subspace.Value.BoundingBox.Center = perturbation.Evaluate(subspace.Value.BoundingBox.Center);
                subspace.Value.BoundingBox.Diameters = perturbation.Evaluate(subspace.Value.BoundingBox.Diameters);
            }
        }

        static IIntegralTransformation Extract(NestedSet box, IScalarFunction alpha, IRootFinder rooter)
        {
            Heights mMinus = CubeSide(box.Root.Value, Symbol.Minus);
            SetZero(mMinus, box, alpha, rooter, Symbol.Minus);
            Heights mPlus = CubeSide(box.Root.Value, Symbol.Plus);
            SetZero(mPlus, box, alpha, rooter, Symbol.Plus);

            int d = box.Root.Value.Geometry.BodyDimension;
            IIntegralTransformation map = NestedMapping.Dimension(d, mPlus, mMinus);
            return map;
        }

        static Axis[] Order(BinaryNode<Set> root)
        {
            int dim = root.Value.Geometry.BodyDimension;
            int[] position = new int[dim];
            Axis[] order = new Axis[dim];
            for (int i = 0; i < dim; ++i)
            {
                position[i] = i;
                order[i] = (Axis)i;
            }
            int height = root.Height();
            int j = dim - 1;
            for (int i = 0; i < height; ++i)
            {
                LinkedList<BinaryNode<Set>> level = root.Descendants(i);
                Axis insertion = Axis.None;
                foreach (BinaryNode<Set> node in level)
                {
                    if (node.Value.HeightDirection != Axis.None)
                    {
                        insertion = node.Value.HeightDirection;
                        break;
                    }
                }
                int a = (int)order[j];
                int b = (int)insertion;

                int temp = position[a];
                position[a] = position[b];
                position[b] = temp;
                order[position[a]] = (Axis)a;
                order[position[b]] = (Axis)b;

                --j;
            }
            return order;
        }

        static Heights CubeSide(Set box, Symbol side)
        {
            int d = box.Geometry.BodyDimension;
            Heights cubeSide = new Heights();
            for (int i = 0; i < d; ++i)
            {
                double m = (int)side * box.Geometry.Diameters[i] / 2.0 + box.Geometry.Center[i];
                switch (i)
                {
                    case 0:
                        cubeSide.SetX(new Point(m));
                        break;
                    case 1:
                        cubeSide.SetY(new Line(m));
                        break;
                    case 2:
                        cubeSide.SetZ(new Plane(m));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return cubeSide;
        }

        static void SetZero(Heights heights, NestedSet box, IScalarFunction alpha, IRootFinder rooter, Symbol side)
        {
            int height = box.Root.Height();
            for (int h = 0; h < height; ++h)
            {
                LinkedList<BinaryNode<Set>> level = box.Root.Descendants(h);
                foreach (BinaryNode<Set> sibling in level)
                {
                    BinaryNode<Set> topChild = sibling.SecondChild;
                    BinaryNode<Set> bottomChild = sibling.FirstChild;
                    if (topChild != null && bottomChild != null)
                    {
                        Symbol sign = Symbol.None;
                        if (side == Symbol.Minus)
                        {
                            sign = bottomChild.Value.Sign;
                        }
                        else if (side == Symbol.Plus)
                        {
                            sign = topChild.Value.Sign;
                        }
                        else
                        {
                            throw new ArgumentException();
                        }

                        if (sign == Symbol.Zero && bottomChild.Value.Geometry.BodyDimension > 0)
                        {
                            Tensor1 top = topChild.Value.Geometry.Center;
                            top = topChild.Value.BoundingBox.Center;
                            Tensor1 bottom = bottomChild.Value.Geometry.Center;
                            bottom = bottomChild.Value.BoundingBox.Center;

                            switch (bottomChild.Value.Geometry.BodyDimension)
                            {
                                case 1:
                                    ZeroLine y = new ZeroLine(rooter, alpha, top, bottom);
                                    heights.SetY(y);
                                    break;
                                case 2:
                                    ZeroPlane z = new ZeroPlane(rooter, alpha, top, bottom);
                                    heights.SetZ(z);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
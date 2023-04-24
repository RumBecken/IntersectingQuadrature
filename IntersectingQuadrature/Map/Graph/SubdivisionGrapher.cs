using System;
using System.Collections.Generic;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Graph
{
    internal class SubdivisionGrapher : IGrapher
    {

        int maxSubdivisions = 4;

        public SubdivisionGrapher()
        {
        }

        public LinkedList<Decomposition> Decompose(IScalarFunction alpha, IHyperRectangle geometry)
        {
            LinearMapping selfmap = FromUnitCubeTo(geometry);
            IScalarFunction subAlpha = new ScalarComposition(alpha, selfmap);

            NestedSet body = new NestedSet(subAlpha, new UnitHyperCube(geometry.SpaceDimension));
            LinkedList<Decomposition> sets = new LinkedList<Decomposition>();
            Decompose(selfmap, body, sets, 0);
            return sets;
        }

        static LinearMapping FromUnitCubeTo(IHyperRectangle geometry)
        {
            Tensor2 B = Tensor2.Zeros(geometry.SpaceDimension);
            for (int i = 0; i < geometry.SpaceDimension; ++i)
            {
                B[i, i] = geometry.Diameters[i] / 2.0;
            }
            LinearMapping map = new LinearMapping(geometry.Center, B);
            return map;
        }

        void Decompose(IIntegralTransformation subdivisionMap, NestedSet body, LinkedList<Decomposition> sets, int h)
        {
            if (Scanner.TryDecompose(body))
            {
                Decomposition decomposition = new Decomposition
                {
                    Subdivision = subdivisionMap,
                    Graph = body
                };
                sets.AddLast(decomposition);
            }
            else
            {
                if (h < maxSubdivisions)
                {
                    (HyperRectangle A, HyperRectangle B) = Split(body.Root.Value.Geometry, subdivisionMap);

                    LinearMapping mapA = FromUnitCubeTo(A);
                    IScalarFunction alpha = new ScalarComposition(body.Alpha, mapA);
                    NestedSet setA = new NestedSet(alpha, new UnitHyperCube(alpha.M));
                    Decompose(new MappingComposition(subdivisionMap, mapA), setA, sets, h + 1);

                    LinearMapping mapB = FromUnitCubeTo(B);
                    IScalarFunction beta = new ScalarComposition(body.Alpha, mapB);
                    NestedSet setB = new NestedSet(beta, new UnitHyperCube(beta.M));
                    Decompose(new MappingComposition(subdivisionMap, mapB), setB, sets, h + 1);
                }
                else
                {
                    Tensor1 center = body.Root.Value.Geometry.Center;
                    (double a, Tensor1 b) = body.Alpha.EvaluateAndGradient(center);
                    a -= b * center;
                    body.Alpha = new LinearPolynomial(a, b);
                    body.Root.RemoveChildren();
                    body.Root.Value.Sign = Symbol.None;
                    body.Root.Value.HeightDirection = Axis.None;
                    Decompose(subdivisionMap, body, sets, h);
                }
            }
        }

        static (HyperRectangle A, HyperRectangle B) Split(HyperRectangle source, IIntegralTransformation subdivisionMap)
        {
            (Tensor1 f, Tensor2 J) = subdivisionMap.EvaluateAndJacobian(source.Center);
            Tensor1 grad = Tensor1.Zeros(f.M);
            for (int i = 0; i < grad.M; ++i)
            {
                grad[i] = J[i, i];
            }
            int direction = Utility.IndexOfMaxEntry(grad);

            HyperRectangle A = source.Clone();
            A.Diameters[direction] = 1;
            A.Center[direction] = -0.5;
            HyperRectangle B = source.Clone();
            B.Diameters[direction] = 1;
            B.Center[direction] = 0.5;
            return (A, B);
        }
    }
}

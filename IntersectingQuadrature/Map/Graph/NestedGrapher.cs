using System;
using System.Collections.Generic;
using IntersectingQuadrature.Tensor;


namespace IntersectingQuadrature.Map.Graph {

    internal class NestedGrapher : IGrapher {

        Finder hunter;

        int maxSubdivisions = 4;

        public NestedGrapher(Finder hunter) {
            this.hunter = hunter;
        }

        public LinkedList<Decomposition> Decompose(IScalarFunction alpha, IHyperRectangle geometry)
        {
            LinearMapping selfmap = FromUnitCubeTo(geometry);
            IScalarFunction subAlpha = new ScalarComposition(alpha, selfmap);

            NestedSet body = new NestedSet(subAlpha, EmbeddedHyperRectangle.UnitCube(geometry.Dimension));
            LinkedList<Decomposition> sets = new LinkedList<Decomposition>();
            Decompose(selfmap, body, sets, 0);
            return sets;
        }

        static LinearMapping FromUnitCubeTo(IHyperRectangle geometry)
        {
            Tensor2 B = Tensor2.Zeros(geometry.Dimension);
            for (int i = 0; i < geometry.Dimension; ++i)
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
                if(h == 0) {
                    List<IntegralMapping> maps = InsertGradientRoot(body.Alpha, EmbeddedHyperRectangle.UnitCube(body.Alpha.M), body, out Axis heightDirection);

                    foreach (IntegralMapping T in maps) {
                        IScalarFunction alphaT = new ScalarComposition(body.Alpha, T.Transformation);
                        IIntegralTransformation mm = new MappingComposition(subdivisionMap, T.Transformation);
                        NestedSet subBody = new NestedSet(alphaT, EmbeddedHyperRectangle.UnitCube(body.Alpha.M));
                        Decompose(mm, subBody, sets, h + 1);
                    }
                } else {
                    if (h < maxSubdivisions) {
                        (EmbeddedHyperRectangle A, EmbeddedHyperRectangle B) = Split(body.Root.Value.Geometry, subdivisionMap);

                        LinearMapping mapA = FromUnitCubeTo(A);
                        IScalarFunction alpha = new ScalarComposition(body.Alpha, mapA);
                        NestedSet setA = new NestedSet(alpha, EmbeddedHyperRectangle.UnitCube(alpha.M));
                        Decompose(new MappingComposition(subdivisionMap, mapA), setA, sets, h + 1);

                        LinearMapping mapB = FromUnitCubeTo(B);
                        IScalarFunction beta = new ScalarComposition(body.Alpha, mapB);
                        NestedSet setB = new NestedSet(beta, EmbeddedHyperRectangle.UnitCube(beta.M));
                        Decompose(new MappingComposition(subdivisionMap, mapB), setB, sets, h + 1);
                    } else {
                        Tensor1 center = body.Root.Value.Geometry.Center;
                        (double a, Tensor1 b) = body.Alpha.EvaluateAndGradient(center);
                        a -= b * center;
                        body.Alpha = new LinearPolynomial(a, b);
                        body.Root.RemoveChildren();
                        body.Root.Value.Sign = Symbol.None;
                        body.Root.Value.HeightDirection = Axis.None;
                        Decompose(subdivisionMap, body, sets, h+1);
                    }
                }
            }
        }

        static (EmbeddedHyperRectangle A, EmbeddedHyperRectangle B) Split(EmbeddedHyperRectangle source, IIntegralTransformation subdivisionMap)
        {
            (Tensor1 f, Tensor2 J) = subdivisionMap.EvaluateAndJacobian(source.Center);
            Tensor1 grad = Tensor1.Zeros(f.M);
            for (int i = 0; i < grad.M; ++i)
            {
                grad[i] = J[i, i];
            }
            int direction = Utility.IndexOfMaxEntry(grad);

            EmbeddedHyperRectangle A = source.Clone();
            A.Diameters[direction] = 1;
            A.Center[direction] = -0.5;
            EmbeddedHyperRectangle B = source.Clone();
            B.Diameters[direction] = 1;
            B.Center[direction] = 0.5;
            return (A, B);
        }

        List<IntegralMapping> InsertGradientRoot(IScalarFunction alpha, EmbeddedHyperRectangle geometry, NestedSet body, out Axis heightDirection)
        {
            IScalarFunction dAlpha = GradientComponentLevelSet(alpha, body, out heightDirection);
            List<IntegralMapping> maps = hunter.FindMappings(dAlpha, geometry);
            return maps;
        }

        IScalarFunction GradientComponentLevelSet(IScalarFunction alpha, NestedSet body, out Axis heightDirection)
        {
            SetList lowestLeafs = body.LowestLeafs();
            LinkedListNode<BinaryNode<Set>> leaf = lowestLeafs.First;
            while (leaf.Value.Value.Graphable == true)
            {
                leaf = leaf.Next;
            }
            Set a = leaf.Value.Value;
            Restriction restriction = new Restriction(a.Geometry);
            heightDirection = a.HeightDirection;
            GradientComponent dAlpha = new GradientComponent(alpha, (int)heightDirection);
            ScalarComposition dAlphaK = new ScalarComposition(dAlpha, restriction);
            return dAlphaK;
        }
    }
}

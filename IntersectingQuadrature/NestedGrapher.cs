using IntersectingQuadrature.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;
using System.Xml.Schema;
using IntersectingQuadrature.Mapper;
using System.Diagnostics;

namespace IntersectingQuadrature {
    
    struct Decomposition {
        public IIntegralMapping Subdivision;
        public NestedSet Graph;
    }

    internal class NestedGrapher {

        MapFinder hunter;

        public static int Subdivisions = 0; 

        public NestedGrapher(MapFinder hunter) {
            this.hunter = hunter;
        }

        public LinkedList<Decomposition> Decompose( IScalarFunction alpha, HyperRectangle geometry) {
            
            Tensor2 B = Tensor2.Zeros(alpha.M);
            for(int i = 0; i < alpha.M; ++i) {
                B[i, i] = geometry.Diameters[i] / 2.0;
            }
            LinearMapping selfmap = new LinearMapping(geometry.Center, B);
            IScalarFunction subAlpha = new ScalarComposition(alpha, selfmap);

            NestedSet body = new NestedSet(subAlpha, new UnitCube(geometry.SpaceDimension));
            LinkedList<Decomposition> sets = new LinkedList<Decomposition>();
            Decompose(selfmap, body, sets);
            return sets;
        }

        void Decompose(IIntegralMapping subdivisionMap, NestedSet body, LinkedList<Decomposition> sets) {
            int h = body.Height();
            if(Scanner.TryDecompose(body)) {
                Decomposition decomposition = new Decomposition {
                    Subdivision = subdivisionMap,
                    Graph = body
                };
                sets.AddLast(decomposition);
            } else {
                if (body.Height() > h) {
                    Subdivisions += 1;
                    List<Map> maps = Subdivide(body.Alpha, new UnitCube(body.Alpha.M), body, out Axis heightDirection);

                    foreach (Map T in maps) {
                        IScalarFunction alphaT = new ScalarComposition(body.Alpha, T.Mapping);
                        IIntegralMapping mm = new MappingComposition(subdivisionMap, T.Mapping);
                        NestedSet subBody = body.Clone();
                        AddFaceLayerTo(subBody.LowestLeafs(), heightDirection);
                        subBody.Alpha = alphaT;
                        Decompose(mm, subBody, sets);
                    }
                } else {
                    Console.WriteLine("Hhahsafsdjlahfkghadfaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaavhk");
                }
                
            }
        }

        static void AddFaceLayerTo(SetList space, Axis heightDirection) {
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
        }

        List<Map> Subdivide(IScalarFunction alpha, HyperRectangle geometry, NestedSet body, out Axis heightDirection) {
            IScalarFunction dAlpha = GradientComponentLevelSet(alpha, body, out heightDirection);
            List<Map> maps = hunter.FindMappings(dAlpha, geometry);
            return maps;
        }

        IScalarFunction GradientComponentLevelSet(IScalarFunction alpha, NestedSet body, out Axis heightDirection) {
            SetList lowestLeafs = body.LowestLeafs();
            LinkedListNode<BinaryNode<Set>> leaf = lowestLeafs.First;
            while(leaf.Value.Value.Graphable == true) {
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

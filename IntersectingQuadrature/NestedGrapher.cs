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
            
            LinkedList<Decomposition> sets = new LinkedList<Decomposition>();
            Decompose(selfmap, subAlpha, sets);
            return sets;
        }

        void Decompose(IIntegralMapping subdivisionMap, IScalarFunction alpha, LinkedList<Decomposition> sets) {
            if(Scanner.TryDecompose(alpha, new UnitCube(alpha.M), out NestedSet body)) {
                Decomposition decomposition = new Decomposition {
                    Subdivision = subdivisionMap,
                    Graph = body
                };
                sets.AddLast(decomposition);
            } else {
                Subdivisions += 1;
                List<Map> maps = Subdivide(alpha, new UnitCube(alpha.M), body);
                foreach (Map T in maps) {
                    IScalarFunction alphaT = new ScalarComposition(alpha, T.Mapping);
                    IIntegralMapping mm = new MappingComposition(subdivisionMap, T.Mapping);
                    Decompose(mm, alphaT, sets);
                }
            }
        }

        List<Map> Subdivide(IScalarFunction alpha, HyperRectangle geometry, NestedSet body) {
            IScalarFunction dAlpha = GradientComponentLevelSet(alpha, body);
            List<Map> maps = hunter.FindMappings(dAlpha, geometry);
            return maps;
        }

        IScalarFunction GradientComponentLevelSet(IScalarFunction alpha, NestedSet body) {
            SetList lowestLeafs = body.LowestLeafs();
            LinkedListNode<BinaryNode<Set>> leaf = lowestLeafs.First;
            while(leaf.Value.Value.Graphable == true) {
                leaf = leaf.Next;
            }
            Set a = leaf.Value.Value;
            Restriction restriction = new Restriction(a.Geometry);
            Axis heightDirection = a.HeightDirection;
            GradientComponent dAlpha = new GradientComponent(alpha, (int)heightDirection);
            ScalarComposition dAlphaK = new ScalarComposition(dAlpha, restriction);
            return dAlphaK;
        }
    }
}

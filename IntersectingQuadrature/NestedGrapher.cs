using IntersectingQuadrature.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;
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

        public NestedGrapher(MapFinder hunter) {
            this.hunter = hunter;
        }

        public LinkedList<Decomposition> Decompose( IScalarFunction alpha, HyperRectangle geometry) {
            LinkedList<Decomposition> sets = new LinkedList<Decomposition>();
            Tensor2 B = Tensor2.Unit(alpha.M);
            LinearMapping selfmap = new LinearMapping(B);
            Decompose(selfmap, alpha, geometry, sets);
            return sets;
        }

        void Decompose(IIntegralMapping subdivision, IScalarFunction alpha, HyperRectangle geometry, LinkedList<Decomposition> sets) {
            if(Scanner1.TryDecompose(alpha, geometry, out NestedSet body)) {
                Decomposition decomposition = new Decomposition {
                    Subdivision = subdivision,
                    Graph = body
                };
                sets.AddLast(decomposition);
            } else {
                List<Map> maps = Subdivide(alpha, geometry, body);
                foreach (Map T in maps) {
                    IScalarFunction alphaT = new ScalarComposition(alpha, T.Mapping);
                    IIntegralMapping mm = new MappingComposition(subdivision, T.Mapping);
                    Decompose(mm, alphaT, T.Domain, sets);
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

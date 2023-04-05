using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.TensorAnalysis;

namespace IntersectingQuadrature {

    internal class MapFinder {

        IRootFinder newton;

        int maxSubdivisions;

        Organizer organizer;

        Mapper.Mapper mapper;

        public MapFinder() {
            newton = new NewtonMethod(Environment.Epsilon);
            maxSubdivisions = 5;
            organizer = new Organizer(newton);
            mapper = new Mapper.Mapper(newton);
        }

        public List<Map> FindMappings(IScalarFunction alpha, Symbol sign, HyperRectangle domain) {
            Debug.Assert(alpha.M == domain.Dimension);
            List<Map> mappings = new List<Map>();
            LinkedList<NestedSet> spaces = Scanner.FindSets(alpha, domain, maxSubdivisions);
            foreach (NestedSet space in spaces) {
                LinkedList<NestedSet> boxes = organizer.Sort(space);
                foreach (NestedSet box in boxes) {
                    if(TryFindMapping(box, sign, out Map map)) {
                        mappings.Add(map);
                    }
                }
            }
            return mappings;
        }

        bool TryFindMapping( NestedSet box, Symbol sign, out Map map) {
            if (sign == box.Root.Value.Sign) {
                IIntegralMapping mapping = mapper.ExtractMapping(box);
                map = new Map {
                    Mapping = mapping,
                    Domain = new UnitCube(box.Root.Value.Geometry.Dimension)
                };
                return true;
            }else if (sign == Symbol.Zero) {
                if (box.Root.Value.Sign == Symbol.Plus && TryGetZeroFace(box.Root, out HyperRectangle surface)) {
                    IIntegralMapping mapping = mapper.ExtractMapping(box);
                    IIntegralMapping emapping = new MappingComposition(mapping, Plane(surface));
                    map = new Map { 
                        Mapping = emapping, 
                        Domain = new UnitCube(surface.Dimension)
                    };
                    return true;
                }
            } 
            map = null;
            return false;
        }

        static bool TryGetZeroFace(BinaryNode<Set> domain, out HyperRectangle surface) {
            Debug.Assert(domain.Value.Sign != Symbol.Zero);
            
            if (domain.Value.HeightDirection != Axis.None) {
                Axis direction = domain.Value.HeightDirection;
                Symbol bottomSign = domain.FirstChild.Value.Sign;
                Symbol topSign = domain.SecondChild.Value.Sign;
                Symbol sign = Symbol.None;
                if (bottomSign == Symbol.Zero) {
                    sign = Symbol.Minus;
                } else if (topSign == Symbol.Zero) {
                    sign = Symbol.Plus;
                }
                if (sign != Symbol.None) {
                    HyperRectangle unit = new UnitCube(domain.Value.Geometry.Dimension);
                    surface = unit.Face((int)direction, sign);
                    return true;
                }
            }
            surface = null;
            return false;
        }

        public static Embedding Plane(HyperRectangle face) {
            Tensor2 b = Tensor2.Zeros(face.Codimension, face.Dimension);
            int j = 0;
            for(int i = 0; i < face.Codimension; ++i) {
                if (face.ActiveDimensions[i]) {
                    b[i, j] = face.Diameters[i] / 2;
                    ++j;
                }
            }
            Tensor1 a = face.Center;
            LinearVectorFunction map = new LinearVectorFunction(a, b);
            return new Embedding(map);
        }
    }
}

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

        NestedGrapher grapher;

        public MapFinder() {
            newton = new NewtonMethod(Environment.Epsilon);
            maxSubdivisions = 6;
            organizer = new Organizer(newton);
            mapper = new Mapper.Mapper(newton);
            grapher = new NestedGrapher(this);
        }

        public List<Map> FindMappings(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, HyperRectangle domain) {
            List<Map> mappings = new List<Map>();
            List<Map> setA = FindMappings(alpha, signAlpha, domain);
            foreach (Map A in setA) {
                IScalarFunction betaA = new ScalarComposition(beta, A.Mapping);
                List<Map> setB = FindMappings(betaA, signBeta, A.Domain);
                foreach (Map B in setB) {
                    IIntegralMapping AB = new MappingComposition(A.Mapping, B.Mapping);
                    mappings.Add(new Map() {
                        Domain = B.Domain,
                        Mapping = AB
                    });
                }
            }
            return mappings;
        }

        public List<Map> FindMappings(IScalarFunction alpha, Symbol sign, HyperRectangle domain) {
            Debug.Assert(alpha.M == domain.BodyDimension);
            List<Map> mappings = new List<Map>();
            LinkedList<Decomposition> spaces = grapher.Decompose(alpha, domain);
            foreach (Decomposition space in spaces) {
                LinkedList<NestedSet> boxes = organizer.Sort(space.Graph);
                foreach (NestedSet box in boxes) {
                    if(TryFindMapping(box, sign, out Map map)) {
                        MappingComposition m = new MappingComposition(space.Subdivision, map.Mapping);
                        map.Mapping = m;
                        mappings.Add(map);
                    }
                }
            }
            return mappings;
        }

        public List<Map> FindMappings(IScalarFunction alpha, HyperRectangle domain) {
            Debug.Assert(alpha.M == domain.BodyDimension);
            List<Map> mappings = new List<Map>();
            LinkedList<Decomposition> spaces = grapher.Decompose(alpha, domain);
            foreach (Decomposition space in spaces) {
                LinkedList<NestedSet> boxes = organizer.Sort(space.Graph);
                foreach (NestedSet box in boxes) {
                    IIntegralMapping mapping = mapper.ExtractMapping(box);
                    Map map = new Map {
                        Mapping = new MappingComposition(space.Subdivision, mapping),
                        Domain = new UnitCube(box.Root.Value.Geometry.BodyDimension)
                    };
                    mappings.Add(map);
                }
            }
            return mappings;
        }

        bool TryFindMapping( NestedSet box, Symbol sign, out Map map) {
            if (sign == box.Root.Value.Sign) {
                IIntegralMapping mapping = mapper.ExtractMapping(box);
                map = new Map {
                    Mapping = mapping,
                    Domain = new UnitCube(box.Root.Value.Geometry.BodyDimension)
                };
                return true;
            }else if (sign == Symbol.Zero) {
                if (box.Root.Value.Sign == Symbol.Plus && TryGetZeroFace(box.Root, out HyperRectangle surface)) {
                    IIntegralMapping mapping = mapper.ExtractMapping(box);
                    IIntegralMapping emapping = new MappingComposition(mapping, Plane(surface));
                    map = new Map { 
                        Mapping = emapping, 
                        Domain = new UnitCube(surface.BodyDimension)
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
                    HyperRectangle unit = new UnitCube(domain.Value.Geometry.BodyDimension);
                    surface = unit.Face((int)direction, sign);
                    return true;
                }
            }
            surface = null;
            return false;
        }

        public static Embedding Plane(HyperRectangle face) {
            Tensor2 b = Tensor2.Zeros(face.SpaceDimension, face.BodyDimension);
            int j = 0;
            for(int i = 0; i < face.SpaceDimension; ++i) {
                if (face.ActiveDimensions[i]) {
                    b[i, j] = face.Diameters[i] / 2;
                    ++j;
                }
            }
            Tensor1 a = face.Center;
            LinearVectorPolynomial map = new LinearVectorPolynomial(a, b);
            return new Embedding(map);
        }
    }
}

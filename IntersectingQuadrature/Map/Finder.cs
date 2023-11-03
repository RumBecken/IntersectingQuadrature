using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Map.Graph;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map
{
    internal class Finder
    {
        IRootFinder newton;

        Tesselater organizer;

        Converter mapper;

        IGrapher grapher;

        public Finder()
        {
            IGrapher grapher = new SubdivisionGrapher();
            //grapher = new NestedGrapher(new Finder(new SubdivisionGrapher()));
            Initialize(grapher);
        }

        Finder(IGrapher grapher) {
            Initialize(grapher);
        }

        void Initialize(IGrapher grapher) {
            this.grapher = grapher;
            newton = new NewtonMethod(Environment.Epsilon);
            organizer = new Tesselater(newton);
            mapper = new Converter(newton);
        }

        public List<IntegralMapping> FindMappings(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, IHyperRectangle domain)
        {
            List<IntegralMapping> mappings = new List<IntegralMapping>();
            List<IntegralMapping> setA = FindMappings(alpha, signAlpha, domain);
            foreach (IntegralMapping A in setA)
            {
                IScalarFunction betaA = new ScalarComposition(beta, A.Transformation);
                List<IntegralMapping> setB = FindMappings(betaA, signBeta, A.Domain);
                foreach (IntegralMapping B in setB)
                {
                    IIntegralTransformation AB = new MappingComposition(A.Transformation, B.Transformation);
                    mappings.Add(new IntegralMapping()
                    {
                        Domain = B.Domain,
                        Transformation = AB
                    });
                }
            }
            return mappings;
        }

        public List<IntegralMapping> FindMappings(IScalarFunction alpha, Symbol sign, IHyperRectangle domain)
        {
            if(alpha.M != domain.Dimension) {
                throw new NotSupportedException("Level Set and Body dimension do not match");
            }
            List<IntegralMapping> mappings = new List<IntegralMapping>();
            LinkedList<Decomposition> spaces = grapher.Decompose(alpha, domain);
            foreach (Decomposition space in spaces)
            {
                LinkedList<NestedSet> boxes = organizer.Sort(space.Graph);
                foreach (NestedSet box in boxes)
                {
                    if (TryFindMapping(box, sign, out IntegralMapping map))
                    {
                        MappingComposition m = new MappingComposition(space.Subdivision, map.Transformation);
                        map.Transformation = m;
                        mappings.Add(map);
                    }
                }
            }
            return mappings;
        }

        public List<IntegralMapping> FindMappings(IScalarFunction alpha, IHyperRectangle domain)
        {
            if (alpha.M != domain.Dimension) {
                throw new NotSupportedException("Level Set and Body dimension do not match");
            }
            List<IntegralMapping> mappings = new List<IntegralMapping>();
            LinkedList<Decomposition> spaces = grapher.Decompose(alpha, domain);
            foreach (Decomposition space in spaces)
            {
                LinkedList<NestedSet> boxes = organizer.Sort(space.Graph);
                foreach (NestedSet box in boxes)
                {
                    IIntegralTransformation mapping = mapper.ExtractMapping(box);
                    IntegralMapping map = new IntegralMapping
                    {
                        Transformation = new MappingComposition(space.Subdivision, mapping),
                        Domain = HyperRectangle.UnitCube(box.Root.Value.Geometry.Dimension)
                    };
                    mappings.Add(map);
                }
            }
            return mappings;
        }

        bool TryFindMapping(NestedSet box, Symbol sign, out IntegralMapping map)
        {
            if (sign == box.Root.Value.Sign)
            {
                IIntegralTransformation mapping = mapper.ExtractMapping(box);
                map = new IntegralMapping
                {
                    Transformation = mapping,
                    Domain = HyperRectangle.UnitCube(box.Root.Value.Geometry.Dimension)
                };
                return true;
            }
            else if (sign == Symbol.Zero)
            {
                if (box.Root.Value.Sign == Symbol.Plus && TryGetZeroFace(box.Root, out EmbeddedHyperRectangle surface))
                {
                    IIntegralTransformation mapping = mapper.ExtractMapping(box);
                    IIntegralTransformation emapping = new MappingComposition(mapping, Plane(surface));
                    map = new IntegralMapping
                    {
                        Transformation = emapping,
                        Domain = HyperRectangle.UnitCube(surface.Dimension)
                    };
                    return true;
                }
            }
            map = null;
            return false;
        }

        static bool TryGetZeroFace(BinaryNode<Set> domain, out EmbeddedHyperRectangle surface)
        {
            Debug.Assert(domain.Value.Sign != Symbol.Zero);

            if (domain.Value.HeightDirection != Axis.None)
            {
                Axis direction = domain.Value.HeightDirection;
                Symbol bottomSign = domain.FirstChild.Value.Sign;
                Symbol topSign = domain.SecondChild.Value.Sign;
                Symbol sign = Symbol.None;
                if (bottomSign == Symbol.Zero)
                {
                    sign = Symbol.Minus;
                }
                else if (topSign == Symbol.Zero)
                {
                    sign = Symbol.Plus;
                }
                if (sign != Symbol.None)
                {
                    EmbeddedHyperRectangle unit = EmbeddedHyperRectangle.UnitCube(domain.Value.Geometry.Dimension);
                    surface = unit.Face((int)direction, sign);
                    return true;
                }
            }
            surface = null;
            return false;
        }

        public static Embedding Plane(EmbeddedHyperRectangle face)
        {
            Tensor2 b = Tensor2.Zeros(face.SpaceDimension, face.Dimension);
            int j = 0;
            for (int i = 0; i < face.SpaceDimension; ++i)
            {
                if (face.ActiveDimensions[i])
                {
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

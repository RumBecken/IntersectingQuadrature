using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map
{

    internal class NestedSet
    {

        public IScalarFunction Alpha;

        public BinaryNode<Set> Root;

        public NestedSet(IScalarFunction alpha, EmbeddedHyperRectangle geometry)
        {
            Set domain = new Set(geometry);
            Root = new BinaryNode<Set>(domain);
            Alpha = alpha;
        }

        public NestedSet(IScalarFunction alpha, Set domain)
        {
            Root = new BinaryNode<Set>(domain);
            Alpha = alpha;
        }

        public NestedSet(IScalarFunction alpha, BinaryNode<Set> root)
        {
            Root = root;
            Alpha = alpha;
        }

        public int Height()
        {
            return BinaryNode<Set>.Height(Root);
        }

        public SetList Gather(int level)
        {
            SetList faces = new SetList(Root.Value.Geometry.Dimension - level);
            Root.Descendants(level, faces);
            return faces;
        }

        public SetList LowestLeafs()
        {
            int level = Height();
            return Gather(level);
        }

        public NestedSet Clone()
        {
            return Clone(this);
        }

        public static NestedSet Clone(NestedSet domain)
        {
            BinaryNode<Set> root = Clone(domain.Root);
            return new NestedSet(domain.Alpha, root);
        }

        public static BinaryNode<Set> Clone(BinaryNode<Set> source)
        {

            Set domainClone = source.Value.Clone();
            BinaryNode<Set> clone = new BinaryNode<Set>(domainClone);
            if (source.FirstChild != null)
            {
                clone.FirstChild = Clone(source.FirstChild);
                clone.FirstChild.Parent = clone;
            }
            if (source.SecondChild != null)
            {
                clone.SecondChild = Clone(source.SecondChild);
                clone.SecondChild.Parent = clone;
            }
            return clone;
        }

        DomainComparer comparer = new DomainComparer();

        public bool TryFind(Set value, out BinaryNode<Set> result)
        {
            if (Contains(Root.Value.Geometry.ActiveDimensions, value.Geometry.ActiveDimensions))
            {
                return Root.TryFind(value, comparer, out result);
            }
            else
            {
                result = null;
                return false;
            }
        }

        //a contains b ?
        static bool Contains(BitArray a, BitArray b)
        {
            bool contains = true;
            for (int i = 0; i < a.Count; ++i)
            {
                contains &= (a[i] || b[i]) == a[i];
            }
            return contains;
        }
    }
}

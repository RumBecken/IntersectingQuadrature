using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectingQuadrature.Map
{

    internal class BinaryNode<T>
    {

        public BinaryNode<T> Parent;

        public BinaryNode<T> FirstChild;

        public BinaryNode<T> SecondChild;

        public T Value;

        public BinaryNode(T value)
        {
            Value = value;
        }

        public int Height()
        {
            return Height(this);
        }

        public static int Height(BinaryNode<T> node)
        {
            if (node != null)
            {
                return Math.Max(Height(node.FirstChild), Height(node.SecondChild)) + 1;
            }
            else
            {
                return -1;
            }
        }

        public int Depth()
        {
            return Depth(this);
        }

        public static int Depth(BinaryNode<T> node)
        {
            if (node != null)
            {
                return Depth(node.Parent) + 1;
            }
            else
            {
                return -1;
            }
        }

        public LinkedList<BinaryNode<T>> Descendants(int depth)
        {
            return Descendants(this, depth);
        }

        public static LinkedList<BinaryNode<T>> Descendants(BinaryNode<T> node, int depth)
        {
            LinkedList<BinaryNode<T>> faces = new LinkedList<BinaryNode<T>>();
            Descendants(node, depth, faces);
            return faces;
        }

        public void Descendants(int depth, LinkedList<BinaryNode<T>> target)
        {
            Descendants(this, depth, target);
        }

        public static void Descendants(BinaryNode<T> node, int depth, LinkedList<BinaryNode<T>> target)
        {
            if (node == null)
            {
                return;
            }
            else if (depth == 0)
            {
                target.AddLast(node);
            }
            else
            {
                depth--;
                Descendants(node.FirstChild, depth, target);
                Descendants(node.SecondChild, depth, target);
            }
        }

        /// <returns>Descendants including itself</returns>
        public IEnumerable<BinaryNode<T>> Descendants()
        {
            return Descendants(this);
        }

        public static IEnumerable<BinaryNode<T>> Descendants(BinaryNode<T> source)
        {
            if (source != null)
            {
                yield return source;
                foreach (BinaryNode<T> subSpace in Descendants(source.FirstChild))
                {
                    yield return subSpace;
                }
                foreach (BinaryNode<T> subSpace in Descendants(source.SecondChild))
                {
                    yield return subSpace;
                }
            }
        }

        public IEnumerable<BinaryNode<T>> Descendants(int from, int to)
        {
            return Descendants(this, from, to);
        }

        public static IEnumerable<BinaryNode<T>> Descendants(BinaryNode<T> source, int from, int to)
        {
            if (source != null)
            {
                if (from <= 0 && to >= 0)
                {
                    yield return source;
                    from--;
                    to--;
                    foreach (BinaryNode<T> subSpace in Descendants(source.FirstChild, from, to))
                    {
                        yield return subSpace;
                    }
                    foreach (BinaryNode<T> subSpace in Descendants(source.SecondChild, from, to))
                    {
                        yield return subSpace;
                    }
                }
            }
        }

        public bool TryFind(T value, IComparer<T> comparer, out BinaryNode<T> result)
        {
            return TryFind(this, value, comparer, out result);
        }

        public static bool TryFind(BinaryNode<T> node, T value, IComparer<T> comparer, out BinaryNode<T> result)
        {
            if (node == null)
            {
                result = null;
                return false;
            }
            else
            {
                int d = comparer.Compare(value, node.Value);
                if (d == 0)
                {
                    result = node;
                    return true;
                }
                else if (d < 1)
                {
                    return TryFind(node.FirstChild, value, comparer, out result);
                }
                else
                {
                    return TryFind(node.SecondChild, value, comparer, out result);
                }
            }
        }

        public void RemoveChildren()
        {
            if (FirstChild != null)
            {
                FirstChild.Parent = null;
                FirstChild = null;
            }
            if (SecondChild != null)
            {
                SecondChild.Parent = null;
                SecondChild = null;
            }
        }
    }
}

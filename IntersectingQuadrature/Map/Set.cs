namespace IntersectingQuadrature.Map
{
    internal class Set : ICloneable<Set>
    {

        public Symbol Sign;

        public Axis HeightDirection;

        public bool Graphable = true;

        public HyperRectangle Geometry;

        public HyperRectangle BoundingBox;

        public Set(HyperRectangle geometry)
        {
            Sign = Symbol.None;
            HeightDirection = Axis.None;
            Geometry = geometry;
            BoundingBox = geometry.Clone();
        }

        protected Set()
        {
            Sign = Symbol.None;
            HeightDirection = Axis.None;
        }

        public Set Face(Axis direction, Symbol sign)
        {
            HyperRectangle face = Geometry.Face((int)direction, sign);
            return new Set(face);
        }

        public virtual void Resize(double coordinate, Axis direction, Symbol side)
        {
            Geometry.Resize(coordinate, (int)direction, side);
        }

        public virtual void Relocate(double coordinate, Axis direction)
        {
            Geometry.Relocate(coordinate, (int)direction);
        }

        public Set Clone()
        {
            Set clone = new Set();
            Clone(this, clone);
            return clone;
        }

        protected static void Clone(Set source, Set target)
        {
            target.Geometry = source.Geometry.Clone();
            target.BoundingBox = source.BoundingBox.Clone();
            target.Sign = source.Sign;
            target.HeightDirection = source.HeightDirection;
        }
    }
}

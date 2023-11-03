using NUnit.Framework;
using IntersectingQuadrature.Tensor;

namespace IntersectingQuadrature.Map.Graph
{
    internal static class ResizerTests
    {
        static BinaryNode<Set> UnitSet()
        {
            EmbeddedHyperRectangle unitCube = EmbeddedHyperRectangle.UnitCube(3);
            Set set = new Set(unitCube);
            BinaryNode<Set> cube = new BinaryNode<Set>(set);
            set.HeightDirection = Axis.X;
            cube.FirstChild = new BinaryNode<Set>(set.Face(Axis.X, Symbol.Minus));
            cube.SecondChild = new BinaryNode<Set>(set.Face(Axis.X, Symbol.Plus));

            BinaryNode<Set> plane = cube.FirstChild;
            set = plane.Value;
            set.HeightDirection = Axis.Y;
            plane.FirstChild = new BinaryNode<Set>(set.Face(Axis.Y, Symbol.Minus));
            plane.SecondChild = new BinaryNode<Set>(set.Face(Axis.Y, Symbol.Plus));

            BinaryNode<Set> line = plane.FirstChild;
            set = line.Value;
            set.HeightDirection = Axis.Z;
            line.FirstChild = new BinaryNode<Set>(set.Face(Axis.Z, Symbol.Minus));
            line.SecondChild = new BinaryNode<Set>(set.Face(Axis.Z, Symbol.Plus));

            return cube;
        }

        [Test]
        public static void ResizeCubeXPlus()
        {
            BinaryNode<Set> cubeSet = UnitSet();
            Resizer.Resize(cubeSet, Axis.X, 0, Symbol.Plus);

            EmbeddedHyperRectangle cube = cubeSet.Value.Geometry;
            EmbeddedHyperRectangle expectedCube = new EmbeddedHyperRectangle(3);
            expectedCube.Center = Tensor1.Vector(-0.5, 0, 0);
            expectedCube.Diameters = Tensor1.Vector(1, 2, 2);
            Assert.IsTrue(AreEqual(cube, expectedCube));

            EmbeddedHyperRectangle plane = cubeSet.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedPlane = new EmbeddedHyperRectangle(3);
            expectedPlane.Center = Tensor1.Vector(-1, 0, 0);
            expectedPlane.Diameters = Tensor1.Vector(0, 2, 2);
            Assert.IsTrue(AreEqual(plane, expectedPlane));

            EmbeddedHyperRectangle line = cubeSet.FirstChild.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedline = new EmbeddedHyperRectangle(3);
            expectedline.Center = Tensor1.Vector(-1, -1, 0);
            expectedline.Diameters = Tensor1.Vector(0, 0, 2);
            Assert.IsTrue(AreEqual(line, expectedline));
        }

        [Test]
        public static void ResizeCubeXMinus()
        {
            BinaryNode<Set> cubeSet = UnitSet();
            Resizer.Resize(cubeSet, Axis.X, 0, Symbol.Minus);

            EmbeddedHyperRectangle cube = cubeSet.Value.Geometry;
            EmbeddedHyperRectangle expectedCube = new EmbeddedHyperRectangle(3);
            expectedCube.Center = Tensor1.Vector(0.5, 0, 0);
            expectedCube.Diameters = Tensor1.Vector(1, 2, 2);
            Assert.IsTrue(AreEqual(cube, expectedCube));

            EmbeddedHyperRectangle plane = cubeSet.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedPlane = new EmbeddedHyperRectangle(3);
            expectedPlane.Center = Tensor1.Vector(0, 0, 0);
            expectedPlane.Diameters = Tensor1.Vector(0, 2, 2);
            Assert.IsTrue(AreEqual(plane, expectedPlane));

            EmbeddedHyperRectangle line = cubeSet.FirstChild.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedline = new EmbeddedHyperRectangle(3);
            expectedline.Center = Tensor1.Vector(0, -1, 0);
            expectedline.Diameters = Tensor1.Vector(0, 0, 2);
            Assert.IsTrue(AreEqual(line, expectedline));
        }

        static bool AreEqual(EmbeddedHyperRectangle a, EmbeddedHyperRectangle b)
        {
            double c = (a.Center - b.Center) * (a.Center - b.Center);
            double d = (a.Diameters - b.Diameters) * (a.Diameters - b.Diameters);
            return c < 1e-9 && d < 1e-9;
        }

        [Test]
        public static void ResizeCubeYPlus()
        {
            BinaryNode<Set> cubeSet = UnitSet();
            Resizer.Resize(cubeSet, Axis.Y, 0, Symbol.Plus);

            EmbeddedHyperRectangle cube = cubeSet.Value.Geometry;
            EmbeddedHyperRectangle expectedCube = new EmbeddedHyperRectangle(3);
            expectedCube.Center = Tensor1.Vector(0, -0.5, 0);
            expectedCube.Diameters = Tensor1.Vector(2, 1, 2);
            Assert.IsTrue(AreEqual(cube, expectedCube));

            EmbeddedHyperRectangle plane = cubeSet.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedPlane = new EmbeddedHyperRectangle(3);
            expectedPlane.Center = Tensor1.Vector(-1, -0.5, 0);
            expectedPlane.Diameters = Tensor1.Vector(0, 1, 2);
            Assert.IsTrue(AreEqual(plane, expectedPlane));

            EmbeddedHyperRectangle line = cubeSet.FirstChild.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedline = new EmbeddedHyperRectangle(3);
            expectedline.Center = Tensor1.Vector(-1, -1, 0);
            expectedline.Diameters = Tensor1.Vector(0, 0, 2);
            Assert.IsTrue(AreEqual(line, expectedline));
        }

        [Test]
        public static void ResizeCubeYMinus()
        {
            BinaryNode<Set> cubeSet = UnitSet();
            Resizer.Resize(cubeSet, Axis.Y, 0, Symbol.Minus);

            EmbeddedHyperRectangle cube = cubeSet.Value.Geometry;
            EmbeddedHyperRectangle expectedCube = new EmbeddedHyperRectangle(3);
            expectedCube.Center = Tensor1.Vector(0, 0.5, 0);
            expectedCube.Diameters = Tensor1.Vector(2, 1, 2);
            Assert.IsTrue(AreEqual(cube, expectedCube));

            EmbeddedHyperRectangle plane = cubeSet.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedPlane = new EmbeddedHyperRectangle(3);
            expectedPlane.Center = Tensor1.Vector(-1, 0.5, 0);
            expectedPlane.Diameters = Tensor1.Vector(0, 1, 2);
            Assert.IsTrue(AreEqual(plane, expectedPlane));

            EmbeddedHyperRectangle line = cubeSet.FirstChild.FirstChild.Value.Geometry;
            EmbeddedHyperRectangle expectedline = new EmbeddedHyperRectangle(3);
            expectedline.Center = Tensor1.Vector(-1, 0, 0);
            expectedline.Diameters = Tensor1.Vector(0, 0, 2);
            Assert.IsTrue(AreEqual(line, expectedline));
        }
    }
}

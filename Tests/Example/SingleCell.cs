using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Experiments.Shapes;
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;
using NUnit.Framework;
using static System.Formats.Asn1.AsnWriter;

namespace Tests.Example
{
    internal static class SingleCell {

        [Test]
        public static void QuarterTubeTest() {
            IScalarFunction alpha = new Cylinder(Tensor1.Vector(1, 1, 0), 1);


            Quadrater finder = new Quadrater();
            HyperRectangle unitDomain = HyperRectangle.UnitCube(3);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, unitDomain, 4);

            IScalarFunction f = new ConstantPolynomial(1, 3);
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(Math.PI /2, Is.EqualTo(F).Within(1e-2));
        }


        [Test]
        public static void PointSurfaceScalingTest([Values(0.5, 1, 1.5)] double scale) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1), Tensor1.Vector(0.1));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(1);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Zero, cell, 1);

            Assert.That(rule.Count, Is.EqualTo(1));
            Assert.That(rule[0].Point[0], Is.EqualTo(0.1).Within(1e-10));
        }

        [Test]
        public static void TwoPointSurfaceTest() {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1), Tensor1.Vector(0.1));
            IScalarFunction beta = new Plane(Tensor1.Vector(1), Tensor1.Vector(0.2));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(1);
            QuadratureRule minusRule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Zero, cell, 1);
            
            Assert.That(minusRule.Count, Is.EqualTo(1));
            Assert.That(minusRule[0].Point[0], Is.EqualTo(0.1).Within(1e-10));

            QuadratureRule plusRule = finder.FindRule(beta, Symbol.Plus, alpha, Symbol.Zero, cell, 1);
            Assert.That(plusRule.Count, Is.EqualTo(0));
        }

        [Test]
        public static void TwoPointSurfaceReverseTest() {

            IScalarFunction alpha = new Plane(Tensor1.Vector(-1), Tensor1.Vector(0.1));
            IScalarFunction beta = new Plane(Tensor1.Vector(-1), Tensor1.Vector(0.2));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(1);
            QuadratureRule minusRule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Zero, cell, 1);

            Assert.That(minusRule.Count, Is.EqualTo(0));


            QuadratureRule plusRule = finder.FindRule(beta, Symbol.Plus, alpha, Symbol.Zero, cell, 1);
            Assert.That(plusRule.Count, Is.EqualTo(1));
            Assert.That(plusRule[0].Point[0], Is.EqualTo(0.1).Within(1e-10));
        }

        [Test]
        public static void PlaneSurfaceScalingTest([Values(0.5, 1, 1.5)] double scale) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, 0), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(3);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(alpha, Symbol.Zero, cell, 1);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = Algebra.Pow(scale * 2.0, 2);
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(exact, Is.EqualTo(F).Within(1e-10));
        }

        [Test]
        public static void TwoPlaneSurfaceScalingTest([Values(0.5, 1, 1.5)] double scale) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, 0), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(0, 1,0), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(3);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Zero, cell, 1);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = Algebra.Pow(scale * 2,2)/2;
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(exact, Is.EqualTo(F).Within(1e-10));
        }

        [Test]
        public static void TwoPlaneIntersectionScalingTest([Values(0.5, 1, 1.5)] double scale) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, 0), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(0, 1, 0), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(3);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Zero, alpha, Symbol.Zero, cell, 1);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = scale * 2;
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(exact, Is.EqualTo(F).Within(1e-10));
        }

        [Test]
        public static void TwoLineIntersectionTest([Values(0.5, 1, 1.5)] double scale) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0), Tensor1.Vector(0.1,0));
            IScalarFunction beta = new Plane(Tensor1.Vector(0, 1), Tensor1.Vector(0, 0.2));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(2);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Zero, alpha, Symbol.Zero, cell, 1);

            Assert.That(rule[0].Point[0], Is.EqualTo(0.1).Within(1e-10));
            Assert.That(rule[0].Point[1], Is.EqualTo(0.2).Within(1e-10));
            Assert.That(rule[0].Weight, Is.EqualTo(1).Within(1e-10));
        }

        [Test]
        public static void TwoPlaneVolumeScalingTest([Values( 0.5, 1, 1.5)]double scale) {

            IScalarFunction alpha = new Plane (Tensor1.Vector(1, 0, 1), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(1, 0, 1), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(3);
            Algebra.Scale(cell.Diameters, scale);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Minus, cell, 1);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = Algebra.Pow(scale * 2, 3) / 4 + scale * scale * 2 * (scale);
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(exact, Is.EqualTo(F).Within(1e-10));
        }

        [Test]
        public static void TwoPlaneVolumeRotationTest([Values(0, 0.25, 0.5, 0.75)] double a) {

            IScalarFunction alpha = new Plane(Tensor1.Vector(1, 0, a), Tensor1.Zeros(3));
            IScalarFunction beta = new Plane(Tensor1.Vector(a, 0, 1), Tensor1.Zeros(3));

            Quadrater finder = new Quadrater();
            HyperRectangle cell = HyperRectangle.UnitCube(3);
            QuadratureRule rule = finder.FindRule(beta, Symbol.Minus, alpha, Symbol.Minus, cell, 10);

            IScalarFunction f = new ConstantPolynomial(1);

            double exact = Algebra.Pow( 2, 3) / 4 +  2  * a;
            double F = Quadrature.Evaluate(f, rule);
            Assert.That(exact, Is.EqualTo(F).Within( 1e-8));
        }
    }
}

﻿using IntersectingQuadrature.Tensor;
using Example.Experiments;
using Example;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Example.Experiments.Shapes;

namespace Tests.Example
{
    internal class SheetTest
    {

        [Test]
        public static void Sphere()
        {
            double r = 1;
            double d = 0.1;

            IScalarFunction sphere = new Sphere(Tensor1.Vector(0, 0), r);
            IScalarFunction alpha = new Sheet(sphere, d);


            Tensor1 X = Tensor1.Vector(2, 3);
            (double F, Tensor1 dF, Tensor2 ddF) = alpha.EvaluateAndGradientAndHessian(X);
            (double f, Tensor1 df, Tensor2 ddf) = AnalyticF(X, r, d);
            Assert.That(F, Is.EqualTo( f).Within( 1e-10));
            Assert.That(dF[0], Is.EqualTo(df[0]).Within(1e-10));
            Assert.That(dF[1], Is.EqualTo(df[1]).Within(1e-10));
            Assert.That(ddF[0, 0], Is.EqualTo(ddf[0, 0]).Within(1e-10));
            Assert.That(ddF[0, 1], Is.EqualTo(ddf[0, 1]).Within(1e-10));
            Assert.That(ddF[1, 0], Is.EqualTo(ddf[1, 0]).Within(1e-10));
            Assert.That(ddF[1, 1], Is.EqualTo(ddf[1, 1]).Within(1e-10));
        }


        static (double f, Tensor1 df, Tensor2 ddf) AnalyticF(Tensor1 X, double r, double d)
        {
            double x = X[0];
            double y = X[1];
            double f = Algebra.Pow(x * x + y * y - r * r, 2) - d * d;
            Tensor1 df = Tensor1.Zeros(2);
            df[0] = (x * x + y * y - r * r) * 4 * x;
            df[1] = (x * x + y * y - r * r) * 4 * y;

            Tensor2 ddf = Tensor2.Zeros(2);
            ddf[0, 0] = (3 * x * x + y * y - r * r) * 4;
            ddf[0, 1] = 8 * x * y;
            ddf[1, 0] = 8 * x * y;
            ddf[1, 1] = (x * x + 3 * y * y - r * r) * 4;

            return (f, df, ddf);
        }

    }
}

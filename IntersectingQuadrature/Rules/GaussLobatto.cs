using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature.Rules {
    internal class GaussLobatto {
        public static QuadratureRule Rule(int n) {
            QuadratureRule gaussRule = new QuadratureRule(n);
            switch (n) {
                case 2:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1
                });
                break;
                case 3:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 3.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0),
                    Weight = 4.0 / 3.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 3.0
                });
                break;
                case 4:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-Math.Sqrt(1.0 / 5.0)),
                    Weight = 5.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(Math.Sqrt(1.0 / 5.0)),
                    Weight = 5.0 / 6.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 6.0
                });
                break;
                case 5:
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-1),
                    Weight = 1.0 / 10.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(-Math.Sqrt(3.0 / 7.0)),
                    Weight = 49.0 / 90.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(0.0000000000000000),
                    Weight = 32.0 / 45.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(Math.Sqrt(3.0 / 7.0)),
                    Weight = 49.0 / 90.0
                });
                gaussRule.Add(new QuadratureNode() {
                    Point = Tensor1.Vector(1),
                    Weight = 1.0 / 10.0
                });
                break;
                default:
                throw new NotImplementedException();
            }
            return gaussRule;
        }
    }
}

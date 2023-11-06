# Intersecting Quadrature 

A package that offers methods to create quadrature rules for domains defined by one or two intersecting level sets.
It is used in the discontinuous Galerkin framework [BoSSS](https://github.com/FDYdarmstadt/BoSSS) developed by the chair of fluid dynamics, Technical University of Darmstadt. A detailed description of the method can be found on [arXiv](https://doi.org/10.48550/arXiv.2308.10698).

## Quick Start

Create a .dotNet project and simply include the NuGet package in your .Net project through nuget.org.

Place this code in program.cs and run it :
```cs
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example {

  class Program {
    
    static void Main(string[] args) {
      IScalarFunction alpha = new LinearPolynomial(0, Tensor1.Vector(1, 0, 0));
      IScalarFunction beta = new LinearPolynomial(0, Tensor1.Vector(0, 1, 0));

      Quadrater finder = new Quadrater();
      HyperRectangle cell = HyperRectangle.UnitCube(3);
      QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, cell, 3);
    }
  }
}
```
This simple example creates a quadrature rule over the volume of a 3-dimensional domain with flat surfaces.

## Usage

You have two options to use this package. You can include it as a libary in your project as a NuGet package, or 
add IntersectingQuadrature.csproj to your project. 

To construct a [`QuadratureRule rule`](api/IntersectingQuadrature.QuadratureRule.yml) : 
- Implement level sets [`IScalarFunction Alpha`](api/TensorAnalysis.IScalarFunction.yml) 
  and [`IScalarFunction Beta`](api/TensorAnalysis.IScalarFunction.yml). 
- Create [`IHyperRectangle K`](api/IntersectingQuadrature.HyperRectangle.yml) of dimension *d* which confines the domain of integration.
- Determine the domain of integration by selecting 
  [`Symbol signAlpha`](api/IntersectingQuadrature.Symbol.yml) and [`Symbol signBeta`](api/IntersectingQuadrature.Symbol.yml) from {0, -, +} for `Alpha` and `Beta` respectively. 
  You will receive a quadrature rule for the set {x &isin; K | sign(Alpha(x)) = signAlpha &and; sign(Beta(x)) = signBeta}.
- Set `int n` to define the number *n<sup>d</sup>* of quadrature nodes.
- Select a number `int subdivions` of subdivisions.


You can construct a quadrature rule by creating a [`Quadrater Q`](api/IntersectingQuadrature.Quadrater.yml) and calling 
`Q.FindRule(...)` :    
```cs
IntersectingQuadrature.Quadrater Q = new Quadrater();
QuadratureRule rule = Q.FindRule(alpha, signAlpha, beta, signBeta, K, n, subdivisions);
```
If required, the [AdaptiveQuadrater](api/IntersectingQuadrature.AdaptiveQuadrater.yml) 
creates a quadrature rule, which is refined adaptively according to the relative error threshold `tau` : 
```cs
double tau;
AdaptiveQuadrater Q = new AdaptiveQuadrater(tau);
QuadratureRule rule = Q.FindRule(alpha, signAlpha, beta, signBeta, K, n, subdivisions);
```

## Examples
To see examples, compile Example/Example.csproj and run the examples in program.cs by uncommenting them.  

## Related publications
- [*High-Order Numerical Integration on Domains Bounded by Intersecting Level Sets*](https://doi.org/10.48550/arXiv.2308.10698)
  We present a high-order method that provides numerical integration on volumes, surfaces, and lines defined implicitly by two smooth intersecting level sets. To approximate the integrals, the method maps quadrature rules defined on hypercubes to the curved domains of the integrals. This enables the numerical integration of a wide range of integrands since integration on hypercubes is a well known problem. The mappings are constructed by treating the isocontours of the level sets as graphs of height functions. Numerical experiments with smooth integrands indicate a high-order of convergence for transformed Gauss quadrature rules on domains defined by polynomial, rational, and trigonometric level sets. We show that the approach we have used can be combined readily with adaptive quadrature methods. Moreover, we apply the approach to numerically integrate on difficult geometries without requiring a low-order fallback method.

![Test status](https://github.com/RumBecken/IntersectingQuadrature/actions/workflows/Test.yml/badge.svg)
![Docs status](https://github.com/RumBecken/IntersectingQuadrature/actions/workflows/Docs.yml/badge.svg)
# ![Intersecting Quadrature](Documentation/pictures/logo.svg) 

A package that offers methods to create quadrature rules for domains defined by one or two intersecting level sets.
It is used in the discontinuous Galerkin framework [BoSSS](https://github.com/FDYdarmstadt/BoSSS) developed by the chair of fluid dynamics, Technical University of Darmstadt.

## Quick Start 
Create a .Net project and simply include the NuGet package in your .Net project through nuget.org.

Place this code in program.cs and run it :
```cs
using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;

namespace Example {

  class Program {
    
    static void Main(string[] args) {
      IScalarFunction alpha = new LinearPolynomial(0, Tensor1.Vector(1, 0, 0));
      IScalarFunction beta = new LinearPolynomial(0, Tensor1.Vector(0, 1, 0));

      IQuadrater finder = IntersectingQuadrature.Methods.Create();
      HyperRectangle cell = HyperRectangle.UnitCube(3);
      QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, cell, 3);
    }
  }
}
```
This simple example creates a quadrature rule with $3 \times 3 \times 3 = 27$ nodes over the volume of a 3-dimensional domain with flat surfaces.

## Usage and documentation 
Simply include the NuGet package in your .Net project and have a look at the 
[documentation](https://rumbecken.github.io/IntersectingQuadrature/).

If you want to learn about the method you can find a detailed explanation in this [paper](https://doi.org/10.48550/arXiv.2308.10698).

## Authors and acknowledgment
Lauritz Beck, [Chair of Fluid Dynamics](https://www.fdy.tu-darmstadt.de/fdy/index.en.jsp), Technical University of Darmstadt

[Graduate-School-CE](https://www.ce.tu-darmstadt.de/graduate_school_ce/index.en.jsp), Technical University of Darmstadt 

[![SPP 2171, DFG](Documentation/pictures/sppLogo.svg)](https://www.uni-muenster.de/SPP2171/index.html)

Intersecting Quadrature gratefully uses 
* [NUnit](https://github.com/nunit/nunit) licensed under MIT
* [docfx](https://github.com/dotnet/docfx/) licensed under MIT
* [Gauss quadrature rules](https://pomax.github.io/bezierinfo/legendre-gauss.html)

## License
Copyright (c) 2023 Lauritz Beck

MIT

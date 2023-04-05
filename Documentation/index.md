# Intersecting Quadrature 

Methods to create quadrature rules for domains defined by one or two intersecting level sets.  

## Quick Start
### NuGet 
To construct a [quadrature rule](api/IntersectingQuadrature.QuadratureRule.yml) `QuadratureRule rule`: 
- Define level sets `IscalarFunction Alpha` and  `IscalarFunction Beta` by implementing [IscalarFunction](api/TensorAnalysis.IScalarFunction.yml).
- Create an [hyperrectangle](api/IntersectingQuadrature.HyperRectangle.yml) `HyperRectangle K` 	of dimension *d* which confines the domain of integration.
- Choose the domain of integration by selecting 
  [Symbols](api/IntersectingQuadrature.Symbol.yml) `Symbol signAlpha` and `Symbol signBeta` from {0, -, +} for `Alpha` and `Beta` respectively. 
  You will receive a quadrature rule for the set T = {x in K | sign(Alpha(x)) = a &and; sign(Beta(x)) = b}.
- Set `int n` to define the number *n<sup>d</sup>* of quadrature nodes.
- Select a number `int subdivions` of subdivisions.


You can construct a quadrature rule by creating a [Quadrater](api/IntersectingQuadrature.Quadrater.yml) and calling 
`FindRule(...)` :    
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

### Example.csproj
To see examples, compile Example/Example.csproj and run the examples in program.cs by uncommenting them.  

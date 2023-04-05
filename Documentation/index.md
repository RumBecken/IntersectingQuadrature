# Intersecting Quadrature 

Methods to create quadrature rules for domains defined by one or two intersecting level sets.  

## Quick Start
### NuGet 
To construct a [QuadratureRule](api/IntersectingQuadrature.QuadratureRule.yml) `rule`: 
- Define level sets `Alpha` and  `Beta` by implementing [IscalarFunction](api/TensorAnalysis.IScalarFunction.yml).
- Create an [HyperRectangle](api/IntersectingQuadrature.HyperRectangle.yml) `K` 	&sube; R<sup>d</sup> which confines the domain of integration.
- Choose the domain of integration by selecting 
  [Symbols](api/IntersectingQuadrature.Symbol.yml) `signAlpha` and `signBeta` from {0, -, +} for `Alpha` and `Beta` respectively. 
  You will receive a quadrature rule for the set T = {x in K | sign(Alpha(x)) = a &and; sign(Beta(x)) = b}.
- Set `n` to define the number of quadrature nodes n<sup>d</sup>.
- Select a number `subdivions` of subdivisions.


You can construct a quadrature rule by creating a [Quadrater](api/IntersectingQuadrature.Quadrater.yml) :    
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

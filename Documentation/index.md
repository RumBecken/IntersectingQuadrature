# Intersecting Quadrature 

Methods to create quadrature rules for domains defined by one or two intersecting level sets.  

## Quick Start
You have two options to use this package. You can include it as a libary in your project as a NuGet package, or 
run examples from Example.csproj. 

### NuGet 
To construct a [`QuadratureRule rule`](api/IntersectingQuadrature.QuadratureRule.yml) : 
- Define level sets [`IScalarFunction Alpha`](api/TensorAnalysis.IScalarFunction.yml) 
  and [`IScalarFunction Beta`](api/TensorAnalysis.IScalarFunction.yml) by implementing the interface. 
- Create [`HyperRectangle K`](api/IntersectingQuadrature.HyperRectangle.yml) of dimension *d* which confines the domain of integration.
- Determine the domain of integration by selecting 
  [signAlpha`](api/IntersectingQuadrature.Symbol.yml) and [`Symbol signBeta`](api/IntersectingQuadrature.Symbol.yml) from {0, -, +} for `Alpha` and `Beta` respectively. 
  You will receive a quadrature rule for the set {x in K | sign(Alpha(x)) = a &and; sign(Beta(x)) = b}.
- Set `int n` to define the number *n<sup>d</sup>* of quadrature nodes.
- Select a number `int subdivions` of subdivisions.


You can construct a quadrature rule by creating a [Quadrater Q](api/IntersectingQuadrature.Quadrater.yml) and calling 
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

### Example.csproj
To see examples, compile Example/Example.csproj and run the examples in program.cs by uncommenting them.  

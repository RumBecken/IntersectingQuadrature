# Intersecting Quadrature

## Description
Methods to create quadrature rules for domains defined by one or two intersecting level sets.  

## Installation
NuGet

## Usage
NuGet: 
```csharp
IScalarFunction alpha; 
IScalarFunction beta;
Symbol signAlpha;
Symbol signBeta;
Hyperrectangle K; 
int n;
int subdivisions;

IntersectingQuadrature.Quadrater Q = new Quadrater();
QuadratureRule rule = Q.FindRule(alpha, signAlpha, beta, signBeta, K, n, subdivisions);

double tau;
AdaptiveQuadrater AQ = new AdaptiveQuadrater(tau);
QuadratureRule adaptedRule = AQ.FindRule(alpha, signAlpha, beta, signBeta, K, n, subdivisions);
```

Source Code: 
Compile and run examples, in program.cs of project Example 

## Documentation 


## Authors and acknowledgment
Lauritz Beck, 
Chair of Fluid Dynamics, TU Darmstadt 
GSC Tu Darmstadt 
SP2171, DFG

Intersecting Quadrature uses 
* [NUnit](https://github.com/nunit/nunit) licensed under MIT
* [docfx](https://github.com/dotnet/docfx/) licensed under MIT

## License
MIT


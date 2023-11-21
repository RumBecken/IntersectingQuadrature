using IntersectingQuadrature.Map;
using IntersectingQuadrature.Rules;
using IntersectingQuadrature.Tensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectingQuadrature {

    /// <summary>
    /// Provides tensorized quadrature rules on subdomains of a hyperrectangle bounded by one or two level sets.
    /// </summary>
    public interface IQuadrater {

        /// <summary>
        /// Constructs a tensorized Gauss quadrature rule on a subdomain of a hyperrectangle 
        /// bounded by the zero-isocontour of a level set.
        /// </summary>
        /// <param name="alpha">Level set</param>
        /// <param name="sign">Sign of domain defined by level set</param>
        /// <param name="domain"></param>
        /// <param name="n">Number of quadrature nodes per dimension</param>
        /// <param name="subdivisions">Additional subdivision of the tensorized quadrature rule</param>
        /// <returns></returns>
        QuadratureRule FindRule(IScalarFunction alpha, Symbol sign, IHyperRectangle domain, int n, int subdivisions = 0);

        /// <summary>
        /// Constructs a tensorized Gauss quadrature rule on a subdomain of a hyperrectangle 
        /// bounded by the zero-isocontours of two level sets.
        /// </summary>
        /// <param name="alpha">Level set</param>
        /// <param name="signAlpha">Sign of domain defined by level set alpha</param>
        /// <param name="beta">Level Set</param>
        /// <param name="signBeta">Sign of domain defined by level set beta</param>
        /// <param name="domain"></param>
        /// <param name="n">Number of quadrature nodes per dimension</param>
        /// <param name="subdivisions">Additional subdivision of the tensorized quadrature rule</param>
        /// <returns></returns>
        QuadratureRule FindRule(IScalarFunction alpha, Symbol signAlpha, IScalarFunction beta, Symbol signBeta, IHyperRectangle domain, int n, int subdivisions = 0);
    }
}

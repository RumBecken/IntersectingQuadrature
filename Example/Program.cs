using IntersectingQuadrature;
using IntersectingQuadrature.Tensor;


IScalarFunction alpha = new LinearPolynomial(0, Tensor1.Vector(1, 0, 0));
IScalarFunction beta = new LinearPolynomial(0, Tensor1.Vector(0, 1, 0));

Quadrater finder = new Quadrater();
HyperRectangle cell = new UnitHyperCube(3);
QuadratureRule rule = finder.FindRule(alpha, Symbol.Minus, beta, Symbol.Minus, cell, 3);



//*/
//Example.Experiments.Grid.Torus();
//Example.Experiments.Sheets.SphereSurface();
//Example.Experiments.Sheets.WaveSurface();
//Example.Experiments.SingleCell.CylindricSheet();
//Example.Experiments.SingleCell.TorusCap();
//Example.Experiments.SingleCell.SphericSheet();


//Example.Experiments.Grid.Potato();
//Example.Experiments.SlantedSheet.Subdivision();
//Example.Experiments.SlantedSheet.SubdivisionPlot();
//Example.Experiments.Convergence.LipVolume(3);
//Example.Experiments.SingleCell.TwoPlaneVolume(2);
//Example.Experiments.OscillatingEdge.Surface(1);

//Example.Experiments.Ufo.LineH(2);

//Example.Experiments.Ufo.LineAdaptive(0.1,1);
//Example.Experiments.TrilinearTunnel.Exact();
//Example.Experiments.SphericalSheet.TorusSurface();
//Example.Experiments.SphericalSheet.WaveSurface();

/*
for (int n = 1; n < 5; ++n) {
    Example.Experiments.OscillatingEdge.Line(n);
    Example.Experiments.OscillatingEdge.Surface(n);
    Example.Experiments.OscillatingEdge.Volume(n);
}
//*/
Example.Experiments.SingleCell.Line();
//Example.Experiments.ToricSection.Line(1);
/*
for (int n = 1; n < 5; ++n) {
    //Example.Experiments.ToricSection.Line(n);
    Example.Experiments.ToricSection.Surface(n);
}
//*/
//Example.Experiments.Ufo.LineAdaptive(0.001, 2);

/*
for (int n = 1; n < 5; ++n) {
    Example.Experiments.Ufo.Line(n);
    Example.Experiments.Ufo.Surface(n);
    Example.Experiments.Ufo.Volume(n);
    
}
//*/
//Example.Experiments.Ufo.LineH(2);

//Example.Experiments.Ufo.Volume(n);

//Example.Experiments.Convergence.SphereSurface(4);

//Example.Experiments.OscillatingEdge.Volume(3);
//Example.Experiments.Ufo.Line(4);
//Example.Experiments.Convergence.SphereSurface(4);


//singleCell[1](4);


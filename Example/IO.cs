
namespace IntersectingQuadrature{
    internal class IO{
        public static void Write(string file, QuadratureRule rule){
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            using (StreamWriter f = new StreamWriter(file)) {
                foreach (QuadratureNode node in rule) {
                    string point = "";
                    for(int i = 0; i < node.Point.M; ++i) {
                        point += $"{node.Point[i]},";
                    }
                    f.WriteLine(point + $"{node.Weight}");
                }
            }
        }

        public static void Write(string file, params QuadratureRule[][,,] rulesList) {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            using (StreamWriter f = new StreamWriter(file)) {
                int c = 0;
                foreach (QuadratureRule[,,] rules in rulesList) {
                    foreach (QuadratureRule rule in rules) {
                        foreach (QuadratureNode node in rule) {
                            string point = "";
                            for (int i = 0; i < node.Point.M; ++i) {
                                point += $"{node.Point[i]},";
                            }
                            f.WriteLine(point + $"{node.Weight}," + $"{c}");
                        }
                        ++c;
                    }
                }
            }
        }

        public static void Write(string file, IEnumerable<IEnumerable<double>> convergence) {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            using (StreamWriter f = new StreamWriter(file)) {
                foreach(var node in convergence) {
                    string line = "";
                    foreach(double member in node) {
                        line += $"{member},";
                    }
                    f.WriteLine(line);
                }
            }
        }
    }
}

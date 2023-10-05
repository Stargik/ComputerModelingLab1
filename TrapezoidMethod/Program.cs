namespace TrapezoidMethod;

class Program
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    static void Main(string[] args)
    {
        bool isShow = true;
        double a = 2;
        double b = 30;
        int n = 2;
        double eps = 0.0025;
        double result1, result2;
        int p = 2;
        int iteration = 0;
        Func<double, double> func = x => { return 1 / (1 + Math.Pow(x, 3)); };
        List<Point> nodes = new List<Point>();
        double h = (b - a) / n;

        Console.WriteLine($"n = {n}; h = {h}");
        nodes = GetNodes(func, a, b, n);
        ShowNodes(nodes, isShow, iteration);

        result1 = TrapezoidMethod(nodes, h);
        Console.WriteLine($"Result I(h/(2^{iteration})): {result1}");
        Console.WriteLine("-----------------------------------------");
        iteration++;
        n *= 2;
        h *= 0.5;
        Console.WriteLine($"n = {n}; h = {h}");
        nodes = GetNodes(func, a, b, n);
        ShowNodes(nodes, isShow, iteration);
        result2 = TrapezoidMethod(nodes, h);
        Console.WriteLine($"Result I(h/(2^{iteration})): {result2}");
        double resultEps = CheckRungeRule(result1, result2, p);

        while (!(resultEps < eps))
        {
            Console.WriteLine($"|I - I(h/(2^{iteration})| = {resultEps} >= {eps}");
            Console.WriteLine("-----------------------------------------");
            iteration++;
            n *= 2;
            h *= 0.5;
            result1 = result2;
            Console.WriteLine($"n = {n}; h = {h}");
            nodes = GetNodes(func, a, b, n);
            ShowNodes(nodes, isShow, iteration);
            result2 = TrapezoidRungeMethod(result2, nodes, h);
            Console.WriteLine($"Result I(h/(2^{iteration})): {result2}");
            resultEps = CheckRungeRule(result1, result2, p);
        }
        Console.WriteLine($"|I - I(h/(2^{iteration})| = {resultEps} < {eps}");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"I = {result2}");
    }

    public static List<Point> GetNodes(Func<double, double> func, double a, double b, int n)
    {

        double h = (b - a) / n;
        List<Point> nodes = new List<Point>();
        for (int i = 0; i < n + 1; i++)
        {
            double x = a + i * h;
            double y = func(x);
            Point point = new Point
            {
                X = x,
                Y = y
            };
            nodes.Add(point);
        }
        return nodes;
    }

    public static double TrapezoidMethod(List<Point> nodes, double h)
    {
        double result = (nodes.First().Y + nodes.Last().Y) / 2;
        for (int i = 1; i < nodes.Count - 1; i++)
        {
            result += nodes[i].Y;
        }
        result *= h;
        return result;
    }

    public static double TrapezoidRungeMethod(double res1, List<Point> nodes, double h)
    {
        double result = 0;
        for (int i = 1; i < nodes.Count; i = i + 2)
        {
            result += nodes[i].Y;
        }
        result *= h;
        result += res1 * 0.5;
        return result;
    }

    public static double CheckRungeRule(double res1, double res2, int p)
    {
        double resultEps = Math.Abs(res2 - res1) / (Math.Pow(2, p) - 1);
        return resultEps;
    }
    public static void ShowNodes(List<Point> nodes, bool isShow, int iteration)
    {
        if (isShow)
        {
            Console.WriteLine($"Nodes I(h/(2^{iteration})):");
            foreach (var node in nodes)
            {
                Console.WriteLine($"({node.X}; {node.Y})");
            }
        }
    }
}


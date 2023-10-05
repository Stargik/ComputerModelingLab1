using System.Drawing;
using Microsoft.VisualBasic;

namespace RombergMethod;

class Program
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    static void Main(string[] args)
    {
        int tableSize = 4;
        bool isShow = true;
        double a = 0;
        double b = 1;
        int n = 1;
        double eps = 0.0025;
        double[,] results = new double[tableSize, tableSize];
        int p = 2;
        int s = 2;
        Func<double, double> func = x => { return (2 * x) / (Math.Pow(x, 3) + 8); };
        List<Point> nodes = new List<Point>();
        double h = (b - a) / n;
        double resultEps;


        for (int i = 0; i < tableSize; i++)
        {
            if (i == 0)
            {
                Console.WriteLine($"n = {n}; h = {h}");
                nodes = GetNodes(func, a, b, n);
                ShowNodes(nodes, isShow, i);
                results[0, 0] = TrapezoidMethod(nodes, h);
                Console.WriteLine($"Result I(h/(2^{i}), 0): {results[0, 0]}");
            }
            else
            {
                Console.WriteLine("-----------------------------------------");
                n *= 2;
                h *= 0.5;
                Console.WriteLine($"n = {n}; h = {h}");
                nodes = GetNodes(func, a, b, n);
                ShowNodes(nodes, isShow, i);
                results[i, 0] = TrapezoidRungeMethod(results[i - 1, 0], nodes, h);
                Console.WriteLine($"Result I(h/(2^{i}), 0): {results[i, 0]}");
                resultEps = CheckRungeRule(results[i - 1, 0], results[i, 0], p, s, 0);
                Console.WriteLine($"|I - I(h/(2^{i}, 0)| = {resultEps}");

            }
        }
        
        for (int i = 1; i < tableSize; i++)
        {
            for (int j = 1; j < i + 1; j++)
            {
                Console.WriteLine("-----------------------------------------");
                results[i, j] = RichardsonMethod(results[i - 1, j - 1], results[i, j - 1], p, s, j - 1);
                Console.WriteLine($"Result I(h/(2^{i}), {j}): {results[i, j]}");
                if (i != j)
                {
                    resultEps = CheckRungeRule(results[i - 1, j], results[i, j], p, s, j - 1);
                    Console.WriteLine($"|I - I(h/(2^{i}, {j})| = {resultEps}");
                }
            }
        }
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("Romberg table:");
        ShowRombergTable(results);

        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"I = {results[tableSize - 1, tableSize - 1]}");
        Console.WriteLine();
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

    public static double CheckRungeRule(double res1, double res2, int p, int s, int i)
    {
        double resultEps = Math.Abs(res2 - res1) / (Math.Pow(2, p + i * s) - 1);
        return resultEps;
    }

    public static double RichardsonMethod(double res1, double res2, int p, int s, int i)
    {
        double result = (Math.Pow(2, p + i * s) * res2 - res1) / (Math.Pow(2, p + i * s) - 1);
        return result;
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

    public static void ShowRombergTable(double[,] table)
    {
        for (int i = 0; i < table.GetLength(0); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                if (j < i + 1)
                {
                    Console.Write($" {table[i, j],20} #");
                }
                else
                {
                    Console.Write($" {' ',20} #");
                }
                
            }
            Console.WriteLine();
        }
    }
}


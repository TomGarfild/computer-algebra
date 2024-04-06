namespace Lab1;

public sealed class Program
{
    public static void Main(string[] args)
    {
        Polynomial p;
        int[] u0, u1, u2;
        
        p = new Polynomial(-20, -8, 7, 3);  // 3x^3 + 7x^2 - 8x - 20
        u0 = [1,-1,20,-20,2,-2,10,-10,4,-4,5,-5];
        u1 = [1,-1,18,-18,2,-2,9,-9,3,-3,6,-6];
        
        // FindDivisible(p, u0, u1);

        p = new Polynomial(-10, 1, 3);  // 3x^2 + x - 10
        u0 = [1,-1,10,-10,2,-2,5,-5];
        u1 = [1,-1,6,-6,2,-2,3,-3];
        
        // FindDivisible(p, u0, u1);

        p = new Polynomial(-4, -11, -9, 1, 1); // x^4 + x^3 - 9x^2 - 11x -4
        u0 = [1,-1,4,-4,2,-2];
        u1 = [1,-1,22,-22,2,-2,11,-11];
        u2 = [1,-1,38,-38,2,-2,19,-19];
        
        FindDivisible3(p, u0, u1, u2);
    }

    private static void FindDivisible(Polynomial p, int[] u0, int[] u1)
    {
        var count = 0;

        foreach (var y0 in u0)
        {
            foreach (var y1 in u1)
            {
                count++;
                if (y1 == y0) continue;

                Console.Write($"{count}. [{y0},{y1}]: ");
                
                var g = new Polynomial(y0, y1-y0); // (y1-y0)x+y0

                var isDivisible = p.IsDivisibleBy(g);
                Console.WriteLine($"Is {p} divisible by {g}? {isDivisible}");

                if (isDivisible) return;
            }
        }
    }

    private static void FindDivisible3(Polynomial p, int[] u0, int[] u1, int[] u2)
    {
        var count = 0;

        foreach (var y0 in u0)
        {
            foreach (var y1 in u1)
            {
                foreach (var y2 in u2)
                {
                    count++;
                    Console.Write($"{count}. [{y0},{y1},{y2}]: ");
                    var a2 = y0 - 2 * y1 + y2;
                    var b2 = 3 * y0 - 4 * y1 + y2;
                    if ((a2 == 0 && b2 == 0) || a2 % 2 != 0 || b2 % 2 != 0)
                    {
                        Console.WriteLine();
                        continue;
                    }


                    var g = new Polynomial(y0, -b2/2f, a2/2f); // ((y0-2y1+y2)x^2 - (3y0-4y1+y2)x)/2 + y0

                    var isDivisible = p.IsDivisibleBy(g);
                    Console.WriteLine($"Is {p} divisible by {g}? {isDivisible}");

                    if (isDivisible) return;
                }
            }
        }
    }
}

public class Polynomial(params float[] coefficients)
{
    private readonly float[] _coefficients = coefficients;

    public bool IsDivisibleBy(Polynomial divisor)
    {
        var dividend = this;
        var dividendDegree = GetDegree(dividend);
        var divisorDegree = GetDegree(divisor);

        while (dividendDegree >= divisorDegree)
        {
            var factor = dividend._coefficients[dividendDegree] / divisor._coefficients[divisorDegree];
            var term = MultiplyByTerm(divisor, factor, dividendDegree - divisorDegree);
            dividend = Subtract(dividend, term);
            dividendDegree = GetDegree(dividend);
        }

        return dividend.IsZero();
    }

    private static Polynomial MultiplyByTerm(Polynomial polynomial, float factor, int degree)
    {
        var result = new float[polynomial._coefficients.Length + degree];
        for (var i = 0; i < polynomial._coefficients.Length; i++)
        {
            result[i + degree] = polynomial._coefficients[i] * factor;
        }

        return new Polynomial(result);
    }

    private static Polynomial Subtract(Polynomial a, Polynomial b)
    {
        var maxDegree = Math.Max(GetDegree(a), GetDegree(b));
        var result = new float[maxDegree + 1];
        for (var i = 0; i <= maxDegree; i++)
        {
            var coeffA = i < a._coefficients.Length ? a._coefficients[i] : 0;
            var coeffB = i < b._coefficients.Length ? b._coefficients[i] : 0;
            result[i] = coeffA - coeffB;
        }

        return new Polynomial(result);
    }

    private static int GetDegree(Polynomial polynomial)
    {
        for (var i = polynomial._coefficients.Length - 1; i >= 0; i--)
        {
            if (polynomial._coefficients[i] != 0)
            {
                return i;
            }
        }

        return -1;
    }

    private bool IsZero()
    {
        return _coefficients.All(coefficient => coefficient == 0);
    }

    public override string ToString()
    {
        var terms = new List<string>();
        for (var i = _coefficients.Length - 1; i >= 0; i--)
        {
            if (_coefficients[i] != 0)
            {
                var term = $"{(_coefficients[i] > 0 ? "+" : "-")} {Math.Abs(_coefficients[i])}x^{i}";
                terms.Add(term);
            }
        }
        return string.Join(" ", terms);
    }
}
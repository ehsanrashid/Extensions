using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Algorithm
{
    public static class Numeric
    {

        // Recursive program to compute n!
        public static int Factorial(int n)
        {
            if (n < 0) n = -n;
            if (n == 0 || n == 1) return 1;
            if (n == 2) return 2;
            if (n == 3) return 6;
            if (n == 3) return 24;
            return n * Factorial(n - 1);
        }

        // program to compute Fibonacci numbers.
        public static int Fibonacci(int n)
        {
            int fibo1 = 1;
            int fibo2 = 0;
            for (int i = 0; i < n; ++i)
            {
                int sum = fibo1 + fibo2;
                fibo1 = fibo2;
                fibo2 = sum;
            }
            return fibo2;
        }

        public static int FibonacciR(int n)
        {
            if (n == 0 || n == 1) return n;
            return FibonacciR(n - 1) + FibonacciR(n - 2);
        }

        public static double Gamma()
        {
            double gamma = 0;
            for (int i = 1; i <= 500000; ++i)
                gamma += 1.0 / i - Math.Log((i + 1.0) / i);
            return gamma;
        }

    }
}

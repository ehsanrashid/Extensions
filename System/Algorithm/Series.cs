
namespace System.Algorithm
{
    public static class Series
    {

        // Program to compute Σ n
        public static int SumSeries(int n)
        {
            int sum = 0;
            for (int i = 1; i <= n; ++i)
                sum += i;
            return sum;
        }

        // Program to compute Σ ax^n using the Horner's rule
        public static int SumSeries(int[] a, int n, int x)
        {
            int sum = a[n];
            for (int i = n - 1; i >= 0; --i)
                sum = sum * x + a[i];
            return sum;
        }


        // Linear search to find Max
        public static int FindMaximum(int[] a)
        {
            int result = a[0];
            for (int i = 1; i < a.Length; ++i)
                if (a[i] > result)
                    result = a[i];
            return result;
        }

        // Linear search to find Min
        public static int FindMinimum(int[] a)
        {
            int result = a[0];
            for (int i = 1; i < a.Length; ++i)
                if (a[i] < result)
                    result = a[i];
            return result;
        }

        // Program to compute Σ x^n
        public static int SumGeometricSeries(int x, int n)
        {
            int sum = 0;
            for (int i = 0; i <= n; ++i)
            {
                int prod = 1;
                for (int j = 0; j < i; ++j)
                    prod *= x;
                sum += prod;
            }
            return sum;
        }

        // Program to compute Σ x^n using the Horner's rule
        public static int SumGeometricSeriesH(int x, int n)
        {
            int sum = 0;
            for (int i = 0; i <= n; ++i)
                sum = sum * x + 1;
            return sum;
        }

        // Program to compute Σ x^n using the closed-form expression
        public static int SumGeometricSeriesC(int x, int n)
        {
            return (Numeric.Power(x, n + 1) - 1) / (x - 1);
        }


        // Program to compute Σ a^i for 0 < i < n.
        public static void PrefixSums(int[] a, int n)
        {
            for (int j = n - 1; j >= 0; --j)
            {
                int sum = 0;
                for (int i = 0; i <= j; ++i)
                    sum += a[i];
                a[j] = sum;
            }
        }


        // Bucket sort
        public static void BucketSort(int[] a, int m)
        {
            int[] buckets = new int[m];

            for (int j = 0; j < m; ++j)
                buckets[j] = 0;
            for (int i = 0; i < a.Length; ++i)
                ++buckets[a[i]];
            for (int i = 0, j = 0; j < m; ++j)
                for (int k = buckets[j]; k > 0; --k)
                    a[i++] = j;
        }



    }
}

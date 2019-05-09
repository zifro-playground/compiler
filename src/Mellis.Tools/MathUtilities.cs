using System;

namespace Mellis.Tools
{
    public static class MathUtilities
    {
        public static int Pow(int x, int exp)
        {
            if (exp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exp));
            }

            if (x == 0 && exp != 0)
            {
                return 0;
            }

            switch (exp)
            {
            case 0: return 1;
            case 1: return x;
            case 2: return checked(x * x);
            case 3: return checked(x * x * x);
            case 4: return checked(x * x * x * x);

            default:
                switch (x)
                {
                case 2:
                case -2 when exp % 2 == 0:
                    if (exp >= 31)
                    {
                        throw new OverflowException();
                    }

                    return 1 << exp;

                case -2:
                    if (exp >= 32)
                    {
                        throw new OverflowException();
                    }

                    // x = -2 && exp is uneven
                    return -1 << exp;

                default:
                    int result = 1;

                    for (int i = 0; i < exp; i++)
                    {
                        checked
                        {
                            result *= x;
                        }
                    }

                    return result;
                }
            }
        }

        public static long Pow(long x, int exp)
        {
            if (exp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(exp));
            }

            if (x == 0 && exp != 0)
            {
                return 0;
            }

            switch (exp)
            {
            case 0: return 1;
            case 1: return x;
            case 2: return checked(x * x);
            case 3: return checked(x * x * x);
            case 4: return checked(x * x * x * x);

            default:
                switch (x)
                {
                case 2:
                case -2 when exp % 2 == 0:
                    if (exp >= 63)
                    {
                        throw new OverflowException();
                    }

                    return 1L << exp;

                case -2:
                    if (exp >= 64)
                    {
                        throw new OverflowException();
                    }

                    // x = -2 && exp is uneven
                    return -1L << exp;

                default:
                    long result = 1;

                    for (long i = 0; i < exp; i++)
                    {
                        checked
                        {
                            result *= x;
                        }
                    }

                    return result;
                }
            }
        }
    }
}
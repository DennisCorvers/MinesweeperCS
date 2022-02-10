using System;
using System.Runtime.CompilerServices;

namespace Minesweeper
{
    public static class MathUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
            {
                ThrowMinMaxException(min, max);
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        private static void ThrowMinMaxException<T>(T min, T max) 
            => throw new ArgumentException($"{min} cannot be greater than {max}.");
    }
}

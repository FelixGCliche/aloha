using System.Collections.Generic;

namespace Game
{
    // Author: Benjamin Lemelin
    public static class ArrayExtensions
    {
        public static IEnumerator<T> Repeat<T>(this T[] array)
        {
            if (array.Length == 0) yield break; // Can't repeat empty array
            var i = 0; // Foreach loop allocates a lot of memory. Using while loop to put less stress on the garbage collector.
            while (true) // Repeat array indefinitely
            {
                yield return array[i];
                i = (i + 1) % array.Length;
            }
        }
    }
}
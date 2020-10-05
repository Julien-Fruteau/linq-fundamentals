using System.Collections.Generic;

namespace Features
{
    public static class MyLinq
    {
        public static int MyCount<T>(this IEnumerable<T> sequence)
        {
            int i = 0;
            foreach (var item in sequence)
            {
                i ++;
            }
            return i;
        }
    }
}
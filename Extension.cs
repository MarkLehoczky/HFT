using System;
using System.Collections.Generic;

namespace Program
{
    public static class Extension
    {
        public static void ConsoleWriteLine<T>(this T value)
        {
            Console.WriteLine("`" + value + "`");
        }

        public static void ConsoleWriteLine<T>(this T value, string text)
        {
            Console.WriteLine(text.Replace("{$}", "`" + value + "`"));
        }
        public static void ConsoleWriteLine<T>(this IEnumerable<T> values)
        {
            foreach (var item in values)
                Console.WriteLine("`{0}`", item);
        }

        public static void ConsoleWriteLine<T>(this IEnumerable<T> values, string text)
        {
            Console.WriteLine(text);
            foreach (var item in values)
                Console.WriteLine(" > `{0}`", item);
            Console.WriteLine();
        }
    }
}
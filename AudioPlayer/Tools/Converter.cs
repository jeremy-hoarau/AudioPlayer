using System;
using System.Collections.Generic;

namespace AudioPlayer.Tools
{
    static class Converter
    {
        public static string ListOfIntToString(List<int> input)
        {
            return string.Join(",", input);
        }

        public static List<int> StringToListOfInt(string input)
        {
            List<int> list = new List<int>();
            if (input == string.Empty)
                return list;
            string[] stringArray = input.Split(',');
            foreach (string s in stringArray)
                list.Add(Int32.Parse(s));
            return list;
        }
    }
}

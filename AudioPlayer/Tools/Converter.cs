using System;
using System.Collections.Generic;

namespace AudioPlayer.Tools
{
    public class Converter
    {
        public string ListOfIntToString(List<int> input)
        {
            return string.Join(",", input);
        }

        public List<int> StringToListOfInt(string input)
        {
            string[] stringArray = input.Split(',');
            List<int> list = new List<int>();
            foreach (string s in stringArray)
                list.Add(Int32.Parse(s));
            return list;
        }
    }
}

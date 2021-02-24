using AudioPlayer.Tools;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test_AudioPlayer
{
    public class T_Converter
    {
        private string stringIDs = "25,56,72,805,2008";
        private List<int> listIDs = new List<int>() {25,56,72,805,2008};

        [Fact]
        public void ListOfIntToString()
        {
            var Result = Converter.ListOfIntToString(listIDs);
            Assert.Equal(stringIDs, Result);
        }
        
        [Fact]
        public void StringToListOfInt()
        {
            var Result = Converter.StringToListOfInt(stringIDs);
            listIDs.Should().BeEquivalentTo(Result);
        }
    }
}

using System.Collections.Generic;
using VRT.Text.Builders;
using Xunit;

namespace VRT.Text.Tests
{
    public class TemplateStringTests
    {
        [Fact()]
        public void ToString_PlaceholderValuesWithRequiredPlaceholder_TemplateWithAppliedPlaceholderValue()
        {
            var sut = CreateSut("{TestToken}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };

            var txtResult=sut.ToString(tokens);                 
            Assert.False(string.IsNullOrWhiteSpace(txtResult));
            Assert.Equal("Test", txtResult);
        }

        [Fact()]
        public void ToString_PlaceholderValuesEmptyDic_EmptyResult()
        {
            var sut = CreateSut("{TestToken}");
            var tokens = new Dictionary<string, string>();            
            var txtResult = sut.ToString(tokens);
            Assert.True(string.IsNullOrWhiteSpace(txtResult));
        }

        [Fact()]
        public void ToString_PlaceholderValuesNull_EmptyResult()
        {
            var sut = CreateSut("{TestToken}");
                       
            var txtResult = sut.ToString(null);
            Assert.True(string.IsNullOrWhiteSpace(txtResult));
        }

        [Fact()]
        public void ToString_DefaultOverloadCall_TemplateText()
        {
            var sut = CreateSut("{TestToken}");
            var txtResult = sut.ToString();
            Assert.False(string.IsNullOrWhiteSpace(txtResult));
            Assert.Equal("{TestToken}", txtResult);
        }

        [Fact()]
        public void ToString_DictionaryWithoutMatchingTokenWithTagNamePreservation_OriginalTemplateText()
        {
            var sut = CreateSut("{TestToken2}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };
            var txtResult = sut.ToString(tokens,true);
            Assert.False(string.IsNullOrWhiteSpace(txtResult));
            Assert.Equal("{TestToken2}", txtResult);
        }


        [Fact()]
        public void ToString_EmptyTokenNameInFormatStringNonemptyTokensDictionary_EmptyResultString()
        {
            var sut = CreateSut("{}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };
            
            var txtResult = sut.ToString(tokens);
            Assert.True(string.IsNullOrEmpty(txtResult),"Text should be null");
        }

        [Fact()]
        public void ToString_NullTemplate_NullResultString()
        {
            var sut = CreateSut(null);
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };
            
            var txtResult = sut.ToString(tokens);
            Assert.True(string.IsNullOrEmpty(txtResult));
        }

        [Fact()]
        public void ToString_TemplateWithTwoOpeningCurlyBraces_TokenNotApplied()
        {
            var sut = CreateSut("{Bonzo{{TestToken}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };
            
            var txtResult = sut.ToString(tokens);
            Assert.Equal("{Bonzo{TestToken}", txtResult);
        }

        [Fact()]
        public void ToString_TemplateWithThreeOpeningCurlyBraces_ReducedBraces()
        {
            var sut = CreateSut("{Bonzo{{{TestToken}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "Test"
            };
                       
            var txtResult = sut.ToString(tokens);
            Assert.Equal("{Bonzo{Test", txtResult);
        }

        [Fact()]
        public void Create_TwoTemplatesWithSameContent_EqualReferences()
        {
            var sut = CreateSut("{Bonzo{{{TestToken}");
            var sut2 = TemplateStringBuilder.Create("{Bonzo{{{TestToken}");
            Assert.True(ReferenceEquals(sut,sut2));
        }

        [Fact()]
        public void Create_ExplicitConvertionToString_EqualReferences()
        {
            var sut = (TemplateStringBuilder)"Bonzo {TestToken}";
            Assert.True(ReferenceEquals("Bonzo {TestToken}", (string) sut));
        }

        [Fact()]
        public void Create_ExplicitConvertionNull_NullAfterConversion()
        {
            var sut = (TemplateStringBuilder)null;
            Assert.Null((string)sut);
        }

        [Fact()]
        public void Create_NestedPlaceholdersMultipleExecution_ProperFinalStringResult()
        {
            var sut = CreateSut("{P{TestToken}S}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "1",
                ["P1S"] = "Final1"
            };
            var txtResult = sut.ToString(tokens);
            var sut2 = CreateSut(txtResult);
            var finalResult = sut2.ToString(tokens);
            Assert.Equal("Final1", finalResult);
        }

        [Fact()]
        public void Create_TemplateTextNull_Null()
        {
            var sut = CreateSut(null);
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "1"                
            };
            var txtResult = sut.ToString(tokens);
            
            Assert.True(null==txtResult, "Result should be null");
        }

        [Fact()]
        public void Create_TemplateTextWithEmptyBlock_Null()
        {
            var sut = CreateSut("{}");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "1"
            };
            var txtResult = sut.ToString(tokens);

            Assert.True(null == txtResult, "Result should be null");
        }

        [Fact()]
        public void Create_TemplateTextWithSingleOpeningCurlyBrace_SingleCurlyBrace()
        {
            var sut = CreateSut("{");
            var tokens = new Dictionary<string, string>()
            {
                ["TestToken"] = "1"
            };
            var txtResult = sut.ToString(tokens);

            Assert.Equal("{", txtResult);
        }


        [Fact()]
        public void Create_TemplateTextWithCharsFromDifferentCultures_ProperlyFormattedOutpuString()
        {
            var templateText = "łłŁŁÓóÓóźłłćŻŻŁŹŁÓ {私は誰} 私は誰 我是誰من";
            var expectedText = "łłŁŁÓóÓóźłłćŻŻŁŹŁÓ 1 私は誰 我是誰من";
            
            var sut = CreateSut(templateText);
            var tokens = new Dictionary<string, string>()
            {
                ["私は誰"] = "1"
            };
            var txtResult = sut.ToString(tokens);

            Assert.Equal(expectedText, txtResult);
        }

        //[Fact()]
        //public void PerformanceTest()
        //{
        //    var sut = CreateSut("Hello {TestToken} {TestToken2} {TestToken}");
        //    var tokens = new Dictionary<string, string>()
        //    {
        //        ["TestToken"] = "1",
        //        ["TestToken2"] = "2"
        //    };
        //    const int numOfIter = 4_000_000;
        //    const string expectedText = "Hello 1 2 1";

        //    Enumerable.Range(0, numOfIter)
        //        .AsParallel()
        //        .ForAll(f =>
        //        {
        //            //var sut = CreateSut("Hello {TestToken} {TestToken2} {TestToken}");
        //            var t = sut.ToString(tokens);
        //            Assert.Equal(expectedText, t);
        //        });
        //    //Parallel.For(0, 3_000_000, a =>
        //    //{
        //    //    var t = sut.ToString(tokens);
        //    //    Assert.Equal("Hello 1", t);
        //    //});
        //    //for (var i = 0; i < numOfIter; i++)
        //    //{
        //    //    var t = sut.ToString(tokens);
        //    //    Assert.Equal(expectedText, t);
        //    //}
        //}

        private TemplateStringBuilder CreateSut(string template)
        {            
            return TemplateStringBuilder.Create(template);
        }
    }
}

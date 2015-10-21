// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace Zanshin.Tests.Domain.Extensions
{
    using System;
    using System.Globalization;
    using System.Text;

    using NUnit.Framework;

    using Zanshin.Domain.Extensions;

    [TestFixture]
    public class StringExtensionsTests
    {
        private const string TestString = "htTP://a.little.com/testString.xml";
        private const string SlashyString = "me\\";
        private const string ExistingFile = "\\ExpectedFile.txt";

        [Test]
        public void EndsWith_Returns_False_If_Passed_An_Empty_Array()
        {
            Assert.AreEqual(false, string.Empty.EndsWith(new string[0], false, CultureInfo.CurrentCulture));
        }

        [Test]
        public void EnsureDotAfter_Adds_Dot_If_String_Does_Not_End_With_Dot()
        {
            string newString = SlashyString.EnsureDotAfter();
            Assert.AreEqual(SlashyString + ".", newString);
        }

        [Test]
        public void EnsureDotAfter_Simply_Returns_Parameter_If_String_Ends_With_Dot()
        {
            string newString = SlashyString + ".";
            Assert.AreEqual(newString, newString.EnsureDotAfter());
        }

        [Test]
        public void EnsureDotBefore_Adds_Dot_If_String_Does_Not_StartWith_With_Dot()
        {
            string newString = SlashyString.EnsureDotBefore();
            Assert.AreEqual("." + SlashyString, newString);
        }

        [Test]
        public void EnsureDotBefore_Simply_Returns_Parameter_If_String_Starts_With_Dot()
        {
            string newString = "." + SlashyString;
            Assert.AreEqual(newString, newString.EnsureDotBefore());
        }

        [Test]
        public void EnsureSlashesAfter_Should_Add_Slashes_If_It_Does_not_End_With_Them()
        {
            Assert.AreNotEqual(TestString, TestString.EnsureSlashesAfter());
        }

        [Test]
        public void EnsureSlashesAfter_Should_Not_Add_Slashes_If_It_Already_Ends_With_Them()
        {
            Assert.AreEqual(SlashyString, SlashyString.EnsureSlashesAfter());
        }

        [Test]
        public void FindPath_Of_Existing_File_Check_Bin_Folders()
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            string expected = AppDomain.CurrentDomain.BaseDirectory + ExistingFile;
            string actual = ExistingFile.FindPath(true);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindPath_Of_Existing_File_Check_Bin_Folders_For_App_Data_Files()
        {
            const string expected = "Test.xml";
            string actual = expected.FindPath(true);
            Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\" + expected, actual);
        }

        [Test]
        public void FromBase64_Correctly_Decodes_Base64_String()
        {
            string sixtyFour = TestString.ToBase64();
            string fromSixtyFour = sixtyFour.FromBase64();

            Assert.AreEqual(fromSixtyFour, TestString);
        }

#pragma warning disable 618

        [Test]
        public void MD5Hash_Should_Create_Same_Hash_For_Same_String()
        {
            string md51 = SlashyString.Md5Hash();
            string md52 = "me\\".Md5Hash();
            Assert.AreEqual(md51, md52);

        }

        [Test]
        public void MD5Hash_Should_Create_Same_Hash_For_String_And_StringBuilder()
        {
            string md51 = SlashyString.Md5Hash();
            StringBuilder md52 = new StringBuilder();
            md52.Append("me\\");

            Assert.AreEqual(md51, md52.Md5Hash());
        }

#pragma warning restore 618

        [Test]
        public void Sha256_Hash_With_Null_Or_StringEmpty_Returns_Null()
        {
            string s = string.Empty.Sha256Hash();
            Assert.AreEqual(string.Empty, s);
        }

        [Test]
        public void Sha256Hash_Should_Create_Same_Hash_For_Same_String()
        {
            string sha2561 = SlashyString.Sha256Hash();
            string sha2562 = "me\\".Sha256Hash();
            Assert.AreEqual(sha2561, sha2562);
        }

        [Test]
        public void StartsWith_Returns_False_If_Passed_Empty_Array()
        {
            Assert.AreEqual(false, TestString.StartsWith(new string[0], true, CultureInfo.InvariantCulture));
        }

        [Test]
        public void StringExtension_EndsWith_Should_Return_True_If_Any_Member_Of_Array_Is_Found()
        {
            bool actual = TestString.EndsWith(new[] { "xxx", "xml" }, false, CultureInfo.InvariantCulture);
            Assert.AreEqual(actual, true);
        }

        [Test]
        public void CapitalizeFirstLetters_Capitalizes_First_Letter_Of_Single_Word_String()
        {
            string sampleString = "testing";

            string actualString = sampleString.CapitalizeFirstLetterOfEachWord();

            Assert.AreEqual("Testing", actualString);
        }

        [Test]
        public void CapitalizeFirstLetters_Capitalizes_First_Letter_Of_Each_LowerCase_Word_In_Multi_Word_String()
        {
            const string sampleString = "test this out";

            string actualString = sampleString.CapitalizeFirstLetterOfEachWord();

            Assert.AreEqual("Test This Out", actualString);
        }

        [Test]
        public void CapitalizeFirstLetters_Capitalizes_First_Letter_Of_Each_UpperCase_Word_In_Multi_Word_String()
        {
            const string sampleString = "TEST THIS OUT";

            string actualString = sampleString.CapitalizeFirstLetterOfEachWord();

            Assert.AreEqual("Test This Out", actualString);
        }

        [Test]
        public void CapitalizeFirstLetters_With_Null_String_Returns_Null()
        {
            string bad = null;
            // ReSharper disable ExpressionIsAlwaysNull
            string s = bad.CapitalizeFirstLetterOfEachWord();
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.IsNull(s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_StringEmpty_Returns_StringEmpty()
        {
            string bad = String.Empty;
            string s = bad.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual(String.Empty, s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_Numeral_Returns_Numeral()
        {
            string numeric = "\0";
            string s = numeric.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual("\0", s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_WhiteSpace_Returns_StringEmpty()
        {
            string whiteSpace = " ";
            string s = whiteSpace.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual(string.Empty, s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_DoubleWhiteSpace_Returns_StringEmpty()
        {
            string whiteSpace = "  ";
            string s = whiteSpace.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual(string.Empty, s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_DoubleWhiteSpace_Followed_By_Numeric_Returns_Numeric()
        {
            string whiteSpace = "  \0\0";
            string s = whiteSpace.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual("\0\0", s);
        }

        [Test]
        public void CapitalizeFirstLetters_With_Trailing_WhiteSpace_Returns_Capital_Letter_Trimmed()
        {
            string whiteSpace = "b ";
            string s = whiteSpace.CapitalizeFirstLetterOfEachWord();
            Assert.AreEqual("B", s);
        }

    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore ExceptionNotDocumented

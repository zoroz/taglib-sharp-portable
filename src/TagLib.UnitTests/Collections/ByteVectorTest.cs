using System;
using Xunit;

namespace TagLib.Tests.Collections
{
    public class ByteVectorTest
    {
        private const string TEST_INPUT = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly ByteVector TestVector = ByteVector.FromString(TEST_INPUT, StringType.UTF8);

        [Fact]
        public void Length()
        {
            Assert.Equal(TEST_INPUT.Length, TestVector.Count);
        }

        [Fact]
        public void StartsWith()
        {
            Assert.True(TestVector.StartsWith("ABCDE"));
            Assert.False(TestVector.StartsWith("NOOP"));
        }

        [Fact]
        public void EndsWith()
        {
            Assert.True(TestVector.EndsWith("UVWXYZ"));
            Assert.False(TestVector.EndsWith("NOOP"));
        }

        [Fact]
        public void ContainsAt()
        {
            Assert.True(TestVector.ContainsAt("JKLMNO", 9));
            Assert.False(TestVector.ContainsAt("NOOP", 30));
        }

        [Fact]
        public void Find()
        {
            Assert.Equal(17, TestVector.Find("RSTUV"));
            Assert.Equal(-1, TestVector.Find("NOOP"));
        }

        [Fact]
        public void RFind()
        {
            Assert.Equal(6, TestVector.RFind("GHIJ"));
            Assert.Equal(-1, TestVector.RFind("NOOP"));
        }

        [Fact]
        public void Mid()
        {
            Assert.Equal(ByteVector.FromString("KLMNOPQRSTUVWXYZ", StringType.UTF8), TestVector.Mid(10));
            Assert.Equal(ByteVector.FromString("PQRSTU", StringType.UTF8), TestVector.Mid(15, 6));
        }

        [Fact]
        public void CopyResize()
        {
            ByteVector a = new ByteVector(TestVector);
            ByteVector b = ByteVector.FromString("ABCDEFGHIJKL", StringType.UTF8);
            a.Resize(12);

            Assert.Equal(b, a);
            Assert.Equal(b.ToString(), a.ToString());
            Assert.False(a.Count == TestVector.Count);
        }

        [Fact]
        public void Int()
        {
            Assert.Equal(Int32.MaxValue, ByteVector.FromInt(Int32.MaxValue).ToInt());
            Assert.Equal(Int32.MinValue, ByteVector.FromInt(Int32.MinValue).ToInt());
            Assert.Equal(0, ByteVector.FromInt(0).ToInt());
            Assert.Equal(30292, ByteVector.FromInt(30292).ToInt());
            Assert.Equal(-30292, ByteVector.FromInt(-30292).ToInt());
            Assert.Equal(-1, ByteVector.FromInt(-1).ToInt());
        }

        [Fact]
        public void UInt()
        {
            Assert.Equal(UInt32.MaxValue, ByteVector.FromUInt(UInt32.MaxValue).ToUInt());
            Assert.Equal(UInt32.MinValue, ByteVector.FromUInt(UInt32.MinValue).ToUInt());
            Assert.Equal((uint)0, ByteVector.FromUInt(0).ToUInt());
            Assert.Equal((uint)30292, ByteVector.FromUInt(30292).ToUInt());
        }

        [Fact]
        public void Long()
        {
            Assert.Equal(UInt64.MaxValue, ByteVector.FromULong(UInt64.MaxValue).ToULong());
            Assert.Equal(UInt64.MinValue, ByteVector.FromULong(UInt64.MinValue).ToULong());
            Assert.Equal((ulong)0, ByteVector.FromULong(0).ToULong());
            Assert.Equal((ulong)30292, ByteVector.FromULong(30292).ToULong());
        }

        [Fact]
        public void Short()
        {
            Assert.Equal(UInt16.MaxValue, ByteVector.FromUShort(UInt16.MaxValue).ToUShort());
            Assert.Equal(UInt16.MinValue, ByteVector.FromUShort(UInt16.MinValue).ToUShort());
            Assert.Equal(0, ByteVector.FromUShort(0).ToUShort());
            Assert.Equal(8009, ByteVector.FromUShort(8009).ToUShort());
        }

        //[Fact]
        //public void FromUri()
        //{
        //    ByteVector vector = ByteVector.FromPath("samples/vector.bin");
        //    Assert.Equal(3282169185, vector.Checksum);
        //    Assert.Equal("1aaa46c484d70c7c80510a5f99e7805d", MD5Hash(vector.Data));
        //}

        [Fact]
        public void OperatorAdd()
        {
            using (new CodeTimer("Operator Add"))
            {
                ByteVector vector = new ByteVector();
                for (int i = 0; i < 10000; i++)
                {
                    vector += ByteVector.FromULong(55);
                }
            }

            using (new CodeTimer("Function Add"))
            {
                ByteVector vector = new ByteVector();
                for (int i = 0; i < 10000; i++)
                {
                    vector.Add(ByteVector.FromULong(55));
                }
            }
        }

        [Fact]
        public void CommentsFrameError()
        {
            // http://bugzilla.gnome.org/show_bug.cgi?id=582735
            // Comments data found in the wild
            ByteVector vector = new ByteVector(
                1, 255, 254, 73, 0, 68, 0, 51, 0, 71, 0, 58, 0, 32, 0, 50, 0, 55, 0, 0, 0);

            var encoding = (StringType)vector[0];
            //var language = vector.ToString (StringType.Latin1, 1, 3);
            var split = vector.ToStrings(encoding, 4, 3);
            Assert.Equal(2, split.Length);
        }

/*
        private static string Md5Hash(byte[] bytes)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            string hashString = String.Empty;

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
*/
    }
}

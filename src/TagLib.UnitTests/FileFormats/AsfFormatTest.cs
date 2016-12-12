using System.Linq;
using Xunit;

namespace TagLib.Tests.FileFormats
{
    public class AsfFormatTest : IFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample.wma";
        private const string TMP_FILE = "samples/tmpwrite.wma";
        private File _file;

        public AsfFormatTest()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE));
        }

        [Fact]
        public void ReadAudioProperties()
        {
            StandardTests.ReadAudioProperties(_file);
        }

        [Fact]
        public void ReadTags()
        {
            Assert.Equal("WMA album", _file.Tag.Album);
            Assert.Equal("Dan Drake", _file.Tag.FirstAlbumArtist);
            Assert.Equal("WMA artist", _file.Tag.FirstPerformer);
            Assert.Equal("WMA comment", _file.Tag.Comment);
            Assert.Equal("Brit Pop", _file.Tag.FirstGenre);
            Assert.Equal("WMA title", _file.Tag.Title);
            Assert.Equal((uint)5, _file.Tag.Track);
            Assert.Equal((uint)2005, _file.Tag.Year);
            Assert.Equal(1, _file.Tag.Pictures.Count());
        }

        [Fact(Skip = "Fix test")]
        public void WriteStandardTags()
        {
            StandardTests.WriteStandardTags (SAMPLE_FILE, TMP_FILE);
        }

        [Fact]
        public void TestCorruptionResistance()
        {
            StandardTests.TestCorruptionResistance ("samples/corrupt/a.wma");
        }
    }
}

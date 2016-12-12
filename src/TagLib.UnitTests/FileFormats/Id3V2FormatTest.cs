using Xunit;

namespace TagLib.Tests.FileFormats
{
    public class Id3V2FormatTest : IFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample_v2_only.mp3";
        private const string CORRUPT_FILE = "samples/corrupt/null_title_v2.mp3";
        private const string TMP_FILE = "samples/tmpwrite_v2_only.mp3";
        private const string EXT_HEADER_FILE = "samples/sample_v2_3_ext_header.mp3";
        private File _file;

        public Id3V2FormatTest()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE));
        }

        [Fact]
        public void ReadAudioProperties()
        {
            Assert.Equal(44100, _file.Properties.AudioSampleRate);
            Assert.Equal(1, _file.Properties.Duration.Seconds);
        }

        [Fact]
        public void TestExtendedHeaderSize()
        {
            // bgo#604488
            var file = File.Create(new LocalFileAbstraction(EXT_HEADER_FILE));
            Assert.Equal("Title v2", file.Tag.Title);
        }

        [Fact] // http://bugzilla.gnome.org/show_bug.cgi?id=558123
        public void TestTruncateOnNull()
        {
            if (System.IO.File.Exists(TMP_FILE))
            {
                System.IO.File.Delete(TMP_FILE);
            }

            System.IO.File.Copy(CORRUPT_FILE, TMP_FILE);
            File tmp = File.Create(new LocalFileAbstraction(TMP_FILE));

            Assert.Equal("T", tmp.Tag.Title);
        }

        [Fact]
        public void ReadTags()
        {
            Assert.Equal("MP3 album", _file.Tag.Album);
            Assert.Equal("MP3 artist", _file.Tag.FirstPerformer);
            Assert.Equal("MP3 comment", _file.Tag.Comment);
            Assert.Equal("Acid Punk", _file.Tag.FirstGenre);
            Assert.Equal("MP3 title", _file.Tag.Title);
            Assert.Equal((uint)6, _file.Tag.Track);
            Assert.Equal((uint)7, _file.Tag.TrackCount);
            Assert.Equal((uint)1234, _file.Tag.Year);
        }

        [Fact]
        public void TestCorruptionResistance()
        {
            
        }
    }
}

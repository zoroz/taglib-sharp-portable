using System;
using TagLib.IFD;
using TagLib.IFD.Entries;
using TagLib.IFD.Tags;
using TagLib.Jpeg;
using TagLib.Xmp;
using Xunit;

namespace TagLib.Tests.FileFormats
{
    public class JpegFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample.jpg";
        private Image.File _file;

        private const TagTypes CONTAINED_TYPES = TagTypes.JpegComment | TagTypes.TiffIFD | TagTypes.XMP;

        public JpegFormatTest()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE)) as Image.File;
        }

        [Fact]
        public void TestJpegRead()
        {
            Assert.True(_file is Jpeg.File);

            Assert.Equal(CONTAINED_TYPES, _file.TagTypes);
            Assert.Equal(CONTAINED_TYPES, _file.TagTypesOnDisk);

            Assert.NotNull(_file.Properties);
            Assert.Equal(7, _file.Properties.PhotoHeight);
            Assert.Equal(10, _file.Properties.PhotoWidth);
            Assert.Equal(90, _file.Properties.PhotoQuality);

            JpegCommentTag tag = _file.GetTag(TagTypes.JpegComment) as JpegCommentTag;
            Assert.False(tag == null);
            Assert.Equal("Test Comment", tag.Value);
        }

        [Fact]
        public void TestExif()
        {
            var tag = _file.GetTag(TagTypes.TiffIFD) as IFDTag;
            Assert.False(tag == null);

            var exifIfd = tag.Structure.GetEntry(0, IFDEntryTag.ExifIFD) as SubIFDEntry;
            Assert.False(exifIfd == null);
            var exifTag = exifIfd.Structure;

            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.ExposureTime) as RationalIFDEntry;
                Assert.False(entry == null);
                Assert.Equal(0.008, entry.Value);
            }
            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.FNumber) as RationalIFDEntry;
                Assert.False(entry == null);
                Assert.Equal(3.2, entry.Value);
            }
            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.ISOSpeedRatings) as ShortIFDEntry;
                Assert.False(entry == null);
                Assert.Equal(100, entry.Value);
            }
        }

        [Fact(Skip = "Fix test")]
        public void TestXmp()
        {
            XmpTag tag = _file.GetTag(TagTypes.XMP) as XmpTag;
            Assert.False(tag == null);

            TestBagNode(tag, XmpTag.DC_NS, "subject", new[] { "keyword1", "keyword2", "keyword3" });
            TestAltNode(tag, XmpTag.DC_NS, "description", new[] { "Sample Image" });
        }

        [Fact(Skip = "Fix test")]
        public void TestConstructor1()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.NotNull(file.ImageTag);
            Assert.Equal(CONTAINED_TYPES, file.TagTypes);

            Assert.NotNull(file.Properties);
        }

        [Fact]
        public void TestConstructor2()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.NotNull(file.ImageTag);
            Assert.Equal(CONTAINED_TYPES, file.TagTypes);

            Assert.Null(file.Properties);
        }

        [Fact]
        public void TestConstructor3()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.NotNull(file.ImageTag);
            Assert.Equal(CONTAINED_TYPES, file.TagTypes);

            Assert.Null(file.Properties);
        }

        private void TestBagNode(XmpTag tag, string ns, string name, string[] values)
        {
            var node = tag.FindNode(ns, name);
            Assert.False(node == null);
            Assert.Equal(XmpNodeType.Bag, node.Type);
            Assert.Equal(values.Length, node.Children.Count);

            int i = 0;
            foreach (var childNode in node.Children)
            {
                Assert.Equal(values[i], childNode.Value);
                Assert.Equal(0, childNode.Children.Count);
                i++;
            }
        }

        private void TestAltNode(XmpTag tag, string ns, string name, string[] values)
        {
            var node = tag.FindNode(ns, name);
            Assert.False(node == null);
            Assert.Equal(XmpNodeType.Alt, node.Type);
            Assert.Equal(values.Length, node.Children.Count);

            int i = 0;
            foreach (var childNode in node.Children)
            {
                Assert.Equal(values[i], childNode.Value);
                Assert.Equal(0, childNode.Children.Count);
                i++;
            }
        }
    }
}

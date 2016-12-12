using System;
using Xunit;

namespace TagLib.Tests.FileFormats
{
    public static class StandardTests
    {
        public static void ReadAudioProperties (File file)
        {
            Assert.Equal(44100, file.Properties.AudioSampleRate);
            Assert.Equal(5, file.Properties.Duration.Seconds);
        }
        
        public static void WriteStandardTags (string sampleFile, string tmpFile)
        {
            if (System.IO.File.Exists (tmpFile))
                System.IO.File.Delete (tmpFile);

            System.IO.File.Copy(sampleFile, tmpFile);
                
            File tmp = File.Create (new LocalFileAbstraction(tmpFile, true));
            SetTags (tmp.Tag);
            tmp.Save ();
                
            tmp = File.Create (new LocalFileAbstraction(tmpFile));
            CheckTags (tmp.Tag);
        }

        private static void SetTags (Tag tag)
        {
            tag.Album = "TEST album";
            tag.AlbumArtists = new[] {"TEST artist 1", "TEST artist 2"};
            tag.BeatsPerMinute = 120;
            tag.Comment = "TEST comment";
            tag.Composers = new[] {"TEST composer 1", "TEST composer 2"};
            tag.Conductor = "TEST conductor";
            tag.Copyright = "TEST copyright";
            tag.Disc = 100;
            tag.DiscCount = 101;
            tag.Genres = new[] {"TEST genre 1", "TEST genre 2"};
            tag.Grouping = "TEST grouping";
            tag.Lyrics = "TEST lyrics 1\r\nTEST lyrics 2";
            tag.Performers = new[] {"TEST performer 1", "TEST performer 2"};
            tag.Title = "TEST title";
            tag.Track = 98;
            tag.TrackCount = 99;
            tag.Year = 1999;
        }

        private static void CheckTags (Tag tag)
        {
            Assert.Equal ("TEST album", tag.Album);
            Assert.Equal ("TEST artist 1; TEST artist 2", tag.JoinedAlbumArtists);
            Assert.Equal (120, (int)tag.BeatsPerMinute);
            Assert.Equal ("TEST comment", tag.Comment);
            Assert.Equal ("TEST composer 1; TEST composer 2", tag.JoinedComposers);
            Assert.Equal ("TEST conductor", tag.Conductor);
            Assert.Equal ("TEST copyright", tag.Copyright);
            Assert.Equal (100, (int)tag.Disc);
            Assert.Equal (101, (int)tag.DiscCount);
            Assert.Equal ("TEST genre 1; TEST genre 2", tag.JoinedGenres);
            Assert.Equal ("TEST grouping", tag.Grouping);
            Assert.Equal ("TEST lyrics 1\r\nTEST lyrics 2", tag.Lyrics);
            Assert.Equal ("TEST performer 1; TEST performer 2", tag.JoinedPerformers);
            Assert.Equal ("TEST title", tag.Title);
            Assert.Equal (98, (int)tag.Track);
            Assert.Equal (99, (int)tag.TrackCount);
            Assert.Equal (1999, (int)tag.Year);
        }
        
        public static void TestCorruptionResistance (string path)
        {
            try
            {
                File.Create(new LocalFileAbstraction(path));
            }
            catch (CorruptFileException)
            {
            }
            catch (NullReferenceException e)
            {
                throw e;
            }
            catch
            {
            }
        }
    }
}

using System;
using Xunit;

namespace TagLib.Tests.Performance
{
    public class FileTest
    {
        [Fact]
        public void CreateM4A()
        {
            try
            {
                double totalTime = 0.0;
                const int iterations = 1000;
                using (new CodeTimer("Combined"))
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        CodeTimer timer = new CodeTimer();
                        using (timer)
                        {
                            File.Create(new LocalFileAbstraction("samples/sample.m4a"));
                        }
                        totalTime += timer.ElapsedTime.TotalSeconds;
                    }
                }
                Console.WriteLine("Average time: {0}", totalTime / iterations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Fact]
        public void CreateOgg()
        {
            try
            {
                double totalTime = 0.0;
                const int iterations = 1000;
                using (new CodeTimer("Combined"))
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        CodeTimer timer = new CodeTimer();
                        using (timer)
                        {
                            File.Create(new LocalFileAbstraction("samples/sample.ogg"));
                        }
                        totalTime += timer.ElapsedTime.TotalSeconds;
                    }
                }
                Console.WriteLine("Average time: {0}", totalTime / iterations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

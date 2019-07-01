using System;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Memory;

namespace BenchmarkSandbox
{
    static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleBenchmark>();
        }
    }

    [MemoryDiagnoser]
    public class SimpleBenchmark
    {
        public SimpleBenchmark()
        {
            // generate raw output
            using (var stream = File.OpenRead("Baseline.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                File.WriteAllBytes("Raw", MemoryMarshal.Cast<Rgba32,byte>(image.GetPixelSpan()).ToArray());
            }

            using (var stream = File.OpenRead("Pixelmator-trim.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                File.WriteAllBytes("Raw-trim", MemoryMarshal.Cast<Rgba32,byte>(image.GetPixelSpan()).ToArray());
            }
        }

        [Benchmark]
        public void Baseline()
        {
            using (var stream = File.OpenRead("Baseline.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void Imagemagick()
        {
            using (var stream = File.OpenRead("ImageMagick.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void ImageOptim()
        {
            using (var stream = File.OpenRead("ImageOptim.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedPixelmator()
        {
            using (var stream = File.OpenRead("Pixelmator-trim.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedImagemagick()
        {
            using (var stream = File.OpenRead("ImageMagick-trim.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedImageOptim()
        {
            using (var stream = File.OpenRead("ImageOptim-trim.png"))
            using (var image = Image.Load<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void Raw()
        {
            using (var memory = Configuration.Default.MemoryAllocator.Allocate<Rgba32>(1024 * 1024))
            using (var stream = File.OpenRead("Raw"))
            {
                stream.Read(MemoryMarshal.Cast<Rgba32, byte>(memory.Memory.Span));

                using (var image = Image.WrapMemory(memory, 1024, 1024))
                {
                    image.GetPixelSpan();
                }
            }
        }

        [Benchmark]
        public void TrimmedRaw()
        {
            using (var memory = Configuration.Default.MemoryAllocator.Allocate<Rgba32>(1010 * 62))
            using (var stream = File.OpenRead("Raw-trim"))
            {
                stream.Read(MemoryMarshal.Cast<Rgba32, byte>(memory.Memory.Span));

                using (var image = Image.WrapMemory(memory, 1010, 62))
                {
                    image.GetPixelSpan();
                }
            }
        }
    }
}
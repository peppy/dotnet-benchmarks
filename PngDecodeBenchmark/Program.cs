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
        [DllImport("stbi", EntryPoint = "stbiLoad")]
        unsafe public static extern bool stbiLoad(byte* dst, byte* data, long len);

        [DllImport("stbi", EntryPoint = "stbiInfo")]
        unsafe public static extern bool stbiInfo(byte* data, long len, out int width, out int height, out int nChannels);

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleBenchmark>();
        }
    }

    [MemoryDiagnoser]
    public class SimpleBenchmark
    {
        unsafe static Image<TPixel> ImageFromStream<TPixel>(Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            long pos = stream.Position;

            using (var m = new MemoryStream())
            {
                stream.CopyTo(m);
                fixed (byte* data = m.GetBuffer())
                {
                    if (Program.stbiInfo(data, m.Length, out int width, out int height, out int nChannels))
                    {
                        var image = new Image<TPixel>(SixLabors.ImageSharp.Configuration.Default, width, height);
                        fixed (TPixel* dst = image.GetPixelSpan())
                            if (Program.stbiLoad((byte*)dst, data, m.Length))
                                return image;
                    }
                }

                stream.Seek(pos, SeekOrigin.Begin);
                return Image.Load<TPixel>(stream);
            }
        }

        public SimpleBenchmark()
        {
            // generate raw output
            using (var stream = File.OpenRead("Baseline.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                File.WriteAllBytes("Raw", MemoryMarshal.Cast<Rgba32,byte>(image.GetPixelSpan()).ToArray());
            }

            using (var stream = File.OpenRead("Pixelmator-trim.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                File.WriteAllBytes("Raw-trim", MemoryMarshal.Cast<Rgba32,byte>(image.GetPixelSpan()).ToArray());
            }
        }

        [Benchmark]
        public void Baseline()
        {
            using (var stream = File.OpenRead("Baseline.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void Imagemagick()
        {
            using (var stream = File.OpenRead("ImageMagick.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void ImageOptim()
        {
            using (var stream = File.OpenRead("ImageOptim.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedPixelmator()
        {
            using (var stream = File.OpenRead("Pixelmator-trim.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedImagemagick()
        {
            using (var stream = File.OpenRead("ImageMagick-trim.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
            {
                image.GetPixelSpan();
            }
        }

        [Benchmark]
        public void TrimmedImageOptim()
        {
            using (var stream = File.OpenRead("ImageOptim-trim.png"))
            using (var image = ImageFromStream<Rgba32>(stream))
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
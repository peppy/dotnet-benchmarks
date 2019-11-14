```
dotnet run -c Release
```


```ini
BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.5 (18F132) [Darwin 18.6.0]
Intel Xeon CPU E5-1620 v2 3.70GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.300
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
```

|      Encode Method  |        Mean |      Error |     StdDev |  Width | Height | Size (on disk)  |
|-------------------- |------------:|-----------:|-----------:|-------:|-------:|------:|
|          ImageOptim | 12,600.3 us |  48.633 us |  40.611 us | 1024px | 1024px |   9kb |
| ImageMagick convert |  6,669.7 us | 119.754 us | 147.069 us | 1024px | 1024px |  14kb |
|            Baseline |  3,416.0 us |  68.152 us | 106.105 us | 1024px | 1024px |  37kb |
|  Trimmed ImageOptim |    899.2 us |   2.917 us |   2.436 us | 1010px |   62px |   9kb |
|  Trimmed Pixelmator |    841.2 us |   4.108 us |   3.642 us | 1010px |   62px |  18kb |
| Trimmed Imagemagick |    664.0 us |   5.053 us |   4.727 us | 1010px |   62px |  11kb |
|                 Raw |    497.1 us |   9.460 us |   8.849 us | 1024px | 1024px |4194kb |
|          TrimmedRaw |     71.7 us |   1.525 us |   1.983 us | 1010px |   62px | 250kb |

- Baseline is direct output from bmfont generator (https://www.angelcode.com/products/bmfont/)
- Raw is raw RGBA32 pixel stream


STBI Results
============

|      Encode Method  |        Single-thread |        Multi-thread |
|-------------------- |------------:|------------:|
|          ImageOptim | 2271 us |  889 us |
| ImageMagick convert | 1185 us |  759 us |
|            Baseline | 2877 us | 1238 us |
|  Trimmed ImageOptim |  241 us |   55 us |
|  Trimmed Pixelmator |  579 us |  108 us |
| Trimmed Imagemagick |  311 us |   56 us |

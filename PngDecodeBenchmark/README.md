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


ImageSharp
==========

|      Encode Method  |        Mean |      Error |     StdDev |  Width | Height | Size (on disk)  |
|-------------------- |------------:|-----------:|-----------:|-------:|-------:|------:|
|            Baseline |  3,416.0 us |  68.152 us | 106.105 us | 1024px | 1024px |  37kb |
|          ImageOptim | 12,600.3 us |  48.633 us |  40.611 us | 1024px | 1024px |   9kb |
| ImageMagick convert |  6,669.7 us | 119.754 us | 147.069 us | 1024px | 1024px |  14kb |
|  Trimmed ImageOptim |    899.2 us |   2.917 us |   2.436 us | 1010px |   62px |   9kb |
|  Trimmed Pixelmator |    841.2 us |   4.108 us |   3.642 us | 1010px |   62px |  18kb |
| Trimmed Imagemagick |    664.0 us |   5.053 us |   4.727 us | 1010px |   62px |  11kb |
|                 Raw |    497.1 us |   9.460 us |   8.849 us | 1024px | 1024px |4194kb |
|          TrimmedRaw |     71.7 us |   1.525 us |   1.983 us | 1010px |   62px | 250kb |


STBI via C# interop into custom C++ library
===========================================

|      Encode Method |        Mean |       Error |      StdDev |      Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------- |------------:|------------:|------------:|------------:|-------:|------:|------:|----------:|
|           Baseline | 2,582.12 us |  58.4809 us | 169.6636 us | 2,653.92 us | 3.9063 |     - |     - |   38.3 KB |
|        Imagemagick | 3,118.35 us | 165.0725 us | 486.7200 us | 2,938.76 us |      - |     - |     - |  15.32 KB |
|         ImageOptim | 1,960.35 us |  49.7322 us |  77.4270 us | 1,934.13 us |      - |     - |     - |  10.92 KB |
|  TrimmedPixelmator |   521.36 us |  10.4985 us |   9.8203 us |   520.44 us | 2.9297 |     - |     - |     20 KB |
| TrimmedImagemagick |   324.95 us |   3.4722 us |   3.2479 us |   323.94 us | 1.9531 |     - |     - |  12.57 KB |
|  TrimmedImageOptim |   219.49 us |   1.9698 us |   1.8425 us |   219.69 us | 1.7090 |     - |     - |  11.02 KB |
|                Raw |   277.76 us |   4.9396 us |   4.1248 us |   277.13 us |      - |     - |     - |   1.94 KB |
|         TrimmedRaw |    43.93 us |   0.6170 us |   0.5772 us |    43.86 us | 0.3052 |     - |     - |   1.96 KB |

- Baseline is direct output from bmfont generator (https://www.angelcode.com/products/bmfont/)
- Raw is raw RGBA32 pixel stream


STBI via C++
====================

|      Encode Method  |        Single-thread |        Multi-thread |
|-------------------- |------------:|------------:|
|          ImageOptim | 2271 us |  889 us |
| ImageMagick convert | 1185 us |  759 us |
|            Baseline | 2877 us | 1238 us |
|  Trimmed ImageOptim |  241 us |   55 us |
|  Trimmed Pixelmator |  579 us |  108 us |
| Trimmed Imagemagick |  311 us |   56 us |

#include <tinylogger/tinylogger.h>

#define STB_IMAGE_IMPLEMENTATION
#include <stb_image.h>

#include <chrono>


void dummyLoadPng(std::string filename) {
    int width, height, nChannels;
    unsigned char* data = stbi_load(filename.c_str(), &width, &height, &nChannels, 0);

    stbi_image_free(data);
}

void performBenchmark(std::string filename) {
    tlog::info() << "Warming up with image " << filename;

    static const int nWarmupSteps = 100;
    auto progress = tlog::progress(nWarmupSteps);
    for (int i = 0; i < nWarmupSteps; ++i) {
        dummyLoadPng(filename);
        progress.update(i);
    }

    tlog::info() << "Benchmarking " << filename;

    static const int nBenchmarkSteps = 1000;
    progress = tlog::progress(nBenchmarkSteps);
    for (int i = 0; i < nBenchmarkSteps; ++i) {
        dummyLoadPng(filename);
        progress.update(i);
    }

    tlog::success() << "Benchmark of " << filename << " completed with " << (std::chrono::microseconds{progress.duration()}.count() / nBenchmarkSteps) << " microseconds per load.";
}

int main(int argc, char** argv) {
    if (argc <= 1) {
        tlog::error() << "Usage: stbbench IMAGE";
        return 1;
    }

    for (int i = 1; i < argc; ++i) {
        performBenchmark(argv[i]);
    }
}

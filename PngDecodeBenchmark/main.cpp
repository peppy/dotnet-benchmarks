#include <tinylogger/tinylogger.h>

#define STB_IMAGE_IMPLEMENTATION
#include <stb_image.h>

#include <atomic>
#include <chrono>
#include <mutex>

#include "ThreadPool.h"


void dummyLoadPng(std::string filename) {
    int width, height, nChannels;
    unsigned char* data = stbi_load(filename.c_str(), &width, &height, &nChannels, 0);

    stbi_image_free(data);
}

void performBenchmark(std::string filename) {
    ThreadPool pool;

    tlog::info() << "Warming up with image " << filename;

    static const int nWarmupSteps = 100;

    std::atomic<int> counter;
    std::mutex mutex;

    counter = 0;
    auto progress = tlog::progress(nWarmupSteps);
    pool.parallelFor(0, nWarmupSteps, [&](int id) {
        dummyLoadPng(filename);

        std::lock_guard<std::mutex> lock(mutex);
        progress.update(++counter);
    });

    tlog::info() << "Benchmarking " << filename;

    static const int nBenchmarkSteps = 4000;

    counter = 0;
    progress = tlog::progress(nBenchmarkSteps);
    pool.parallelFor(0, nBenchmarkSteps, [&](int id) {
        dummyLoadPng(filename);

        std::lock_guard<std::mutex> lock(mutex);
        progress.update(++counter);
    });

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



cmake_minimum_required(VERSION 3.0.0)

project(stbbench)

if (MSVC)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /D_CRT_SECURE_NO_WARNINGS")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /MP")
endif()

set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++14")

add_executable(stbbench
    main.cpp
)

target_include_directories(stbbench PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}/dependencies/stb
    ${CMAKE_CURRENT_SOURCE_DIR}/dependencies/tinylogger
)

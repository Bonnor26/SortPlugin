#include "SortPlugin.h"
#include <algorithm>

extern "C"
{
    __declspec(dllexport) void TestSort(int a[], int length)
    {
        std::sort(a, a + length);
    }
}
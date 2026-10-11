using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SortTimer : MonoBehaviour
{
    // Method 1: C++ native plugin
    [DllImport("SortPlugin")]
    private static extern void TestSort(int[] a, int length);

    void Start()
    {
        int[] original = LoadNumbers();
        if (original == null)
        {
            return;
        }

        Debug.Log($"Loaded {original.Length} numbers ({original.Distinct().Count()} unique).");

        // Method 1: C++ native plugin
        int[] a1 = (int[])original.Clone();
        Stopwatch sw1 = Stopwatch.StartNew();
        TestSort(a1, a1.Length);
        sw1.Stop();
        Debug.Log($"Method 1 (C++ native plugin) took {sw1.Elapsed.TotalMilliseconds:F3} ms.");

        // Method 2: C# managed plugin
        int[] a2 = (int[])original.Clone();
        Stopwatch sw2 = Stopwatch.StartNew();
        ManagedSortPlugin.Sorter.TestSort(a2);
        sw2.Stop();
        Debug.Log($"Method 2 (C# managed plugin) took {sw2.Elapsed.TotalMilliseconds:F3} ms.");

        // Method 3: C# class inside Unity
        int[] a3 = (int[])original.Clone();
        Stopwatch sw3 = Stopwatch.StartNew();
        SortInUnity(a3);
        sw3.Stop();
        Debug.Log($"Method 3 (MonoBehaviour class) took {sw3.Elapsed.TotalMilliseconds:F3} ms.");
    }

    private void SortInUnity(int[] a)
    {
        Array.Sort(a);
    }

    private int[] LoadNumbers()
    {
        TextAsset file = Resources.Load<TextAsset>("numbers");
        if (file == null)
        {
            Debug.LogError("Could not find numbers.txt in Assets/Resources.");
            return null;
        }

        return file.text
            .Split(new[] { '\r', '\n', ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }
}
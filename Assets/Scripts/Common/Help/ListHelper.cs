using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

/// <summary>
/// Функции дял работы с List.
/// </summary>
public static class ListHelper
{
    public static T GetLast<T>(this IList<T> list)
    {
        T res = list[list.Count - 1];
        return res;
    }
    public static T GetAndRemoveLast<T>(this IList<T> list)
    {
        T res = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return res;
    }
    /// <summary>
    /// Объединяет несколько массивов в один, последовательно.
    /// </summary>
    public static T[] ConcatArrays<T>(params T[][] arrays)
    {
        int totalLength = 0;
        for (int i = 0; i < arrays.Length; i++)
        {
            totalLength += arrays[i].Length;
        }
        int currentIndex = 0;
        T[] result = new T[totalLength];
        for (int i = 0; i < arrays.Length; i++)
        {
            arrays[i].CopyTo(result, currentIndex);
            currentIndex += arrays[i].Length;
        }
        return result;
    }
    /// <summary>
    /// Выбирает случайный элемент из нескольких списков. Возвращает элемент и ссылку на список, из которого он выбран.
    /// </summary>
    public static (T, IList<T>) SelectRandomFromLists<T>(params IList<T> [] lists)
    {
        int totalCount = 0;
        foreach (var list in lists) totalCount += list.Count;
        int current = 0;
        int selection = Random.Range(0, totalCount);
        while (selection >= lists[current].Count)
        {
            selection -= lists[current].Count;
            current++;
        }
        return (lists[current][selection], lists[current]);
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static (int, T) GetMax<T>(this T[] values) where T : IComparable
    {
        T max = values[0];
        int maxIndex = 0;

        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].CompareTo(max) > 0)
            {
                max = values[i];
                maxIndex = i;
            }
        }

        return (maxIndex, max);
    }
}
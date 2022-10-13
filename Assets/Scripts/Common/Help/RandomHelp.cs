using System.Collections.Generic;
using UnityEngine;

public static class RandomHelp
{
    /// <summary>
    /// Возвращает 1 или -1.
    /// </summary>
    public static int Sign => Random.Range(0, 2) == 0 ? -1 : 1;
    /// <summary>
    /// Возвращает массив из length количества элементов, в котором перемешаны числа от 0 до length(не включительно).
    /// </summary>
    public static int[] Range(int length)
    {
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = i;
        }
        for (int i = 0; i < length; i++)
        {
            int tmp = result[i];
            int swapIndex = Random.Range(0, length);
            result[i] = result[swapIndex];
            result[swapIndex] = tmp;
        }
        return result;
    }
    /// <summary>
    /// Перемешивает элементы в списке, модифиируя его.
    /// </summary>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int swapIndex = Random.Range(0, list.Count);
            T tmp = list[i];
            list[i] = list[swapIndex];
            list[swapIndex] = tmp;
        }
        return list;
    }
}
using UnityEngine;
using System.Collections.Generic;

public static class MeshHelper
{
    /// <summary>
    /// Объединяет несколько мешей в один.
    /// </summary>
    public static Mesh CombineMeshes(params Mesh[] meshes)
    {
        Vector3[][] verticesArrays = new Vector3[meshes.Length][];
        Vector2[][] uvArrays = new Vector2[meshes.Length][];
        int[][] triangles = new int[meshes.Length][];

        for (int i = 0; i < meshes.Length; i++)
        {
            verticesArrays[i] = meshes[i].vertices;
            uvArrays[i] = meshes[i].uv;
            triangles[i] = meshes[i].triangles;
        }

        int shift = 0;
        for (int i = 1; i < meshes.Length; i++)
        {
            shift += verticesArrays[i - 1].Length;
            for (int j = 0; j < triangles[i].Length; j++)
            {
                triangles[i][j] += shift;
            }
        }

        Mesh result = new Mesh();
        result.vertices = ListHelper.ConcatArrays(verticesArrays);
        result.uv = ListHelper.ConcatArrays(uvArrays);
        result.triangles = ListHelper.ConcatArrays(triangles);
        result.RecalculateNormals();
        result.RecalculateTangents();
        return result;
    }
}
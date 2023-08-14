using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static void Plane(MeshFilter meshFilter, float length, float width, int resolutionX, int resolutionZ)
    {
        // You can change that line to provide another MeshFilter
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        // create vertices (position)
        Vector3[] vertices = new Vector3[resolutionX * resolutionZ];
        for (int z = 0; z < resolutionZ; z++)
        {
            // [ -length / 2, length / 2 ]
            float zPos = ((float)z / (resolutionZ - 1) - .5f) * length;
            for (int x = 0; x < resolutionX; x++)
            {
                // [ -width / 2, width / 2 ]
                float xPos = ((float)x / (resolutionX - 1) - .5f) * width;
                vertices[x + z * resolutionX] = new Vector3(xPos, 0f, zPos);
            }
        }

        // create normals
        Vector3[] normals = new Vector3[vertices.Length];
        Array.ForEach(normals, normal => normal = Vector3.up) ;

        // create tangents
        Vector4[] tangents = new Vector4[vertices.Length];
        Array.ForEach(tangents, normal => normal = new Vector4(1, 0, 0, -1));

        // create uvs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int v = 0; v < resolutionZ; v++)
        {
            for (int u = 0; u < resolutionX; u++)
            {
                uvs[u + v * resolutionX] = new Vector2((float)u / (resolutionX - 1), (float)v / (resolutionZ - 1));
            }
        }

        // create triangles
        int nbFaces = (resolutionX - 1) * (resolutionZ - 1);
        int[] triangles = new int[nbFaces * 6];
        int t = 0;
        for (int face = 0; face < nbFaces; face++)
        {
            // Retrieve lower left corner from face index
            int i = face % (resolutionX - 1) + (face / (resolutionZ - 1) * resolutionX);

            triangles[t++] = i + resolutionX;
            triangles[t++] = i + 1;
            triangles[t++] = i;

            triangles[t++] = i + resolutionX;
            triangles[t++] = i + resolutionX + 1;
            triangles[t++] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.tangents = tangents;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}

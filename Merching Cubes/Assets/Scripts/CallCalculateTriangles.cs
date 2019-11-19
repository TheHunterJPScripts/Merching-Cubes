using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scene1;

static public class CallCalculateTriangles
{
    public struct Triangle {
#pragma warning disable 649 // disable unassigned variable warning
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this[int i] {
            get {
                switch (i) {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

    const int threadGroupSize = 8;
    static public Triangle[] Calculate(ComputeShader shader, Vector4[] points, int count, float isoLevel) {
        ComputeBuffer triangleBuffer = new ComputeBuffer((count-1)*5, sizeof(float) * 3 * 3);
        ComputeBuffer pointsBuffer = new ComputeBuffer(points.Length, sizeof(float)*4);
        ComputeBuffer triCountBuffer = new ComputeBuffer(1, sizeof(int));

        shader.SetBuffer(0, "points", pointsBuffer);
        shader.SetBuffer(0, "triangles", triangleBuffer);
        shader.SetInt("numPointsPerAxis", count);
        shader.SetFloat("isoLevel", isoLevel);

        int numThreadsPerAxis = Mathf.CeilToInt ((count-1) / (float) threadGroupSize);
        shader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        triangleBuffer.GetData(tris,0,0, numTris);

        triangleBuffer.Release();
        pointsBuffer.Release();
        triCountBuffer.Release();

        return tris;
    }
}

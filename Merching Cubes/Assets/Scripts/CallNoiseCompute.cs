using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CallNoiseCompute
{
    static public Vector4[] ComputeNoise(ComputeShader shader, Vector4[] input, Vector3 center) {
        ComputeBuffer buffer = new ComputeBuffer(input.Length, 16);
        buffer.SetData(input);
        int kernel = shader.FindKernel("Noise");
        shader.SetVector("centerPos", center);
        shader.SetBuffer(kernel, "points", buffer);
        shader.Dispatch(kernel, input.Length, 1, 1);
        buffer.GetData(input);
        buffer.Release();
        return input;
    }
}

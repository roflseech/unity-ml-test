using System;
using Unity.Barracuda;
using UnityEngine;

public class ImageRecognizer : IDisposable
{
    private NNModel _model;
    private IWorker _worker;

    private Tensor _input;
    private Tensor _output;

    private int _width;
    private int _height;
    private int _classCount;

    public ImageRecognizer(NNModel model, int width, int height, int classCount)
    {
        _model = model;
        _worker = _model.CreateWorker(WorkerFactory.Device.CPU);
        _width = width;
        _height = height;
        _classCount = classCount;
    }
    public float[] GetProbabilities(VirtualTexture virtualTexture)
    {
        float[] converted = new float[28 * 28];

        var pixels = virtualTexture.Pixels;

        for (int i = 0; i < pixels.Length; i++)
        {
            int row = i / 28;
            int column = i - row * 28;
            converted[(27 - row) * 28 + column] = ColorToFloat(pixels[i]);
        }

        _input = new Tensor(1, _width, _height, 1);
        _input.data.Upload(converted, new TensorShape(1, _width, _height, 1));

        _output = _worker.Execute(_input).PeekOutput();

        return _output.data.Download(new TensorShape(1, 1, 1, _classCount));
    }
    public void Dispose()
    {
        _worker.Dispose();
        _input?.Dispose();
        _output?.Dispose();
    }
    private float ColorToFloat(Color color)
    {
        return (color.r + color.g + color.b) / 3.0f;
    }
    /*private float[] GetClass(float[] array)
    {
        int max = 0;
        float maxValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > maxValue)
            {
                max = i;
                maxValue = array[i];
            }
        }

        return ("Class: " + max);
    }*/
}
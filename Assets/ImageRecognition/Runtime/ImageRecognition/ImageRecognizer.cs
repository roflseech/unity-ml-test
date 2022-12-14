using System;
using Unity.Barracuda;
using UnityEngine;

namespace ImageRecognition
{
    public class ImageRecognizer : IDisposable
    {
        private NNModel _model;
        private IWorker _worker;

        private Tensor _input;

        private int _width;
        private int _height;
        private int _classCount;

        public ImageRecognizer(NNModel model, int width, int height, int classCount)
        {
            _model = model;
            _worker = _model.CreateWorker(WorkerFactory.Device.CPU);
            _input = new Tensor(1, width, height, 1);
            _width = width;
            _height = height;
            _classCount = classCount;
        }
        public PredictionEntry Predict(VirtualTexture virtualTexture)
        {
            var probabilities = GetProbabilities(virtualTexture);

            var max = GetMax(probabilities);

            return new PredictionEntry(max.Item1, max.Item2);
        }
        public PredictionEntry[] PredictProbabilities(VirtualTexture virtualTexture)
        {
            var probabilities = GetProbabilities(virtualTexture);

            PredictionEntry[] predictions = new PredictionEntry[probabilities.Length];

            for (int i = 0; i < probabilities.Length; i++)
            {
                predictions[i] = new PredictionEntry(i, probabilities[i]);
            }

            return predictions;
        }
        public void Dispose()
        {
            _worker.Dispose();
            _input?.Dispose();
        }
        private float[] GetProbabilities(VirtualTexture virtualTexture)
        {
            if (virtualTexture.Width != _width ||
                virtualTexture.Height != _height)
            {
                throw new Exception($"Wrong image size! Required: {_width}x{_height}. " +
                    $"Given: {virtualTexture.Width}x{virtualTexture.Height}");
            }

            float[] converted = new float[_width * _height];

            var pixels = virtualTexture.Pixels;

            for (int i = 0; i < pixels.Length; i++)
            {
                int row = i / _width;
                int column = i - row * _width;
                converted[(_height - 1 - row) * _width + column] = ColorToFloat(pixels[i]);
            }

            _input.data.Upload(converted, new TensorShape(1, _width, _height, 1));

            var output = _worker.Execute(_input).PeekOutput();

            var data = output.data.Download(new TensorShape(1, 1, 1, _classCount));

            output.Dispose();

            return data;
        }

        private float ColorToFloat(Color color)
        {
            return (color.r + color.g + color.b) / 3.0f;
        }
        private (int, float) GetMax(float[] values)
        {
            float max = values[0];
            int maxIndex = 0;

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxIndex = i;
                }
            }

            return (maxIndex, max);
        }
    }
}
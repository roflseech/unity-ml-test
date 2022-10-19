using System;
using System.Collections.Generic;

namespace ImageRecognition
{
    public class ImageRecognitionModel : IDisposable
    {
        private ImageRecognizer _recognizer;
        private ModelClassList _modelClasses;

        public ImageRecognitionModel(ImageRecognizer recognizer, ModelClassList classList)
        {
            _recognizer = recognizer;
            _modelClasses = classList;
        }
        public int PredictClassId(VirtualTexture texture)
        {
            return _recognizer.Predict(texture).ClassId;
        }
        public string PredictClassName(VirtualTexture texture)
        {
            var prediction = _recognizer.Predict(texture);
            return _modelClasses.GetClassName(prediction.ClassId);
        }
        public PredictionEntry[] PredictProbabilities(VirtualTexture texture)
        {
            return _recognizer.PredictProbabilities(texture);
        }
        public NamedPredictionEntry[] PredictProbabilitiesNamed(VirtualTexture texture)
        {
            var predictions = _recognizer.PredictProbabilities(texture);
            return _modelClasses.ConvertPredictionEntries(predictions);
        }
        public void Dispose()
        {
            _recognizer.Dispose();
        }
    }
}



namespace ImageRecognition
{
    public struct NamedPredictionEntry
    {
        public readonly string ClassName;
        public readonly float Probability;

        public NamedPredictionEntry(string className, float probability)
        {
            ClassName = className;
            Probability = probability;
        }
    }
}

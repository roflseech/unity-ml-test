

namespace ImageRecognition
{
    public struct PredictionEntry
    {
        public readonly int ClassId;
        public readonly float Probability;
        public PredictionEntry(int classId, float probability)
        {
            ClassId = classId;
            Probability = probability;
        }
    }
}

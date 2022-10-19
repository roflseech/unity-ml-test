using Unity.Barracuda;
using UnityEngine;


namespace ImageRecognition
{
    [CreateAssetMenu(fileName = "ModelDefinition", menuName = "Image Recognition/Model Definition")]
    public class ImageRecognitionModelDefinition : ScriptableObject
    {
        [SerializeField]
        private NNModel _model;
        [SerializeField]
        private TextAsset _classList;
        [SerializeField]
        private int _imageSize;

        public NNModel Model => _model;
        public TextAsset ClassList => _classList;
        public int ImageSize => _imageSize;

        public ImageRecognitionModel CreateModel()
        {
            var classList = CreateClassList();
            var recognizer = CreateRecognizer(classList.Items.Count);

            return new ImageRecognitionModel(recognizer, classList);
        }
        public ImageRecognizer CreateRecognizer(int classCount)
        {
            return new ImageRecognizer(_model, _imageSize, _imageSize, classCount);
        }
        public ModelClassList CreateClassList()
        {
            return ModelClassList.FromJSON(_classList);
        }
    }
}

using UnityEditor;

namespace ImageRecognition
{
    [CustomEditor(typeof(ImageRecognitionModelDefinition))]
    public class ImageRecognitionModelDefinitionInspector : Editor
    {
        private ImageRecognitionModelDefinition _modelDefinition;

        private SerializedProperty _modelProperty;
        private SerializedProperty _classListProperty;
        private SerializedProperty _imageSizeProperty;

        private ModelClassList _classList;
        private bool _expandedClassList;
        private void OnEnable()
        {
            _modelDefinition = (ImageRecognitionModelDefinition)target;

            _modelProperty = serializedObject.FindProperty("_model");
            _classListProperty = serializedObject.FindProperty("_classList");
            _imageSizeProperty = serializedObject.FindProperty("_imageSize");

            if (_modelDefinition.ClassList != null)
            {
                _classList = _modelDefinition.CreateClassList();
            }
            else
            {
                _classList = null;
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modelProperty);
            EditorGUILayout.PropertyField(_classListProperty);
            EditorGUILayout.PropertyField(_imageSizeProperty);

            EditorGUILayout.Space();

            _expandedClassList = EditorGUILayout.Foldout(_expandedClassList, "Class list");

            if (_classList != null && _expandedClassList)
            {
                EditorGUI.indentLevel += 1;
                foreach (var className in _classList.Items)
                {
                    EditorGUILayout.LabelField(className);
                }
                EditorGUI.indentLevel -= 1;
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace ImageRecognition
{
    [CustomEditor(typeof(ImageRecognitionModelDefinition))]
    public class ImageRecognitionModelDefinitionInspector : Editor
    {
        private ImageRecognitionModelDefinition _modelDefinition;

        private SerializedProperty _modelProperty;
        private SerializedProperty _classListProperty;
        private SerializedProperty _imageSizeProperty;

        private TextAsset _lastClassListAsset;
        private ModelClassList _classList;
        private bool _expandedClassList;
        private void OnEnable()
        {
            _modelDefinition = (ImageRecognitionModelDefinition)target;

            _modelProperty = serializedObject.FindProperty("_model");
            _classListProperty = serializedObject.FindProperty("_classList");
            _imageSizeProperty = serializedObject.FindProperty("_imageSize");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UpdateClassListIfNeeded();

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
            serializedObject.ApplyModifiedProperties();
        }
        private void UpdateClassListIfNeeded()
        {
            if (_lastClassListAsset != _modelDefinition.ClassList)
            {
                _lastClassListAsset = _modelDefinition.ClassList;

                if (_lastClassListAsset != null)
                {
                    _classList = _modelDefinition.CreateClassList();
                }
                else
                {
                    _classList = null;
                }
            }
        }
    }
}

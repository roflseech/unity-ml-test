using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using UnityEngine;

namespace ImageRecognition.Samples
{
    public class DrawingGuesser : MonoBehaviour
    {
        [Header("Model")]
        [SerializeField]
        private NNModel _model;
        [SerializeField]
        private TextAsset _classNames;

        [Header("Scene Objects")]
        [SerializeField]
        private VirtualTextureManager _textureManager;

        [SerializeField]
        private SpriteRenderer _targetCanvasSprite;

        [SerializeField]
        private TMP_Text _text;

        [Header("Parameters")]
        [SerializeField]
        private int _padding = 40;
        [SerializeField]
        private int _targetImageSize = 28;

        private ImageRecognizer _imageRecognizer;
        private ClassList _classList;

        [System.Serializable]
        private class ClassList
        {
            public List<string> Items;
            public ClassList()
            {
                Items = new();
            }
        }

        private void Start()
        {
            _classList = JsonUtility.FromJson<ClassList>(_classNames.text);

            _imageRecognizer = new ImageRecognizer(_model, 28, 28, 5);
            var texture = new Texture2D(512, 512);

            _targetCanvasSprite.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, 512, 512),
                Vector2.one * 0.5f,
                512);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            if (Input.GetMouseButtonUp(0))
            {
                var processed = _textureManager.VirtualTexture
                    .FilledSubset(new Color(0, 0, 0, 0), _padding)
                    .Resize(_targetImageSize, _targetImageSize)
                    .TransparentBlackToBlackWhite();

                int w = processed.Width;
                int h = processed.Height;

                var texture = new Texture2D(w, h);

                _targetCanvasSprite.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, w, h),
                    Vector2.one * 0.5f,
                    w);

                var max = GetMax(_imageRecognizer.GetProbabilities(processed));
                _text.text = _classList.Items[max.Item1];

                _targetCanvasSprite.sprite.texture.SetPixels(processed.Pixels);
                _targetCanvasSprite.sprite.texture.Apply();
            }
        }
        private void OnDestroy()
        {
            _imageRecognizer.Dispose();
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
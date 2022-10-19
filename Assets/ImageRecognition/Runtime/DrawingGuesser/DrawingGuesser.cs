using TMPro;
using UnityEngine;

namespace ImageRecognition.Samples
{
    public class DrawingGuesser : MonoBehaviour
    {
        [Header("Model")]
        [SerializeField]
        private ImageRecognitionModelDefinition _modelDefinition;

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

        private ImageRecognitionModel _model;

        private void Start()
        {
            _model = _modelDefinition.CreateModel();

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


                _text.text = _model.PredictClassName(processed);

                _targetCanvasSprite.sprite.texture.SetPixels(processed.Pixels);
                _targetCanvasSprite.sprite.texture.Apply();
            }
        }
        private void OnDestroy()
        {
            _model.Dispose();
        }
    }
}
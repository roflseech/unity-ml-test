using UnityEngine;

namespace ImageRecognition
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class VirtualTextureManager : MonoBehaviour
    {
        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;
        [SerializeField]
        private Color _background;

        private SpriteRenderer _renderer;

        private VirtualTexture _virtualTexture;

        public VirtualTexture VirtualTexture => _virtualTexture;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            var texture = new Texture2D(_width, _height);

            _renderer.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, _width, _height),
                Vector2.one * 0.5f,
                Mathf.Max(_width, _height));

            _virtualTexture = new VirtualTexture(_width, _height, _background);
            UpdateTexture();
        }

        public void UpdateTexture()
        {
            _renderer.sprite.texture.SetPixels(_virtualTexture.Pixels, 0);
            _renderer.sprite.texture.Apply();
        }

        public void DrawAt(int x, int y, int width, Color color)
        {
            _virtualTexture.DrawAt(x, y, width, color);
        }

        public void RelativeDrawAt(float x, float y, int width, Color color)
        {
            x += 0.5f;
            y += 0.5f;

            int pixelX = (int)(x * _virtualTexture.Width);
            int pixelY = (int)(y * _virtualTexture.Height);

            DrawAt(pixelX, pixelY, width, color);
        }
        public void DrawLine(Vector2 start, Vector2 end, int width, Color color)
        {
            int startX = Mathf.RoundToInt(start.x);
            int startY = Mathf.RoundToInt(start.y);

            int endX = Mathf.RoundToInt(end.x);
            int endY = Mathf.RoundToInt(end.y);

            _virtualTexture.DrawLine(startX, startY, endX, endY, width, color);
        }
        public void RelativeDrawLine(Vector2 start, Vector2 end, int width, Color color)
        {
            start += Vector2.one * 0.5f;
            end += Vector2.one * 0.5f;

            int startX = Mathf.RoundToInt(start.x * _virtualTexture.Width);
            int startY = Mathf.RoundToInt(start.y * _virtualTexture.Height);

            int endX = Mathf.RoundToInt(end.x * _virtualTexture.Width);
            int endY = Mathf.RoundToInt(end.y * _virtualTexture.Height);

            _virtualTexture.DrawLine(startX, startY, endX, endY, width, color);
        }
        public void Fill(Color color)
        {
            _virtualTexture.FillColor(color);
        }
    }
}
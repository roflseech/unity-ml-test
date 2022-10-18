using UnityEngine;

namespace ImageRecognition
{
    public class VirtualTexture
    {
        private Color[] _pixels;
        private int _width;

        public Color[] Pixels => _pixels;
        public int Width => _width;
        public int Height => _pixels.Length / _width;
        public VirtualTexture(int width, int height)
        {
            _pixels = new Color[width * height];
            _width = width;
        }
        public VirtualTexture(int width, int height, Color baseColor)
        {
            _pixels = new Color[width * height];
            _width = width;

            for (int i = 0; i < _pixels.Length; i++)
            {
                _pixels[i] = baseColor;
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            _pixels[x + y * Width] = color;
        }

        public void SetMaxOpaquePixel(int x, int y, Color color)
        {
            if (_pixels[x + y * Width].a < color.a)
            {
                _pixels[x + y * Width] = color;
            }
        }
        public Color GetPixel(int x, int y)
        {
            return _pixels[x + y * Width];
        }
        public void DrawAt(int x, int y, Color color)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) return;

            SetPixel(x, y, color);
        }
        public void DrawAt(int x, int y, int width, Color color)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) return;

            int startX = Mathf.Max(0, x - width - 1);
            int endX = Mathf.Min(Width - 1, x + width + 1);

            int startY = Mathf.Max(0, y - width - 1);
            int endY = Mathf.Min(Height - 1, y + width + 1);

            Vector2 a = new Vector2();
            Vector2 b = new Vector2();
            for (int curX = startX; curX <= endX; curX++)
            {
                for (int curY = startY; curY <= endY; curY++)
                {
                    a.x = x;
                    a.y = y;
                    b.x = curX;
                    b.y = curY;
                    float dist = Vector2.Distance(a, b);

                    if (dist < width) SetPixel(curX, curY, color);
                }
            }

            SetPixel(x, y, color);
        }

        public void DrawLine(int startX, int startY, int endX, int endY, int width, Color color)
        {
            int dx = Mathf.Abs(endX - startX);
            int sx = startX < endX ? 1 : -1;
            int dy = -Mathf.Abs(endY - startY);
            int sy = startY < endY ? 1 : -1;
            int error = dx + dy;

            while (true)
            {
                DrawAt(startX, startY, width, color);
                if (startX == endX && startY == endY) break;
                int e2 = 2 * error;
                if (e2 >= dy)
                {
                    if (startX == endX) break;
                    error = error + dy;
                    startX = startX + sx;
                }

                if (e2 <= dx)
                {
                    if (startY == endY) break;
                    error = error + dx;
                    startY = startY + sy;
                }
            }
        }
        /// <summary>
        /// 0.0f <= x, y <= 1.0f
        /// </summary>
        public void RelativeDrawAt(float x, float y, int width, Color color)
        {
            int pixelX = (int)(x * Width);
            int pixelY = (int)(y * Height);

            DrawAt(pixelX, pixelY, width, color);
        }
        /// <summary>
        /// 0.0f <= start, end <= 1.0f
        /// </summary>
        public void RelativeDrawLine(Vector2 start, Vector2 end, int width, Color color)
        {
            int startX = Mathf.RoundToInt(start.x * Width);
            int startY = Mathf.RoundToInt(start.y * Height);

            int endX = Mathf.RoundToInt(end.x * Width);
            int endY = Mathf.RoundToInt(end.y * Height);

            DrawLine(startX, startY, endX, endY, width, color);
        }
        public void FillColor(Color color)
        {
            for (int i = 0; i < _pixels.Length; i++)
            {
                _pixels[i] = color;
            }
        }
    }
}
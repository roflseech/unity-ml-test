using System.Text;
using TMPro;
using Unity.Barracuda;
using UnityEngine;

public class DrawingSpawner : MonoBehaviour
{
    [SerializeField]
    private NNModel _model;

    [SerializeField]
    private VirtualTextureManager _textureManager;

    [SerializeField]
    private SpriteRenderer _targetCanvasSprite;

    [SerializeField]
    private TMP_Text _text;

    private ImageRecognizer _imageRecognizer;
    private void Start()
    {
        _imageRecognizer = new ImageRecognizer(_model, 28, 28, 10);
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
            var subset = _textureManager.VirtualTexture.FilledSubset(new Color(0, 0, 0, 0), 40);

            subset = subset.Resize(28, 28);
            subset = subset.TransparentBlackToBlackWhite();

            int w = subset.Width;
            int h = subset.Height;
            
            var texture = new Texture2D(w, h);

            _targetCanvasSprite.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, w, h),
                Vector2.one * 0.5f,
                w);

            var max = _imageRecognizer.GetProbabilities(subset).GetMax();
            _text.text = max.Item1.ToString();

            _targetCanvasSprite.sprite.texture.SetPixels(subset.Pixels);
            _targetCanvasSprite.sprite.texture.Apply();
        }
    }
    private void OnDestroy()
    {
        _imageRecognizer.Dispose();
    }
    private void PrintFloatArrray(float[] array, int itemsPerRow = 100000)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < array.Length; i++)
        {
            if (i % itemsPerRow == 0) sb.Append("\n");
            sb.Append(array[i].ToString("0.00") + " ");
        }

        Debug.Log(sb.ToString());
    }
}
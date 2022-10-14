using UnityEngine;

[RequireComponent(typeof(VirtualTextureManager))]
public class TextureDrawingController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [Range(1, 1000)]
    [SerializeField]
    private int _width = 1;

    [SerializeField]
    private Color _color = Color.black;

    private VirtualTextureManager _textureDrawing;

    public Camera Camera { get => _camera; set => _camera = value; }
    private Vector2 _prevPos;
    private bool _prevTouch;
    private void Start()
    {
        _textureDrawing = GetComponent<VirtualTextureManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _textureDrawing.VirtualTexture.FillColor(Color.clear);
            _textureDrawing.UpdateTexture();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var worldMousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            var localMousePos = transform.worldToLocalMatrix.MultiplyPoint(worldMousePos);

            if (_prevTouch)
            {
                _textureDrawing.RelativeDrawLine(_prevPos, localMousePos, _width, _color);
            }
            else
            {
                _textureDrawing.RelativeDrawAt(localMousePos.x, localMousePos.y, _width, _color);
                _prevTouch = true;
            }

            _prevPos = localMousePos;
            _textureDrawing.UpdateTexture();
        }
        else
        {
            _prevTouch = false;
        }
    }
}
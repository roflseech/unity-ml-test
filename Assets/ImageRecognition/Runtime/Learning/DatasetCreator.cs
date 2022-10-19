using ImageRecognition;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ImageRecognition.Samples
{
    public class DatasetCreator : MonoBehaviour
    {
        [SerializeField]
        private string _folderName;
        [SerializeField]
        private VirtualTextureManager _textureManager;
        [SerializeField]
        private SpriteRenderer _targetCanvasSprite;

        int currentFile;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            if (Input.GetMouseButtonUp(0))
            {
                SaveFile();
                ClearCanvas();
            }
        }
        private void SaveFile()
        {
            Texture2D newTexture = new Texture2D(28, 28);
            var convertedVirtualTexture = _textureManager.VirtualTexture.FilledSubset(new Color(0, 0, 0, 0), 40);
            convertedVirtualTexture = convertedVirtualTexture.Resize(28, 28);
            convertedVirtualTexture = convertedVirtualTexture.TransparentBlackToBlackWhite();

            newTexture.SetPixels(convertedVirtualTexture.Pixels);
            newTexture.Apply();

            var bytes = newTexture.EncodeToPNG();
            var dirPath = Application.dataPath + "/" +
                _folderName + "/";

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllBytes(dirPath +
                currentFile.ToString() + ".png", bytes);

            Debug.Log("File " + currentFile.ToString() + " is saved, " + bytes.Length / 1024 + "Kb");

            currentFile += 1;
        }
        private void ClearCanvas()
        {
            _textureManager.Fill(new Color(0, 0, 0, 0));
            _textureManager.UpdateTexture();
        }
    }
}
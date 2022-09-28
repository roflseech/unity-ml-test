using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Barracuda;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    [SerializeField]
    private NNModel _model;

    [SerializeField]
    private List<Sprite> _data = new();

    private IWorker _worker;
   
    void Start()
    {
        //var model = ModelLoader.Load("Models/sequential_1");
        //var engine = WorkerFactory.CreateWorker(model, WorkerFactory.Device.CPU);
        _worker = _model.CreateWorker(WorkerFactory.Device.CPU);

        ProcessData();
        _worker.Dispose();
        /*var input = new Tensor(1, 28, 28, 1);

        var output = engine.Execute(input).PeekOutput();

        PrintFloatArrray(output.data.Download(new TensorShape(1, 1, 1, 10)));

        input.Dispose();
        output.Dispose();*/
    }

    private void ProcessData()
    {
        foreach (var sprite in _data)
        {
            ProcessSprite(sprite);
        }
    }
    private void ProcessSprite(Sprite sprite)
    {
        var pixels = sprite.texture.GetPixels();

        float[] converted = new float[28 * 28];

        for (int i = 0; i < pixels.Length; i++)
        {
            int row = i / 28;
            int column = i - row * 28;
            converted[(27 - row) * 28 + column] = ColorToFloat(pixels[i]);
        }

        var input = new Tensor(1, 28, 28, 1);
        input.data.Upload(converted, new TensorShape(1, 28, 28, 1));

        var output = _worker.Execute(input).PeekOutput();

        var spriteName = sprite.name;
        var className = GetClass(output.data.Download(new TensorShape(1, 1, 1, 10)));

        Debug.Log(spriteName + " " + className);
        input.Dispose();
        output.Dispose();
    }
    private float ColorToFloat(Color color)
    {
        return (color.r + color.g + color.b) / 3.0f;
    }
    private string GetClass(float[] array)
    {
        int max = 0;
        float maxValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > maxValue)
            {
                max = i;
                maxValue = array[i];
            }
        }

        return ("Class: " + max);
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

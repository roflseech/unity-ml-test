using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ImageRecognition
{
    [Serializable]
    public class ModelClassList
    {
        public List<string> Items;

        public string GetClassName(int classId)
        {
            if (classId < 0 || classId >= Items.Count)
            {
                throw new Exception($"Class id {classId} out of range [${0};{Items.Count - 1}]");
            }

            return Items[classId];
        }
        public string GetClassName(PredictionEntry entry)
        {
            return GetClassName(entry.ClassId);
        }
        public NamedPredictionEntry[] ConvertPredictionEntries(PredictionEntry[] entries)
        {
            NamedPredictionEntry[] result = new NamedPredictionEntry[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                result[i] = new NamedPredictionEntry(GetClassName(entries[i].ClassId), entries[i].Probability);
            }

            return result;
        }
        public static ModelClassList FromJSON(TextAsset jsonList)
        {
            return JsonUtility.FromJson<ModelClassList>(jsonList.text);
        }
    }
}

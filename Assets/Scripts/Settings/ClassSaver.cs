using System.Runtime.Serialization;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ClassSaver<T> where T : class, new()
{
    private T data;
    private readonly string filename;
    private readonly string filePath;

    public T Data
    {
        get => data;
        set
        {
            data = value;
        }
    }

    public ClassSaver(string filename)
    {
        this.filename = filename;
        this.filePath = Path.Combine(Application.persistentDataPath, $"{filename}.json");
        this.data = new T();
        LoadFromFile();
    }

    public void LoadFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<T>(jsonContent);
                Debug.Log($"[ClassSaver] Successfully loaded {typeof(T).Name} from {filename}.json");
            }
            else
            {
                Debug.Log($"[ClassSaver] No saved file found for {typeof(T).Name}. Using default values.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ClassSaver] Error loading {typeof(T).Name}: {e.Message}");
            data = new T();
        }
    }

    public void SaveToFile()
    {
        try
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
            Debug.Log($"[ClassSaver] Successfully saved {typeof(T).Name} to {filename}.json");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ClassSaver] Error saving {typeof(T).Name}: {e.Message}");
        }
    }

    public void DeleteFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"[ClassSaver] Successfully deleted {filename}.json");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ClassSaver] Error deleting {filename}.json: {e.Message}");
        }
    }
}

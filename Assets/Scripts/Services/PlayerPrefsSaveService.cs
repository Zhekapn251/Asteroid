using Services;
using UnityEngine;

public class PlayerPrefsSaveService: ISaveService
{
    public void Save<T>(string key, T data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public T Load<T>(string key, T defaultValue)
    {
        string jsonData = PlayerPrefs.GetString(key, JsonUtility.ToJson(defaultValue));
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
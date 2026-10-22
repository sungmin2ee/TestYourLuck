using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using System.Net;
[Serializable]
public class SaveData
{
    public string Name;
    public int coins;
    public int currentlevel;
    public int highscore;// many more

}
public class DataSaver : MonoBehaviour
{
    public SaveData saveData;
    public string userid;

    DatabaseReference database;

    private void Awake()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void Start()
    {
        saveData.Name = "ali";
        saveData.coins = 12;
        saveData.currentlevel = 3;
        saveData.highscore = 300;
    }
    public void SaveDataFn()
    {
        string json = JsonUtility.ToJson(saveData);
        database.Child("users").Child(userid).SetRawJsonValueAsync(json);
    }
    public void LoadDataFn()
    {
        StartCoroutine(LoadDataEnum());
    }

    IEnumerator LoadDataEnum()
    {
        var serverData = database.Child("users").Child(userid).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("process is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (jsonData != null)
        {
            print("server data found");

            saveData = JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            print("no data found");
        }

    }
}
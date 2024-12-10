using Firebase.Database;
using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;

[Serializable]
public class DataToSave
{
    public string username;
    public int age;
    public int snakeL1Score;
    public int snakeL2Score;
    public int snakeL3Score;
    public int snakeL4Score;
    public int snakeL5Score;
    public int snakeL1ErrorCount;
    public int snakeL2ErrorCount;
    public int snakeL3ErrorCount;
    public int snakeL4ErrorCount;
    public int snakeL5ErrorCount;
    public int sbqL1Coins;
    public int sbqL2Coins;
    public int sbqL3Coins;
    public int sbqL4Coins;
    public int sbqL5Coins;
}
public class DataSaver : MonoBehaviour
{
    public DataToSave dts;
    public string userId;
    DatabaseReference databaseReference;

    private static DataSaver instance;

    private void Awake()
    {
        // Singleton pattern to ensure a single instance of AudioController
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        userId = References.userID;

        SaveData();
    }

    public void SaveData()
    {
        AssignValues();
        string json = JsonUtility.ToJson(dts);
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        Debug.Log("Data Saved");
    }

    public void LoadData()
    {
        StartCoroutine(LoadDataEnum());
    }

    IEnumerator LoadDataEnum()
    {
        var serverData = databaseReference.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(() => serverData.IsCompleted);
        DataSnapshot snapshot = serverData.Result;
        string json = snapshot.GetRawJsonValue();
        if(json != null)
        {
            dts = JsonUtility.FromJson<DataToSave>(json);
        }
        else
        {
            Debug.Log("No data found");
        }
    }

    public void AssignValues()
    {
        dts.username = References.userName;
        dts.age = PlayerPrefs.GetInt("Age");
        dts.snakeL1Score = PlayerPrefs.GetInt("PlayerScore Lv1");
        dts.snakeL2Score = PlayerPrefs.GetInt("PlayerScore Lv2");
        dts.snakeL3Score = PlayerPrefs.GetInt("PlayerScore Lv3");
        dts.snakeL4Score = PlayerPrefs.GetInt("PlayerScore Lv4");
        dts.snakeL1ErrorCount = PlayerPrefs.GetInt("Error Count Lv 1");
        dts.snakeL2ErrorCount = PlayerPrefs.GetInt("Error Count Lv 2");
        dts.snakeL3ErrorCount = PlayerPrefs.GetInt("Error Count Lv 3");
        dts.snakeL4ErrorCount = PlayerPrefs.GetInt("Error Count Lv 4");
        dts.snakeL5ErrorCount = PlayerPrefs.GetInt("Error Count Lv 5");
        dts.sbqL1Coins = PlayerPrefs.GetInt("SBQ Lv1 Coins");
        dts.sbqL2Coins = PlayerPrefs.GetInt("SBQ Lv2 Coins");
        dts.sbqL3Coins = PlayerPrefs.GetInt("SBQ Lv3 Coins");
        dts.sbqL4Coins = PlayerPrefs.GetInt("SBQ Lv4 Coins");
        dts.sbqL5Coins = PlayerPrefs.GetInt("SBQ Lv5 Coins");
        Debug.Log("Data Assigned");
    }

}

using Firebase.Database;
using System;
using System.Collections;
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
    private DatabaseReference databaseReference;

    private static DataSaver instance;

    private void Awake()
    {
        // Singleton pattern to ensure a single instance of DataSaver
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        userId = References.userID;

        LoadData();
        //Invoke("SaveData", 3f);
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

    private IEnumerator LoadDataEnum()
    {
        Debug.Log("Starting data load...");

        // Request data from Firebase
        var serverData = databaseReference.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(() => serverData.IsCompleted);

        if (serverData.IsFaulted)
        {
            Debug.LogError("Error loading data from Firebase: " + serverData.Exception);
            yield break;
        }

        if (!serverData.IsCompleted)
        {
            Debug.LogError("Data load not completed successfully.");
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;

        if (snapshot.Exists)
        {
            string json = snapshot.GetRawJsonValue();
            Debug.Log("Data loaded from Firebase: " + json);

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    dts = JsonUtility.FromJson<DataToSave>(json);
                    Debug.Log("Data deserialized successfully.");

                    // Save data to PlayerPrefs
                    SaveToPlayerPrefs();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogWarning("No data found in Firebase.");
            }
        }
        else
        {
            Debug.LogWarning("Snapshot does not exist for user ID: " + userId);
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
        dts.snakeL5Score = PlayerPrefs.GetInt("PlayerScore Lv5");
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

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString("username", dts.username);
        PlayerPrefs.SetInt("Age", dts.age);
        PlayerPrefs.SetInt("PlayerScore Lv1", dts.snakeL1Score);
        PlayerPrefs.SetInt("PlayerScore Lv2", dts.snakeL2Score);
        PlayerPrefs.SetInt("PlayerScore Lv3", dts.snakeL3Score);
        PlayerPrefs.SetInt("PlayerScore Lv4", dts.snakeL4Score);
        PlayerPrefs.SetInt("PlayerScore Lv5", dts.snakeL5Score);
        PlayerPrefs.SetInt("Error Count Lv 1", dts.snakeL1ErrorCount);
        PlayerPrefs.SetInt("Error Count Lv 2", dts.snakeL2ErrorCount);
        PlayerPrefs.SetInt("Error Count Lv 3", dts.snakeL3ErrorCount);
        PlayerPrefs.SetInt("Error Count Lv 4", dts.snakeL4ErrorCount);
        PlayerPrefs.SetInt("Error Count Lv 5", dts.snakeL5ErrorCount);
        PlayerPrefs.SetInt("SBQ Lv1 Coins", dts.sbqL1Coins);
        PlayerPrefs.SetInt("SBQ Lv2 Coins", dts.sbqL2Coins);
        PlayerPrefs.SetInt("SBQ Lv3 Coins", dts.sbqL3Coins);
        PlayerPrefs.SetInt("SBQ Lv4 Coins", dts.sbqL4Coins);
        PlayerPrefs.SetInt("SBQ Lv5 Coins", dts.sbqL5Coins);
        PlayerPrefs.Save();
        Debug.Log("Data saved to PlayerPrefs.");
    }
}

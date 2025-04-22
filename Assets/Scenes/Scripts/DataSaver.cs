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
    public int sbqL1score;
    public int sbqL2score;
    public int sbqL3score;
    public int sbqL4score;
    public int sbqL5score;
    public float RnCT1Score;
    public float RnCT2Score;
    public float RnCT3Score;
    public float RnCT4Score;
    public float RnCT5Score;
    public float RnCT6Score;
    public float RnCT7Score;
    public int PSL1Score;
    public int PSL2Score;
    public int PSL3Score;
    public int PSErrL1;
    public int PSErrL2;
    public int PSErrL3;
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

        SaveData();
        LoadData();
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
        dts.sbqL1score = PlayerPrefs.GetInt("Score 1");
        dts.sbqL2score = PlayerPrefs.GetInt("Score 2");
        dts.sbqL3score = PlayerPrefs.GetInt("Score 3");
        dts.sbqL4score = PlayerPrefs.GetInt("Score 4");
        dts.sbqL5score = PlayerPrefs.GetInt("Score 5");
        dts.RnCT1Score = PlayerPrefs.GetFloat("Task1_Accuracy");
        dts.RnCT2Score = PlayerPrefs.GetFloat("Task2_Accuracy");
        dts.RnCT3Score = PlayerPrefs.GetFloat("Task3_Accuracy");
        dts.RnCT4Score = PlayerPrefs.GetFloat("Task4_Accuracy");
        dts.RnCT5Score = PlayerPrefs.GetFloat("Task5_Accuracy");
        dts.RnCT6Score = PlayerPrefs.GetFloat("Task6_Accuracy");
        dts.RnCT7Score = PlayerPrefs.GetFloat("Task7_Accuracy");
        dts.PSL1Score = PlayerPrefs.GetInt("PS L1");
        dts.PSL2Score = PlayerPrefs.GetInt("PS L2");
        dts.PSL3Score = PlayerPrefs.GetInt("PS L3");
        dts.PSErrL1 = PlayerPrefs.GetInt("PS Err L1");
        dts.PSErrL2 = PlayerPrefs.GetInt("PS Err L2");
        dts.PSErrL3 = PlayerPrefs.GetInt("PS Err L3");


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
        PlayerPrefs.SetInt("SBQ Score 1", dts.sbqL1score);
        PlayerPrefs.SetInt("SBQ Score 2", dts.sbqL2score);
        PlayerPrefs.SetInt("SBQ Score 3", dts.sbqL3score);
        PlayerPrefs.SetInt("SBQ Score 4", dts.sbqL4score);
        PlayerPrefs.SetInt("SBQ Score 5", dts.sbqL5score);
        PlayerPrefs.SetFloat("RnC Task 1", dts.RnCT1Score);
        PlayerPrefs.SetFloat("RnC Task 2", dts.RnCT2Score);
        PlayerPrefs.SetFloat("RnC Task 3", dts.RnCT3Score);
        PlayerPrefs.SetFloat("RnC Task 4", dts.RnCT4Score);
        PlayerPrefs.SetFloat("RnC Task 5", dts.RnCT5Score);
        PlayerPrefs.SetFloat("RnC Task 6", dts.RnCT6Score);
        PlayerPrefs.SetFloat("RnC Task 7", dts.RnCT7Score);
        PlayerPrefs.SetInt("PS L1 Score", dts.PSL1Score);
        PlayerPrefs.SetInt("PS L2 Score", dts.PSL2Score);
        PlayerPrefs.SetInt("PS L3 Score", dts.PSL3Score);
        PlayerPrefs.SetInt("PS L1 Err", dts.PSErrL1);
        PlayerPrefs.SetInt("PS L2 Err", dts.PSErrL2);
        PlayerPrefs.SetInt("PS L3 Err", dts.PSErrL3);

        PlayerPrefs.Save();
        Debug.Log("Data saved to PlayerPrefs.");
    }
}

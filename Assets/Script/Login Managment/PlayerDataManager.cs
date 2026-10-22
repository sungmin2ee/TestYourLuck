using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private DatabaseReference dbReference;
    private FirebaseAuth auth;

    public TextMeshProUGUI GoldText;

    private int gold = 1000;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (auth.CurrentUser != null)
        {
            LoadGold();
        }
        else
        {
            GoldText.text = ""; 
        }

    }

    public void LoadGold()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning(" No user logged in.");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        Debug.Log($" Loading gold for user {uid}");

        dbReference.Child("users").Child(uid).Child("gold").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.Value != null)
                {
                    gold = int.Parse(snapshot.Value.ToString());
                    
                    Debug.Log($" Gold loaded successfully: {gold}");
                }
                else
                {
                    Debug.LogWarning(" No gold data found. Setting default value.");
                    
                    SaveGold(); 
                }
                UpdateGoldUI();
            }
            else
            {
                Debug.LogError($" Failed to load gold: {task.Exception}");
            }
        });
    }


    public void SaveGold()
    {
        Debug.Log("Save Called.");
        
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("No user logged in.");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        dbReference.Child("users").Child(uid).Child("gold").SetValueAsync(gold).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Gold saved: " + gold);
                
            }
            else
            {
                Debug.LogError("Failed to save gold: " + task.Exception);
                
            }
        });
        Debug.Log("Save End.");
       
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
        SaveGold();
    }

    public void SpendGold(int amount)
    {
        gold = Mathf.Max(0, gold - amount);
        UpdateGoldUI();
        SaveGold();
    }

    private void UpdateGoldUI()
    {
        if (GoldText != null)
        {
            GoldText.text = gold.ToString();
        }
    }
    public int getGold()
    {
        return gold;
    }
}

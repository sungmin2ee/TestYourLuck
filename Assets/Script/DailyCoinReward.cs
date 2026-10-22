using Firebase.Firestore;
using Firebase.Auth;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;

public class DailyCoinReward : MonoBehaviour
{
    public Button rewardButton;
    public TextMeshProUGUI timerText;
    private FirebaseFirestore db;
    private FirebaseAuth auth;
    private TimeSpan cooldown = TimeSpan.FromHours(24);

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        rewardButton.onClick.AddListener(ClaimReward);
        CheckCooldown();
    }

    void Update()
    {
        CheckCooldown();
    }

    void ClaimReward()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        string uid = auth.CurrentUser.UserId;
        DocumentReference docRef = db.Collection("users").Document(uid);

        // 코인 추가 
        PlayerDataManager.Instance.AddGold(500);

        // 서버에 시간 저장 (UTC 시간)
        docRef.UpdateAsync("lastCoinClaim", Timestamp.GetCurrentTimestamp());
    }

    void CheckCooldown()
    {
        string uid = auth.CurrentUser.UserId;
        DocumentReference docRef = db.Collection("users").Document(uid);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.ContainsField("lastCoinClaim"))
                {
                    Timestamp lastClaim = snapshot.GetValue<Timestamp>("lastCoinClaim");
                    DateTime lastClaimTime = lastClaim.ToDateTime();

                    TimeSpan timePassed = DateTime.UtcNow - lastClaimTime;

                    if (timePassed < cooldown)
                    {
                        TimeSpan remaining = cooldown - timePassed;
                        rewardButton.interactable = false;
                        timerText.text = $"Next Claim: {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                    }
                    else
                    {
                        rewardButton.interactable = true;
                        timerText.text = "Claim!";
                    }
                }
                else
                {
                    // 최초 보상 가능
                    rewardButton.interactable = true;
                    timerText.text = "보상 받기!";
                }
            }
        });
    }
}

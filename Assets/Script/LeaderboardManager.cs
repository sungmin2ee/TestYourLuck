using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Firebase.Auth;

public class LeaderboardManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    public TextMeshProUGUI rank1Name;
    public TextMeshProUGUI rank1Gold;

    public TextMeshProUGUI rank2Name;
    public TextMeshProUGUI rank2Gold;

    public TextMeshProUGUI rank3Name;
    public TextMeshProUGUI rank3Gold;

    public TextMeshProUGUI playerRank;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerGold;

    private FirebaseAuth auth;
    private FirebaseUser currentUser;

    private bool leaderboardStatus = false;
    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        currentUser = auth.CurrentUser;

        GetTop3UsersByGold();
        GetCurrentPlayerData();
    }

    void GetTop3UsersByGold()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .OrderByChild("gold")
            .LimitToLast(3) // 골드 높은 순으로 가져올 때는 마지막부터!
            .GetValueAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.LogError("데이터 로드 실패: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;
                List<(string name, long gold)> topUsers = new List<(string, long)>();

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string name = userSnapshot.Child("displayName").Value?.ToString() ?? "Unknown";
                    long gold = 0;
                    long.TryParse(userSnapshot.Child("gold").Value?.ToString(), out gold);

                    topUsers.Add((name, gold));
                }

                // LimitToLast는 낮은 값부터 오니까 다시 높은 순으로 정렬
                topUsers.Sort((a, b) => b.gold.CompareTo(a.gold));

                if (topUsers.Count > 0)
                {
                    rank1Name.text = topUsers[0].name;
                    rank1Gold.text = topUsers[0].gold.ToString();
                }
                if (topUsers.Count > 1)
                {
                    rank2Name.text = topUsers[1].name;
                    rank2Gold.text = topUsers[1].gold.ToString();
                }
                if (topUsers.Count > 2)
                {
                    rank3Name.text = topUsers[2].name;
                    rank3Gold.text = topUsers[2].gold.ToString();
                }
            });
    }
    void GetCurrentPlayerData()
    {
        if (currentUser == null)
        {
            Debug.LogWarning("로그인된 유저 없음.");
            return;
        }

        string uid = currentUser.UserId;

        FirebaseDatabase.DefaultInstance.GetReference("users")
            .OrderByChild("gold")
            .GetValueAsync()
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.LogError("전체 유저 데이터 로드 실패: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                List<(string uid, string name, long gold)> allUsers = new List<(string, string, long)>();

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string userId = userSnapshot.Key;
                    string name = userSnapshot.Child("displayName").Value?.ToString() ?? "Unknown";
                    long gold = 0;
                    long.TryParse(userSnapshot.Child("gold").Value?.ToString(), out gold);

                    allUsers.Add((userId, name, gold));
                }

                // 골드 높은 순 정렬
                allUsers.Sort((a, b) => b.gold.CompareTo(a.gold));

                // 내 유저를 찾음
                int rank = -1;
                string myName = "Unknown";
                long myGold = 0;

                for (int i = 0; i < allUsers.Count; i++)
                {
                    if (allUsers[i].uid == uid)
                    {
                        rank = i + 1;
                        myName = allUsers[i].name;
                        myGold = allUsers[i].gold;
                        break;
                    }
                }

                if (rank != -1)
                {
                    playerName.text = myName;
                    playerGold.text = myGold.ToString();
                    playerRank.text = rank.ToString();

                }
                else
                {
                    Debug.LogWarning("랭킹 목록에서 본인 UID를 찾지 못함");
                }
            });
    }


    public void RefreshLeaderboard()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        GetTop3UsersByGold();
        GetCurrentPlayerData();
    }
    public void OnLeaderboardButtonPressed()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        if (!leaderboardStatus)
        {
            gameObject.SetActive(true);
            leaderboardStatus = true;
        }
        else
        {
            gameObject.SetActive(false);
            leaderboardStatus = false;
        }
       
    }
    public void OnLeaderboardExitButtonPressed()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        gameObject.SetActive(false);
        leaderboardStatus = false;
    }
}

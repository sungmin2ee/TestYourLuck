using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Firebase.Firestore;

public class LoginWithGoogle : MonoBehaviour
{
    public static LoginWithGoogle Instance;
    public string GoogleAPI = "205055880965-gjbuuomtojqo29ej8888fc3h17sunuqe.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    FirebaseAuth auth;
    FirebaseUser user;

    //private TextMeshProUGUI Username;
    private bool isGoogleSignInInitialized = false;

    public GameObject nicknamePanel;
    public TMP_InputField nicknameInput;
    public Button nicknameConfirmButton;
    public GameObject loginButton;
   
    private bool readyToProceed = false;

    public TextMeshProUGUI touchToStartText;
    private Coroutine blinkCoroutine;

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
        InitFirebase();
        
        if (auth.CurrentUser != null)
        {
            user = auth.CurrentUser;
            Debug.Log("Already logged in as " + user.DisplayName);



            string uid = user.UserId;
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            // displayName±îÁö ÀÖ´ÂÁö ¹Ù·Î È®ÀÎ
            reference.Child("users").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists && snapshot.Child("displayName").Exists)
                    {
                        SceneManager.LoadScene("GameChoose");
                        // ÀÌ¹Ì ´Ð³×ÀÓ±îÁö ÀÖÀ½ -> ÅÍÄ¡ ´ë±â
                        /*readyToProceed = true;
                        if (loginButton != null)
                            loginButton.SetActive(false);

                        if (touchToStartText != null)
                        {
                            touchToStartText.gameObject.SetActive(true);
                            if (blinkCoroutine == null)
                                blinkCoroutine = StartCoroutine(BlinkTouchText());
                        }*/
                    }
                    else
                    {
                        // ´Ð³×ÀÓ ¾ÆÁ÷ ¾øÀ½ ¡æ ´Ð³×ÀÓ ÀÔ·Â ÀýÂ÷·Î

                        SaveUserData();
                    }

                    PlayerDataManager.Instance.LoadGold();
                }
            });
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainWindow")
            return;

        if (loginButton == null)
        {
            loginButton = GameObject.Find("SignIn");
        }
        if (touchToStartText == null)
        {
            GameObject text = GameObject.Find("TouchToStartText");
            if(text != null)
            {
                touchToStartText = text.GetComponent<TextMeshProUGUI>();
            }
        }
        if (readyToProceed && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            readyToProceed = false;

            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            if (touchToStartText != null)
                touchToStartText.gameObject.SetActive(false);

            SceneManager.LoadScene("GameChoose");
        }
    }

    void InitFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login()
    {
        Debug.Log("Login button clicked");
        if (!isGoogleSignInInitialized)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = GoogleAPI,
                RequestEmail = true,
                ForceTokenRefresh = true
            };
            isGoogleSignInInitialized = true;
        }

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Google Sign-In failed.");
                Debug.Log("Google Sign-In failed.");

                return;
            }

            var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled || authTask.IsFaulted)
                {
                    Debug.LogError("Firebase Auth failed.");
                    Debug.Log("Firebase Auth failed.");

                    return;
                }
                Debug.Log("Login Successfully");

                user = auth.CurrentUser;
                Debug.Log("Debugging 1");


                Debug.Log("Debugging 2");

                Debug.Log("Calling SaveUserData() now");
                SaveUserData();
                PlayerDataManager.Instance.LoadGold();
                /*readyToProceed = true;
                loginButton.SetActive(false);*/
            });
        });
        
    }

    private void OnApplicationQuit()
    {
        Debug.Log("App quit - Auto Signout");
        auth.SignOut();
    }

    private void OnDestroy()
    {
        Debug.Log("App destroyed - Auto Signout");
        auth.SignOut();
    }

    public void SaveUserData()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("FailFailFailFailFail");
            return;
        }
        Debug.Log("SuccessSuccessSuccess");
        string uid = auth.CurrentUser.UserId;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        reference.Child("users").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (!snapshot.Exists || !snapshot.Child("displayName").Exists)
                {
                    nicknamePanel.SetActive(true);
                    nicknameConfirmButton.onClick.RemoveAllListeners();
                    nicknameConfirmButton.onClick.AddListener(() =>
                    {
                        string baseName = nicknameInput.text.Trim();
                        if (baseName.Length < 2 || baseName.Length > 10)
                        {
                            Debug.LogWarning("´Ð³×ÀÓÀº 2ÀÚ ÀÌ»ó 10ÀÚ ÀÌÇÏ·Î ÀÔ·ÂÇØ¾ß ÇÕ´Ï´Ù.");
                            return;
                        }

                        if (!System.Text.RegularExpressions.Regex.IsMatch(baseName, @"^[a-zA-Z°¡-ÆR]+$"))
                        {
                            Debug.LogWarning("´Ð³×ÀÓÀº ÇÑ±Û ¶Ç´Â ¿µ¾î¸¸ ÀÔ·ÂÇÒ ¼ö ÀÖ½À´Ï´Ù.");
                            return;
                        }

                        reference.Child("users").GetValueAsync().ContinueWithOnMainThread(allUsersTask =>
                        {
                            if (allUsersTask.IsFaulted) return;

                            DataSnapshot allUsersSnapshot = allUsersTask.Result;
                            int maxSuffix = 0;

                            foreach (DataSnapshot userSnapshot in allUsersSnapshot.Children)
                            {
                                if (userSnapshot.Child("displayName").Exists)
                                {
                                    string existingName = userSnapshot.Child("displayName").Value.ToString();

                                    if (existingName == baseName)
                                        maxSuffix = Mathf.Max(maxSuffix, 1);
                                    else if (existingName.StartsWith(baseName + "#"))
                                    {
                                        string suffixStr = existingName.Substring(baseName.Length + 1);
                                        if (int.TryParse(suffixStr, out int suffixNum))
                                            maxSuffix = Mathf.Max(maxSuffix, suffixNum + 1);
                                    }
                                }
                            }

                            string finalName = (maxSuffix == 0) ? baseName : $"{baseName}#{maxSuffix}";

                            user.UpdateUserProfileAsync(new UserProfile { DisplayName = finalName }).ContinueWithOnMainThread(profileTask =>
                            {
                                if (profileTask.IsCompleted)
                                {
                                    var userData = new Dictionary<string, object>
                                    {
                                        { "displayName", finalName },
                                        { "gold", 1000 }
                                    };

                                    reference.Child("users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(saveTask =>
                                    {
                                        if (saveTask.IsCompleted)
                                        {
                                            Debug.Log("´Ð³×ÀÓ ÀúÀå ¿Ï·á: " + finalName);
                                            //Username.text = finalName;
                                            nicknamePanel.SetActive(false);
                                            SceneManager.LoadScene("GameChoose");
                                        }
                                        else
                                        {
                                            Debug.LogError("DB ÀúÀå ½ÇÆÐ: " + saveTask.Exception);
                                        }
                                    });

                                    DocumentReference docRef = firestore.Collection("users").Document(user.UserId);
                                    docRef.SetAsync(new Dictionary<string, object>
                                {
                                    { "displayName", finalName },
                                    { "gold", 1000 },
                                    { "createdAt", Timestamp.GetCurrentTimestamp() }
                                }).ContinueWithOnMainThread(fsTask =>
                                {
                                    if (fsTask.IsCompleted)
                                        Debug.Log("Firestore ÀúÀå ¿Ï·á: " + finalName);
                                    else
                                        Debug.LogError("Firestore ÀúÀå ½ÇÆÐ: " + fsTask.Exception);
                                });
                                }
                                else
                                {
                                    Debug.LogError("DisplayName ¾÷µ¥ÀÌÆ® ½ÇÆÐ: " + profileTask.Exception);
                                }
                            });
                        });
                    });
                }
                else
                {
                    //Username.text = user.DisplayName;
                    //SceneManager.LoadScene("GameChoose");
                    readyToProceed = true;
                    touchToStartText.gameObject.SetActive(true);
                    loginButton.gameObject.SetActive(false);
                    if (blinkCoroutine == null)
                        blinkCoroutine = StartCoroutine(BlinkTouchText());
                }
            }
        });
    }
    IEnumerator BlinkTouchText()
    {
        while (true)
        {
            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.PingPong(elapsed * 2f, 1f); // 0 ¡ê 1 ¹Ýº¹
                if (touchToStartText != null)
                {
                    var color = touchToStartText.color;
                    color.a = alpha;
                    touchToStartText.color = color;
                }
                yield return null;
            }
        }
    }

    public bool IsLoggedIn() => user != null;

    public string GetUserName() => user?.DisplayName ?? "";

    public void Logout()
    {

        FirebaseAuth.DefaultInstance.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
        GoogleSignIn.DefaultInstance.Disconnect();
        SceneManager.LoadScene("MainWindow");
        loginButton = GameObject.Find("SignIn");
        loginButton.SetActive(true);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

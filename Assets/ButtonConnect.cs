using UnityEngine;
using UnityEngine.UI;

public class ButtonConnect : MonoBehaviour
{
    public Button login;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(login == null)
        {
            GameObject signIn = GameObject.Find("SignIn");
            if(signIn != null)
            {
                login = signIn.GetComponent<Button>();
            }
        }
        if(login != null)
        {
            login.onClick.AddListener(LoginWithGoogle.Instance.Login);
            /*login.onClick.AddListener(() =>
            {
                Debug.Log("Login button clicked!");
            });*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

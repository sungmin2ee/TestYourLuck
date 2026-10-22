using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopPanelUI : MonoBehaviour
{
    //public TextMeshProUGUI usernameText;
    public TextMeshProUGUI goldText;
    private void Update()
    {
        /*if (LoginWithGoogle.Instance != null && LoginWithGoogle.Instance.IsLoggedIn())
        {
            usernameText.text = LoginWithGoogle.Instance.GetUserName();
        }*/

        if (PlayerDataManager.Instance != null)
        {
            goldText.text = PlayerDataManager.Instance.getGold().ToString();
        }
    }

    
}

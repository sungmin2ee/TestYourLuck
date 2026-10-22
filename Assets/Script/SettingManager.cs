using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public GameObject settingPanel;
    private bool isOpened = false;

    public void OnSettingButtonPressed() {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        if (isOpened)
        {
            settingPanel.SetActive(false);
            isOpened = false;
        }
        else
        {
            settingPanel.SetActive(true);
            isOpened = true;
        }
        
    }
    public void LogoutButtonPressed()
    {
        LoginWithGoogle.Instance.Logout();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{

    public void LoadScene(string SceneName)
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        SceneManager.LoadScene(SceneName);
        Debug.Log("button clicked");
    }
}

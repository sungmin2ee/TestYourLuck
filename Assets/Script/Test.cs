using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject button;
    

    // Update is called once per frame
    void Update()
    {
        if(button == null)
        {
           button = GameObject.Find("Button");
        }
    }
    public void SceneChange(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}

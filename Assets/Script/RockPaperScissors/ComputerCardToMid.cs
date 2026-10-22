using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ComputerCardToMid : MonoBehaviour
{
   
    private float duration = 1f;
    private Quaternion targetRotation;
    private float rotationSpeed = 180f;
    private Coroutine moveCoroutine;
    private float speed = 1f;

    public string computerSelection;
    public Button moveButton;
    public Transform target;
    public GameObject rock;
    public GameObject paper;
    public GameObject scissors;
    public GameObject gamePanel;
    public CoinBetting coinBetting;
    

    public static bool restart = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveButton.onClick.AddListener(() =>
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveToTargetAndFlip());
        });
        targetRotation = Quaternion.Euler(0, 0, 180);
    }

   
    IEnumerator MoveToTargetAndFlip()
    {
        gamePanel.SetActive(false);
        computerSelection = ComputerCard.GetComputerSelection();
        GameObject selected = null;
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        if (computerSelection == "Rock") selected = rock;
        else if (computerSelection == "Scissors") selected = scissors;
        else if (computerSelection == "Paper") selected = paper;

        if (selected == null) yield break;

        Vector3 start = selected.transform.position;
        Vector3 end = target.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            selected.transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        selected.transform.position = end;
        Debug.Log($"Selected Object: {selected}");
        StartCoroutine(DelayedRotate(selected, 2f));
        gameResult();
        Debug.Log(Compare.checkResult());
        yield return new WaitForSeconds(5f);
        
        restart = true;
        
        
    }
 
    void rotate(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("Object is null");
            return;
        }
            gameObject.transform.rotation = Quaternion.RotateTowards(
            gameObject.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    IEnumerator DelayedRotate(GameObject obj, float delay)
    {
       
        Debug.Log($"[DelayedRotate] Waiting for {delay} seconds...");
        yield return new WaitForSeconds(delay);
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardFlip);
        Debug.Log($"[DelayedRotate] Starting rotation loop for {obj.name}");
        while (Quaternion.Angle(obj.transform.rotation, targetRotation) > 0.1)
        {
            rotate(obj);
            yield return null;
        }
        

        // 최종 각도 보정 
        obj.transform.rotation = targetRotation;
        
    }

    public void StopAllActions()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    public void gameResult()
    {
        if(Compare.checkResult() == "Win") {
            StartCoroutine(Win());
            
        }
        else if(Compare.checkResult() == "Draw")
        {
            StartCoroutine(Draw());
        }
        else
        {
            VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Lose);
            StartCoroutine(Lose());
   
        }
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(3f);
        PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount() * 3);
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Congrat);
    }
    IEnumerator Draw()
    {
        yield return new WaitForSeconds(3f);
        PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount());
    }
    IEnumerator Lose()
    {
        yield return new WaitForSeconds(3f);
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Lose);
    }


}

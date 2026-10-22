using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEngine.UIElements;

public class moveCards : MonoBehaviour
{
    public Transform target;        // 목표 지점 (B 지점)
    private float duration = 1f;      // 이동에 걸리는 시간 (초)
    public Button moveButton;        // 버튼 오브젝트
    private Vector3 originalPos;
    public float speed = 5f;
    private Coroutine moveCoroutine;

    public GameObject rock;
    public GameObject paper;
    public GameObject scissors;

    private Vector3 rockOriginal = new Vector3(-2.51f , -6f, 0f);
    private Vector3 paperOriginal = new Vector3(-2.51f, -6f, 1.30f);
    private Vector3 scissorsOriginal = new Vector3(-2.51f, -6f, -1.35f);


    void Start()
    {
        moveButton.onClick.AddListener(() => StartCoroutine(StartMove()));
        Debug.Log("Target Pos = " + target.position);


        Debug.Log(rock.transform.position);
        Debug.Log(paper.transform.position);
    
        Debug.Log(scissors.transform.position);
    }

    IEnumerator StartMove()
    {
        // Outline 켜진 오브젝트 찾기
        OutlineToggle[] toggles = FindObjectsOfType<OutlineToggle>();
        Transform selectedObject = null;

        foreach (var toggle in toggles)
        {
            if (toggle.IsOutlined())
            {
                selectedObject = toggle.transform;
                break;
            }
        }

        if (selectedObject == null)
        {
            Debug.LogWarning("선택된 오브젝트가 없습니다.");
            yield break;
        }
        Transform parentTransform = selectedObject.parent;
        originalPos = parentTransform.position;
        Debug.Log("original pos = " + originalPos);
        Vector3 start = parentTransform.position;
        Debug.Log("startPosition = " + start);
        Vector3 end = target.position;
        Debug.Log("EndPosition = " + end);
        float elapsed = 0f;
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        while (elapsed < duration)
        {
            parentTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log(selectedObject.position);
        parentTransform.position = end; // 정확한 위치 보정
    }
    public void resetCard()
    {
        StartCoroutine(ResetMove());
    }

    // 카드 원위치
    IEnumerator ResetMove()
    {
        OutlineToggle[] toggles = FindObjectsOfType<OutlineToggle>();
        Transform selectedObject = null;

        foreach (var toggle in toggles)
        {
            if (toggle.IsOutlined())
            {
                selectedObject = toggle.transform;
                toggle.ToggleOutlineOff();
                break;
            }
        }
        
        if (selectedObject == null)
        {
            Debug.LogWarning("선택된 오브젝트가 없습니다.");
            yield break;
        }

        Transform parentTransform = selectedObject.parent;
       
        Vector3 start = parentTransform.position;
        Vector3 end = Vector3.zero;
        if (selectedObject.parent.name == "Rock")
        {

            end = rockOriginal;
        }
        else if (selectedObject.parent.name == "Paper")
        {
            end = paperOriginal;
        }
        else if (selectedObject.parent.name == "Scissors")
        {
            end = scissorsOriginal;

        }
        
        float elapsed = 0f;
        Debug.Log("Start: " + start);
        Debug.Log("End: " + end);
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        while (elapsed < duration)
        {
            parentTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        parentTransform.position = end; // 위치 정확히 맞추기
    }

}

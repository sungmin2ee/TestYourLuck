
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComputerCard : MonoBehaviour
{
    public GameObject rock;
    public GameObject paper;
    public GameObject scissors;

    private Vector3 [] originalPos;
    

    private Quaternion initialRotation;
    public Transform targetPosition;
    private Quaternion targetRotation;

    public float rotationSpeed = 180f;
    public float speed = 0.01f;
    private bool startRotate = true;
    private bool startMove = false;
    private bool spreadCard = false;
    private bool spreadComplete = false;
    public Transform target; // move selected card to target
    public float duration = 2f;

    private Vector3[] targetSpreadPositions;

    public static string computerSelection;
    public bool computerSelected = false;
    public ComputerCardToMid cardToMid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = new Vector3[] {rock.transform.position, paper.transform.position, scissors.transform.position};
        initialRotation = rock.transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!computerSelected)
        {
            GenerateComputerSelection();
            computerSelected = true;
        }
        if (startRotate)
        {
            rotate();
        }

        if (startMove)
        {
            moveToMiddle();
        }
        if (spreadCard)
        {
            spread();
        }
        if (spreadComplete)
        {
            DrawOutline();
        }
    }
    public void rotate()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardFlip);
        rock.transform.rotation = Quaternion.RotateTowards(
            rock.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        paper.transform.rotation = Quaternion.RotateTowards(
            paper.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        scissors.transform.rotation = Quaternion.RotateTowards(
            scissors.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
        if (rock.transform.rotation == targetRotation &&
            paper.transform.rotation == targetRotation &&
            scissors.transform.rotation == targetRotation)
        {
            startRotate = false;
            startMove = true;
            Debug.Log("startmove");
        }
      
    }
    void moveToMiddle()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        rock.transform.position = Vector3.Lerp(rock.transform.position, targetPosition.position, speed * Time.deltaTime);
        paper.transform.position = Vector3.Lerp(paper.transform.position, targetPosition.position, speed * Time.deltaTime);
        scissors.transform.position = Vector3.Lerp(scissors.transform.position, targetPosition.position, speed * Time.deltaTime);


        if (Vector3.Distance(rock.transform.position, targetPosition.position) < 0.01f &&
            Vector3.Distance(paper.transform.position, targetPosition.position) < 0.01f &&
            Vector3.Distance(scissors.transform.position, targetPosition.position) < 0.01f)
        {
            startMove = false;
            spreadCard = true;
        }
    }
    void spread()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        GameObject[] cards = { rock, paper, scissors };

        // spreadCard가 true일 때 최초 한 번만 셔플
        if (targetSpreadPositions == null)
        {
            targetSpreadPositions = (Vector3[])originalPos.Clone();

            // Fisher-Yates 셔플
            for (int i = 0; i < targetSpreadPositions.Length; i++)
            {
                int random = Random.Range(i, targetSpreadPositions.Length);
                Vector3 temp = targetSpreadPositions[i];
                targetSpreadPositions[i] = targetSpreadPositions[random];
                targetSpreadPositions[random] = temp;
            }
        }

        // 부드럽게 목표 위치로 이동
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, targetSpreadPositions[i], speed * Time.deltaTime);
        }

        // 도달 체크
        if (Vector3.Distance(cards[0].transform.position, targetSpreadPositions[0]) < 0.01f &&
            Vector3.Distance(cards[1].transform.position, targetSpreadPositions[1]) < 0.01f &&
            Vector3.Distance(cards[2].transform.position, targetSpreadPositions[2]) < 0.01f)
        {
            spreadCard = false;
            spreadComplete = true;
            targetSpreadPositions = null; // 다음 Spread를 위해 초기화
            Debug.Log("Spread Complete!");
        }
    }
    IEnumerator MoveToTarget()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CardDrag);
        yield return null;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (computerSelection == "Rock")
            {
                rock.transform.position = Vector3.Lerp(rock.transform.position, target.position, speed * Time.deltaTime);
            }
            else if (computerSelection == "Scissors")
            {
                scissors.transform.position = Vector3.Lerp(scissors.transform.position, target.position, speed * Time.deltaTime);
            }
            else if (computerSelection == "Paper")
            {
                paper.transform.position = Vector3.Lerp(paper.transform.position, target.position, speed * Time.deltaTime);
            }
        }

    }
    void DrawOutline()
    {
        if (computerSelection == "Rock")
        {
            Transform rockChild = rock.transform.Find("Rock");
            if (rockChild != null)
            {
                ComputerOutline outline = rockChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutline();
            }
        }
        else if (computerSelection == "Paper")
        {
            Transform paperChild = paper.transform.Find("Paper");
            if (paperChild != null)
            {
                ComputerOutline outline = paperChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutline();
            }
        }
        else if (computerSelection == "Scissors")
        {
            Transform scissorsChild = scissors.transform.Find("Scissors");
            if (scissorsChild != null)
            {
                ComputerOutline outline = scissorsChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutline();
            }
        }
        spreadComplete = false;
    }
    void EraseOutline()
    {
        if (computerSelection == "Rock")
        {
            Transform rockChild = rock.transform.Find("Rock");
            if (rockChild != null)
            {
                ComputerOutline outline = rockChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutlineOff();
            }
        }
        else if (computerSelection == "Paper")
        {
            Transform paperChild = paper.transform.Find("Paper");
            if (paperChild != null)
            {
                ComputerOutline outline = paperChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutlineOff();
            }
        }
        else if (computerSelection == "Scissors")
        {
            Transform scissorsChild = scissors.transform.Find("Scissors");
            if (scissorsChild != null)
            {
                ComputerOutline outline = scissorsChild.GetComponent<ComputerOutline>();
                if (outline != null) outline.ToggleOutlineOff();
            }
        }
    }

    public void GenerateComputerSelection()
    {
        string[] options = { "Rock", "Paper", "Scissors" };
        int randomIndex = Random.Range(0, options.Length);
        computerSelection = options[randomIndex];
        Debug.Log($"Computer selected: {computerSelection}");
    }
    public static string GetComputerSelection()
    {
        return computerSelection;
    }
    
    public void Reset()
    {
        StartCoroutine(Reset1());
    }
    IEnumerator Reset1()
    {
        cardToMid.StopAllActions();
        EraseOutline();
       


        rock.transform.rotation = initialRotation;
        rock.transform.position = originalPos[0];
        paper.transform.rotation = initialRotation;
        paper.transform.position = originalPos[1];
        scissors.transform.rotation = initialRotation;
        scissors.transform.position = originalPos[2];

        yield return new WaitForSeconds(2);
        computerSelected = false;
        startRotate = true;
    }
}

using UnityEngine;

public class Compare : MonoBehaviour
{
    private static string selected;
    private static string matchResult;

    void Update()
    {
        selected = OutlineManager.GetSelected();
    }

    public static string checkResult()
    {
        
        if (string.IsNullOrEmpty(selected))
        {
            Debug.Log("No selection made!");
            return null;
        }
        string computerSelection = ComputerCard.GetComputerSelection();
        switch (selected)
        {

            case "Rock":
                matchResult = (computerSelection == "Paper") ? "Lose" :
                              (computerSelection == "Rock") ? "Draw" : "Win";
                break;
            case "Paper":
                matchResult = (computerSelection == "Scissors") ? "Lose" :
                              (computerSelection == "Paper") ? "Draw" : "Win";
                break;
            case "Scissors":
                matchResult = (computerSelection == "Rock") ? "Lose" :
                              (computerSelection == "Scissors") ? "Draw" : "Win";
                break;
            default:
                matchResult = "Invalid Selection";
                break;
        }
        return matchResult;
        Debug.Log($"Player: {selected}, Computer: {computerSelection}, Result: {matchResult}");
    }
  
    
}

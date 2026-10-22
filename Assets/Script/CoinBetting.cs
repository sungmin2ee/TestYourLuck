using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class CoinBetting : MonoBehaviour
{
    public TMP_InputField inputField;
    private int betAmount;
    public bool playerBet = false;
    public GameObject bettingPanel;
    public GameObject gamePanel;


    public void OnEndEdit(string value)
    {
        if (int.TryParse(value, out int parsed))
        {
            betAmount = parsed;
            
        }
        else
        {
            Debug.LogWarning("Enter an integer.");
            inputField.text = "";
        }

        EventSystem.current.SetSelectedGameObject(null);
        inputField.DeactivateInputField();
    }

    
    public int getBettingAmount()
    {
        return betAmount;
    }
    public void OnBetButtonPress()
    {
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        if (!int.TryParse(inputField.text, out betAmount))
        {
            Debug.LogWarning("Invalid input");
            return;
        }
        int playerOwn = PlayerDataManager.Instance.getGold();

        if (betAmount > playerOwn)
        {
            betAmount = playerOwn;
            PlayerDataManager.Instance.SpendGold(playerOwn);
            bettingPanel.SetActive(false);
            gamePanel.SetActive(true);
            playerBet = true;
            Debug.Log("Bet: " + betAmount);

        }
        else
        {
            PlayerDataManager.Instance.SpendGold(betAmount);
            bettingPanel.SetActive(false);
            gamePanel.SetActive(true);
            playerBet = true;
            Debug.Log("Bet: " + betAmount);
           
        }
        
    }
}

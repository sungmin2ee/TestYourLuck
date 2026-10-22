using UnityEngine;

public class GameReset : MonoBehaviour
{
    public moveCards playerCard;
    public ComputerCard computerCard;
    public GameObject bettingPanel;
    public GameObject gamePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ComputerCardToMid.restart)
        {
            playerCard.resetCard();
            computerCard.Reset();
            ComputerCardToMid.restart = false;
            gamePanel.SetActive(false);
            bettingPanel.SetActive(true);
        }
    }
}

using UnityEngine;

public class BackButtonPress : MonoBehaviour
{
    public CoinBetting coinBetting;

    public void onBackButtonPress()
    {
        if(coinBetting != null)
        {
            PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount());
            coinBetting.playerBet = false;
            coinBetting.gamePanel.SetActive(false);
            coinBetting.bettingPanel.SetActive(true);
            VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
        }
    }
}

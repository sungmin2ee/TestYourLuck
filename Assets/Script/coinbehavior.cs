using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class coinbehavior : MonoBehaviour
{
    public GameObject coin;
    public Rigidbody rb;
    public CoinBetting coinBetting;
    private bool resultChecked = true;
    private bool tossClicked = false;
    private bool isStopped = false;
    private bool firstFrame = false;
    private string playerGuess;
    public ParticleSystem winEffectPrefab;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (coinBetting.gamePanel == null)
        {
            Debug.Log("Null");
        }else if(coinBetting.bettingPanel == null)
        {
            Debug.Log("Null");
        }
        else
        {
            Debug.Log("exist");
           
        }
        if (winEffectPrefab == null)
        {
            Debug.Log("Null");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (tossClicked && firstFrame)
        {
            isStopped = coinStopped();

            if (!resultChecked && isStopped)
            {
                CheckResult();
                StartCoroutine(DelayedResetCoin());
            }
        }
    }


    bool coinStopped()
    {
        return rb.angularVelocity.magnitude == 0 && rb.linearVelocity.magnitude == 0;
    }

    void CheckResult()
    {
        if (Vector3.Dot(transform.up, Vector3.up) > 0.5f)
        {
            Debug.Log("Result: HEAD");
            resultChecked = true;
            if (playerGuess == "head")
            {
                Debug.Log("Player Win");
                playParticle();
                VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Congrat);
                //coinBetting.bettingPanel.SetActive(true);

                PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount() * 2);
            }
            else
            {
                Debug.Log("Player Lose");
                //coinBetting.bettingPanel.SetActive(true);
              
                coinBetting.playerBet = false;
               
            }
            
        }
        else if(Vector3.Dot(transform.up, Vector3.up) < 0.5f)
        {
            resultChecked = true;
            Debug.Log("Result: TAIL");
            if (playerGuess == "tail")
            {
                Debug.Log("Player Win");
                playParticle();
                //coinBetting.bettingPanel.SetActive(true);
                PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount() * 2);
            }
            else
            {
                Debug.Log("Player Lose");
                //coinBetting.bettingPanel.SetActive(true);
                coinBetting.playerBet = false;
            }
        }
        else
        {
            resultChecked = true;
            playParticle();
            PlayerDataManager.Instance.AddGold(coinBetting.getBettingAmount() * 3);
        }
    }
    void ResetCoin()
    {

        coin.transform.position = new Vector3(0,5,-50);
    }
    IEnumerator DelayedResetCoin()
    {
        yield return new WaitForSeconds(3f);
        ResetCoin();
        tossClicked = false;
        coinBetting.bettingPanel.SetActive(true);
    }
    public void cointoss(string playerBet)
    {
        if(playerBet == "head")
        {
            playerGuess = "head";
        }else if(playerBet == "tail")
        {
            playerGuess = "tail";
        }
        int jumpForce = Random.Range(150, 250);
        rb.AddForce(0, jumpForce, 0);
        int torqx = Random.Range(20, 200);
        int torqz = Random.Range(20, 200);
        rb.AddTorque(torqx, 0, torqz);
        coinBetting.gamePanel.SetActive(false);
        tossClicked = true;
        resultChecked = false;
        firstFrame = false;
        VolumeSettings.Instance.PlayLoopingSfx(VolumeSettings.Sfx.CoinFlip);

        StartCoroutine(EnableCheckAfterFrame());

    }
    IEnumerator EnableCheckAfterFrame()
    {
        yield return new WaitForFixedUpdate();
        firstFrame = true;
    }
    public void playParticle()
    {

        if (winEffectPrefab != null)
        {
            ParticleSystem effect = Instantiate(winEffectPrefab, coin.transform.position + Vector3.up * 1.5f + Vector3.forward * 1.5f, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 3f); 
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // ¹Ù´Ú°ú ºÎµúÇû´ÂÁö È®ÀÎ 
        if (collision.gameObject.CompareTag("Ground"))
        {
            VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.CoinDropOnGround);
            VolumeSettings.Instance.StopLoopingSfx();
        }
        
    }
}

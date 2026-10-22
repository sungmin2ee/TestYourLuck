using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class VolumeSettings : MonoBehaviour
{
    public static VolumeSettings Instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource [] sfxPlayers;
   
    public Slider bgmSlider;
    public Slider sfxSlider;
    int channelIndex;
    AudioSource loopingSfxPlayer;
    public enum Sfx { Button, CoinFlip, CoinDropOnGround, CoinDropOnTable, CardFlip, CardDrag, Congrat, Lose}
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
        bgmPlayer.Play();
        
    }
    private void Start()
    {
        
        if (bgmSlider != null)
        {
            bgmSlider.value = bgmVolume;
            SetBgmVolume(bgmSlider.value);
            bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            SetSfxVolume(sfxSlider.value);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }
        
    }
    private void Update()
    {
        if(bgmSlider == null)
        {
            bgmSlider = GameObject.Find("Setting/Music Slider")?.GetComponent<Slider>();
            bgmSlider.value = bgmVolume;
            SetBgmVolume(bgmSlider.value);
            bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        }
        if(sfxSlider == null)
        {
            sfxSlider = GameObject.Find("Setting/SFX Slider")?.GetComponent<Slider>();
            sfxSlider.value = sfxVolume;
            SetSfxVolume(sfxSlider.value);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }
    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = true;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        //효과음 루프플레이어 초기화 
        GameObject loopingObject = new GameObject("LoopingSfxPlayer");
        loopingObject.transform.parent = transform;
        loopingSfxPlayer = loopingObject.AddComponent<AudioSource>();
        loopingSfxPlayer.loop = true;
        loopingSfxPlayer.playOnAwake = false;
        loopingSfxPlayer.volume = sfxVolume;

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
        
    }
    public void PlayLoopingSfx(Sfx sfx)
    {
        loopingSfxPlayer.clip = sfxClips[(int)sfx];
        loopingSfxPlayer.Play();
    }

    public void StopLoopingSfx()
    {
        if (loopingSfxPlayer.isPlaying)
            loopingSfxPlayer.Stop();
    }
    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        if (bgmPlayer != null)
            bgmPlayer.volume = bgmVolume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;

        // 일반 SFX 채널들 반영
        foreach (var sfx in sfxPlayers)
        {
            sfx.volume = sfxVolume;
        }

        // 루프용 SFX 채널에도 반영
        if (loopingSfxPlayer != null)
            loopingSfxPlayer.volume = sfxVolume;
    }
   

}

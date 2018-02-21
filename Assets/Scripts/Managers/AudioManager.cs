using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioManager;
    public AudioSource MusicManager;
    public AudioClip Explosion;
    public AudioClip Shot;
    public AudioClip WinGame;
    public AudioClip LoseGame;
    public AudioClip StartGame;
    public AudioClip SwitchTarget;
    public AudioClip BaseDestroyed;
    public AudioClip CardClick;
    public AudioClip Click;
    public static AudioManager instance;
    public UISlider SoundSlider;
    public UISlider MusicSlider;

    public float soundVolume
    {
        get { return audioManager.volume; }
        set
        {
            audioManager.volume = value;
            PlayerPrefs.SetFloat("SoundVolume", audioManager.volume);
            PlayerPrefs.Save();
        }
    }

    public float musicVolume
    {
        get { return MusicManager.volume; }
        set
        {
            MusicManager.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", MusicManager.volume);
            PlayerPrefs.Save();
        }
    }

    void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        else
            if (instance != this)
        {
            Destroy(gameObject);
        }
        audioManager = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioManager.volume = PlayerPrefs.GetFloat("SoundVolume");
        SoundSlider.value = audioManager.volume;
        MusicManager.volume = PlayerPrefs.GetFloat("MusicVolume");
        MusicSlider.value = MusicManager.volume;
    }

    public void ExplosionPlay()
    {
        audioManager.PlayOneShot(Explosion);
    }

	public void WinGamePLay()
	{
        StopMusic();
        audioManager.PlayOneShot(WinGame);
	}

	public void LoseGamePLay()
	{
        StopMusic();
        audioManager.PlayOneShot(LoseGame);
	}

	public void StartGamePLay()
	{
        audioManager.PlayOneShot(StartGame);
        StartCoroutine(StartMusicPlay());
	}

	public void SwitchTargetPlay()
	{
        audioManager.PlayOneShot(SwitchTarget);
	}

	public void BaseDestroyedPlay()
	{
        audioManager.PlayOneShot(BaseDestroyed);
	}

    public void CannoballShotPlay()
    {
        audioManager.PlayOneShot(Shot);
    }

    public void PlayMusic()
    {
        MusicManager.Play();
    }

    public void StopMusic()
    {
        MusicManager.Stop();
    }

    public void SoundVolumeChange()
    {
        soundVolume = SoundSlider.value;
    }

    public void MusicVolumeChange()
    {
        musicVolume = MusicSlider.value;
    }

    public void StopAudio()
    {
        audioManager.Stop();
    }

    public void PlayAudio()
    {
        audioManager.Play();

	}

	public void ClickPlay()
	{
        audioManager.PlayOneShot(Click);
	}

    public void CardClickPlay()
    {
        audioManager.PlayOneShot(CardClick);
    }
    
    IEnumerator StartMusicPlay()
    {
        yield return new WaitForSeconds(StartGame.length);
        PlayMusic();
    }
}

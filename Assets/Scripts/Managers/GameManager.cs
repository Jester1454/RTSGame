using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
	InGame,
	GameIsOver,
	GameOnPause
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField]
    private GameObject PausePanel;

	[SerializeField]
	private GameObject PauseButton;

    [SerializeField]
    private UILabel PlayTime;

    [SerializeField]
    private GameObject WinPanel;

    [SerializeField]
    private GameObject LosePanel;
	
	
    public float minutes = 0;
    public float seconds = 0;

    public GameState GameState=GameState.InGame;

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
		StartCoroutine(InputTime());
	}

    private void Start()
    {
        AudioManager.instance.StopMusic();
		AudioManager.instance.StartGamePLay();
	}

    private void Update()
    {
#if UNITY_EDITOR
	    if (GameState == GameState.GameIsOver &&Input.GetMouseButton(0))
	    {
		    Replay();
	    }  
#endif
        if (GameState == GameState.GameIsOver && Input.touchCount>0)
        {
            Replay();
        }          
    }


    public void GameOver()
    {
        LosePanel.SetActive(true);
        AudioManager.instance.LoseGamePLay();
		GameState = GameState.GameIsOver;
		PlayTime.gameObject.SetActive(false);
		PauseButton.SetActive(false);
		Time.timeScale = 0.0f;
	}

    public void GameWin()
    {
        WinPanel.SetActive(true);
        AudioManager.instance.WinGamePLay();
        GameState = GameState.GameIsOver;
		PlayTime.gameObject.SetActive(false);
        PauseButton.SetActive(false);
		Time.timeScale = 0.0f;
	}

    public void OnPause()
    {
        if (GameState == GameState.GameOnPause)
        {
            UIManager.instance.OffSettings();
			OffPause();
		}
        else
        {
			GameState = GameState.GameOnPause;
			PausePanel.SetActive(true);
			Time.timeScale = 0.0f;
            PlayTime.gameObject.SetActive(false);
		}
    }

    public void OffPause()
    {
        GameState = GameState.InGame;
		PausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        PlayTime.gameObject.SetActive(true);
	}

	public void Menu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1.0f;
        GameState = GameState.InGame;
	}

	public void Replay()
	{
		SceneManager.LoadScene("LoadScene");
		Time.timeScale = 1.0f;
		GameState = GameState.InGame;
		GameManager.instance.minutes = 0;
		GameManager.instance.seconds = 0;
	}

    IEnumerator InputTime()
    {
        while (true)
        {
			minutes = Mathf.Floor(Time.timeSinceLevelLoad / 60);
            seconds = (Time.timeSinceLevelLoad % 60);
            PlayTime.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            yield return new WaitForSeconds(1.0f);
        }
    }
}
    
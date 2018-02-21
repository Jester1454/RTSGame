using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

	public GameObject SettingsPanel;

	public GameObject DifficultChangePanel;
	
	public GameObject MainMenuButtons;	
	
	public GameObject CollectionsMenu;
	
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
	}

	public void Play()
	{
		 StartGame();
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void OnSettings()
	{
		SettingsPanel.SetActive(true);
        MainMenuButtons.SetActive(false);
	}

	public void OffSettings()
	{
		SettingsPanel.SetActive(false);
        MainMenuButtons.SetActive(true);
    }

    public void HardPlay()
    {
        //string Difficult = "Hard";
        //PlayerPrefs.SetString("Difficult", Difficult);
        //PlayerPrefs.Save();
//        DifficultManager.instance.SetHard();
		StartGame();
	}

	public void MediumPlay()
	{
		//PlayerPrefs.SetString("Difficult", "Medium");
		//PlayerPrefs.Save();
      //  DifficultManager.instance.SetMedium();
		StartGame();	}

	public void EasyPlay()
	{
		//PlayerPrefs.SetString("Difficult", "Easy");
		//PlayerPrefs.Save();
 //       DifficultManager.instance.SetEasy();
		StartGame();    
    }

    void StartGame()
    {
		SceneManager.LoadScene("LoadScene");
		Time.timeScale = 1.0f;
	}

	public void OpenCollection()
	{
		MainMenuButtons.SetActive(false);
		CollectionsMenu.SetActive(true);
	}

	public void CloseCollection()
	{
		CollectionsMenu.SetActive(false);
		MainMenuButtons.SetActive(true);
	}
}

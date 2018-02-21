using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cards;
using PrototypeScripts;
using UnityEngine;

public class SaveDeckManager : MonoBehaviour
{

	public static SaveDeckManager instance;

	private DeckConfig deckConfig;
		//private bool isDowloand = false;
	#region URL

	public static string DeckConfigURL
	{
		get { return "https://jester1454.github.io/SeaBattleDeck.JSON"; }
	}
	#endregion

	#region Pathes
	public static string SavedGamesPath 
	{
		get { return Application.persistentDataPath + Path.DirectorySeparatorChar + "Saves"; }
	}
	
	public static string NameSaveDeckFile 
	{
		get { return "Deck.json"; }
	}
	#endregion

	private void Awake()
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
		LoadDeck();
		//SetConfigFromURL(DeckConfigURL);
	}
	
	private void SetConfigFromURL(string url)
	{
		StartCoroutine(DowloadConfig(url));
	}
	
	private IEnumerator DowloadConfig(string url)
	{		
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			File.WriteAllText(SavedGamesPath + NameSaveDeckFile, www.text);
			LoadDeck();
		}
		else
		{
			Debug.LogError(www.error);
		}     
	}

	private void LoadDeck()
	{
		deckConfig = LoadSaveData<DeckConfig>(SavedGamesPath + NameSaveDeckFile);
	}

	public DeckConfig GetConfig()
	{
		return deckConfig;
	}
	
	public void SaveDeck(Deck deck)
	{
		SaveObject(fromDeckToConfig(deck), SavedGamesPath + NameSaveDeckFile);
	}
	
	private DeckConfig fromDeckToConfig(Deck deck)
	{
		DeckConfig config = new DeckConfig(deck.DeckSize);

		for (int i = 0; i < deck.DeckSize; i++)
		{
			config.Cards[i].Id = deck.Cards[i].Id;
		}
		return config;
	}

	
	private T LoadSaveData<T>(string filePath)
	{
		if (File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText (filePath);
			return  JsonUtility.FromJson<T>(dataAsJson);
		}
		return default(T);
	}

	private void SaveObject<T>(T obj, string path)
	{
		File.WriteAllText(path, JsonUtility.ToJson (obj, true));	
	}
}

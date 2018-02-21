using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour 
{
	private bool loadScene = false;

	[SerializeField]
	private int scene;
	[SerializeField]
	private Text loadingText;

	void Update() {

		if (loadScene == false) 
		{
			loadScene = true;
			loadingText.text = "Loading...";
			StartCoroutine(LoadNewScene());
		}

		if (loadScene == true) 
		{
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}

	}

	IEnumerator LoadNewScene()
	{
		yield return new WaitForSeconds(3);
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);
		while (!async.isDone) 
		{
			yield return null;
		}
	}

}
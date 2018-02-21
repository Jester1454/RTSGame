using System.Collections;
using UnityEngine;

public class CircularTimer : MonoBehaviour 
{

	public float maxTime;
	
	[HideInInspector]
	public bool isStop = true;
	
	private UISprite timerTexture;
	
	public delegate void Stop();
	public event Stop stop = delegate { };
	
	private void Start()
	{
		timerTexture = GetComponent<UISprite>();
	}

	public void StartTurnTimer()
	{
		if (isStop)
		{
			isStop = false;
			StartCoroutine("StartTimer");
		}
	}

	public void StopTimer()
	{
		timerTexture.color = Color.white;
		timerTexture.fillAmount = 1;
		isStop = true;
		stop();
	}
	
	private IEnumerator StartTimer()
	{
		float i = 0;
		float rate = 1 / maxTime;
		while (i < 1)
		{
			i += rate * Time.deltaTime;
			timerTexture.fillAmount = Mathf.Lerp(1, 0, i);
			if (i > 0.50f)
				timerTexture.color = Color.red;
			yield return null;
		}
		StopTimer();
	}
}

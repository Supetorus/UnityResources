using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
	[SerializeField, Tooltip("The text object that displays the countdown.")] TMP_Text countdown;
	[SerializeField, Tooltip("The number of seconds to count down from")] int startTime;
	[SerializeField, Tooltip("The object which is activated when coundown is over")] GameObject toActivate;
	//[SerializeField, Tooltip("The position to spawn the object")] Transform position;

	private float timer;
	// Start is called before the first frame update
	void Start()
	{
		timer = startTime;
		countdown.text = startTime.ToString();
		Time.timeScale = 0;
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.unscaledDeltaTime;
		countdown.text = Mathf.CeilToInt(timer).ToString();
		if (timer <= 0)
		{
			Time.timeScale = 1;
			Game.Instance.PlayerStart();
			countdown.text = "";
			toActivate.SetActive(true);
			//Instantiate(toInstantiate, position.position, position.rotation);
		}
	}
}

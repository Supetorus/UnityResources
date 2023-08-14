using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityAgent : Agent
{
	[SerializeField] Perception perception;
	[SerializeField] MeterUI meter;

	const float MIN_SCORE = 0.1f;

	Need[] needs;
	UtilityObject activeUtilityObject = null;

	public bool isUsingUtilityObject { get { return activeUtilityObject != null; } }
	public float happiness
	{
		get
		{
			float totalMotive = 0;
			foreach (Need need in needs)
			{
				totalMotive += need.motive;
			}
			return 1 - (totalMotive / needs.Length);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		needs = GetComponentsInChildren<Need>();

		meter.text.text = "";
	}

	// Update is called once per frame
	void Update()
	{
		if (activeUtilityObject == null)
		{
			var gameObjects = perception.GetGameObjects();
			List<UtilityObject> utilityObjects = new List<UtilityObject>();
			foreach (var go in gameObjects)
			{
				if (go.TryGetComponent(out UtilityObject utilityObject))
				{
					utilityObject.visible = true;
					utilityObject.score = GetUtilityObjectScore(utilityObject);
					if (utilityObject.score > MIN_SCORE) utilityObjects.Add(utilityObject);
				}
			}
			activeUtilityObject = utilityObjects.Count > 0 ? GetRandomUtilityObject(utilityObjects.ToArray()) : null;
			if (activeUtilityObject != null)
			{
				StartCoroutine(ExecuteUtilityObject(activeUtilityObject));
			}
		}
	}

	private void LateUpdate()
	{
		meter.slider.value = happiness;
		meter.worldPosition = transform.position + Vector3.up * 4;
	}

	IEnumerator ExecuteUtilityObject(UtilityObject utilityObject)
	{
		print("Walking to " + utilityObject.name);
		movement.MoveTowards(utilityObject.location.position);
		while (Vector3.Distance(transform.position, utilityObject.location.position) > 0.2f)
		{
			Debug.DrawLine(transform.position, utilityObject.location.position);
			yield return null;
		}

		print("start effect " + utilityObject.name);
		if (utilityObject.effect != null) utilityObject.effect.SetActive(true);
		yield return new WaitForSeconds(utilityObject.duration);
		print("end effect " + utilityObject.name);
		if (utilityObject.effect != null) utilityObject.effect.SetActive(false);
		ApplyUtilityObject(utilityObject);
		activeUtilityObject = null;

		yield return null;
	}

	void ApplyUtilityObject(UtilityObject utilityObject)
	{
		foreach (var effector in utilityObject.effectors)
		{
			Need need = GetNeedByType(effector.type);
			if (need != null)
			{
				need.input += effector.change;
				need.input = Mathf.Clamp(need.input, -1, 1);
			}
		}
	}

	float GetUtilityObjectScore(UtilityObject uo)
	{
		float score = 0;

		foreach (var effector in uo.effectors)
		{
			Need need = GetNeedByType(effector.type);
			if (need != null)
			{
				float futureNeed = need.GetMotive(need.input + effector.change);
				score += need.motive - futureNeed;
			}
		}

		return score;
	}

	Need GetNeedByType(Need.Type t)
	{
		return needs.First(need => need.type == t);
	}

	UtilityObject GetHighestUtilityObject(UtilityObject[] utilityObjects)
	{
		UtilityObject highestUtilityObject = null;
		float highestScore = MIN_SCORE;
		foreach (var utilityObject in utilityObjects)
		{
			if (utilityObject.score > highestScore)
			{
				highestScore = utilityObject.score;
				highestUtilityObject = utilityObject;
			}
		}

		return highestUtilityObject;
	}

	UtilityObject GetRandomUtilityObject(UtilityObject[] utilityObjects)
	{
		// evaluate all utility objects
		float[] scores = new float[utilityObjects.Length];
		float totalScore = 0;
		for (int i = 0; i < utilityObjects.Length; i++)
		{
			scores[i] = utilityObjects[i].score;
			totalScore += scores[i];
		}

		// select random utility object based on score
		// the higher the score the greater the chance of being randomly selected

		float random = Random.Range(0, totalScore);
		for (int i = 0; i < scores.Length; i++)
		{
			if (random < scores[i])
			{
				return utilityObjects[i];
			}
			random -= scores[i];
			// <check if random value is less than scores[i]>
			// <return utilityObjects[i] if less than>
			// <subtract scores[i] from random value>
		}

		return null;
	}

	private void OnGUI()
	{
		Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

		GUI.color = Color.black;
		int offset = 0;
		foreach (var need in needs)
		{
			GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), need.type.ToString() + ": " + need.motive);
			offset += 20;
		}
		//GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), mood.ToString());
	}
}

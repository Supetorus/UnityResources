using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityObject : MonoBehaviour
{
	[System.Serializable]
	public class Effector
	{
		public Need.Type type;
		[Range(-2, 2)]
		public float change;
	}

	[SerializeField] MeterUI meterPrefab;
	public float duration;
	public Transform location;
	public GameObject effect;


	public Effector[] effectors;
	public Dictionary<Need.Type, float> registry = new Dictionary<Need.Type, float>();

	public float score { get; set; }
	public bool visible { get; set; }

	MeterUI meter;
	// Start is called before the first frame update
	void Start()
	{
		meter = Instantiate(meterPrefab, GameObject.Find("Canvas").transform);
		meter.name = name;
		meter.text.text = name;

		foreach (var effector in effectors)
		{
			registry[effector.type] = effector.change;
		}
	}

	public float GetEffectorChange(Need.Type type)
	{
		registry.TryGetValue(type, out float change);
		return change;
	}

	public bool HasEffector(Need.Type type)
	{
		return registry.ContainsKey(type);
	}

	private void LateUpdate()
	{
		meter.gameObject.SetActive(visible);
		meter.worldPosition = transform.position + (Vector3.up * 2);
		meter.slider.value = score;
		visible = false;
	}
}

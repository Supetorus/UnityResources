using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyCreator : MonoBehaviour
{
	[SerializeField] Body bodyPrefab;
	[SerializeField] FloatData speed;
	[SerializeField] FloatData size;
	[SerializeField] FloatData Density;
	[SerializeField] FloatData Drag;
	[SerializeField] FloatData Restitution;
	[SerializeField] EnumData bodyType;

	bool action = false;
	/// <summary>
	/// If it has ever been pressed
	/// </summary>
	bool pressed = false;

	void Update()
	{
		if (action && (pressed || Input.GetKey(KeyCode.LeftControl)))
		{
			pressed = false;
			Vector3 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);
			Body body = Instantiate(bodyPrefab, position, Quaternion.identity);
			body.BodyType = (Body.eBodyType)bodyType.value;
			body.drag = Drag.value;
			body.restitution = Restitution.value;
			body.shape.Size = size.value;
			body.shape.Density = Density.value;
			body.ApplyForce(Random.insideUnitCircle.normalized * speed.value, Body.ForceMode.VELOCITY);
			Simulator.Instance.bodies.Add(body);
		}
	}

	public void OnPointerDown()
	{
		if (Input.GetMouseButton(0))
		{

			action = true;
			pressed = true;
		}
	}

	public void OnPointerExit()
	{
		action = false;
	}

	public void OnPointerUp()
	{
		action = false;
	}
}

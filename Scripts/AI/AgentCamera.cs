using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCamera : MonoBehaviour
{
	[SerializeField] Vector3 localPosition = new Vector3(0, 5, -10);
	[SerializeField] float angle = 20;

	Camera followCamera;

	void Start()
	{
		followCamera = Camera.main;
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			Agent[] agents = Agent.GetAgents<Agent>();
			if (agents.Length == 0) return;

			followCamera.transform.parent = agents[Random.Range(0, agents.Length)].transform;
			followCamera.transform.localPosition = localPosition;
			followCamera.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.right);
		}
	}
}

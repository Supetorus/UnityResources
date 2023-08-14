using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSelector : MonoBehaviour
{
	[SerializeField] LayerMask layerMask;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
			{
				StateAgent[] agents = Agent.GetAgents<StateAgent>();
				foreach (var agent in agents)
				{
					if (agent.TryGetComponent<Movement>(out Movement movement))
					{
						movement.MoveTowards(hitInfo.point);
					}
				}
			}
		}
	}
}	

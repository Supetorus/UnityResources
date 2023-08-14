using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphNodeCreator : MonoBehaviour
{
	public GameObject nodePrefab;
	public LayerMask layerMask;
	public float neighborRadius = 3;
	public float grid = 2;

	void Update()
	{
		if ((Input.GetMouseButtonDown(1) ||
			(Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))))
		{
			Vector3 position = Vector3.zero;
			if (Utilities.ScreenToWorld(Input.mousePosition, layerMask, ref position))
			{
				position = Utilities.SnapToGrid(position, new Vector3(grid, 0, grid));//Vector3.one * grid);
																					  // make sure position doesn't have a node already
				if (Physics.CheckSphere(position, grid * 0.25f, 1 << nodePrefab.layer))
				{
					return;
				}

				Ray ray = new Ray(Camera.main.transform.position, position - Camera.main.transform.position); //Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out RaycastHit hitInfo, 100))
				{
					// check if hit layer mask
					if (((1 << hitInfo.collider.gameObject.layer) & layerMask) == 0) return;

					// create node
					Instantiate(nodePrefab, position, Quaternion.identity, transform);

					// unlink/link nodes within radius
					GraphNode.UnlinkNodes();
					GraphNode.LinkNodes(neighborRadius);
				}
			}
		}
	}


}
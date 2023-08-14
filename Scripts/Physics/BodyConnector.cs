using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyConnector : MonoBehaviour
{
    [SerializeField] FloatData springK;

	Body source;

	private void Update()
    {
		if (source != null)
		{
			Vector2 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);
			Debug.DrawLine(position, source.transform.position);
		}
	}

    void Create(Body bodyA, Body bodyB, float restLength, float k)
    {
        Spring spring = new Spring();
        spring.BodyA = bodyA;
        spring.BodyB = bodyB;
        spring.RestLength = restLength;
        spring.K = k;

        bodyA.springs.Add(spring);
    }


	public void OnPointerDown()
	{
		if (Input.GetMouseButton(1)) // right mouse button
		{
			source = Simulator.Instance.GetScreenToBody(Input.mousePosition);
		}
	}

	public void OnPointerUp()
	{
		if (source != null)
		{
			Body destination = Simulator.Instance.GetScreenToBody(Input.mousePosition);
			if (destination != null && destination != source)
			{
				float restLength = (source.Position - destination.Position).magnitude;
				Create(source, destination, restLength, springK.value);
			}
		}

		source = null;
	}

	public void OnPointerExit()
	{
		source = null;
	}

}

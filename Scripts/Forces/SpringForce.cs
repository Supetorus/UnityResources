using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringForce : Force
{
	[SerializeField] BoolData global;
	[SerializeField] FloatData k;
	[SerializeField] FloatData length;

	public override void ApplyForce(List<Body> bodies)
	{
		if (global.value)
		{
			bodies.ForEach(b => b.springs.ForEach(s =>
			{
				s.K = k.value;
				s.RestLength = length.value;
			}));
		}

		bodies.ForEach(b => b.springs.ForEach(s => s.ApplyForce()));
	}
}

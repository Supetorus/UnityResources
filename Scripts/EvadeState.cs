using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State
{
	private float prevAngle;
	private float prevDist;
	public EvadeState(StateAgent owner, string name) : base(owner, name)
	{
	}

	public override void OnEnter()
	{
		prevAngle = owner.perception.angle;
		prevDist = owner.perception.distance;
		owner.perception.angle = 180;
		owner.perception.distance = 10;
		owner.movement.Resume();
	}

	public override void OnExit()
	{
		owner.perception.angle = prevAngle;
		owner.perception.distance = prevDist;
	}

	public override void OnUpdate()
	{
		if (owner.Enemy == null) return;
		Vector3 direction = (owner.transform.position - owner.Enemy.transform.position).normalized;
		owner.movement.MoveTowards(owner.transform.position + direction);
	}
}

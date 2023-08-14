using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	public ChaseState(StateAgent owner, string name) : base(owner, name)
	{
	}

	public override void OnEnter()
	{
		owner.movement.Resume();
		//Debug.Log(name + " enter");
	}

	public override void OnExit()
	{
		//Debug.Log(name + " exit");
	}

	public override void OnUpdate()
	{
		//Debug.Log(name + " update");
		if (owner.Enemy != null) owner.movement.MoveTowards(owner.Enemy.transform.position);
	}
}

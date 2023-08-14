using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAgent : Agent
{
	[SerializeField] public Perception perception;
	public PathFollower pathFollower;
	public StateMachine stateMachine = new StateMachine();

	public BoolRef enemySeen;
	public BoolRef atDestination;
	public FloatRef health;
	public FloatRef timer;
	public FloatRef enemyDistance;

	public GameObject Enemy { get; set; }

	// Start is called before the first frame update
	void Start()
	{
		stateMachine.AddState(new IdleState(this, typeof(IdleState).Name));
		stateMachine.AddState(new PatrolState(this, typeof(PatrolState).Name));
		stateMachine.AddState(new ChaseState(this, typeof(ChaseState).Name));
		stateMachine.AddState(new DeathState(this, typeof(DeathState).Name));
		stateMachine.AddState(new AttackState(this, typeof(AttackState).Name));
		stateMachine.AddState(new EvadeState(this, typeof(EvadeState).Name));
		stateMachine.AddState(new SeekState(this, typeof(SeekState).Name));
		stateMachine.AddState(new RoamState(this, typeof(RoamState).Name));

		// From Idle
		stateMachine.AddTransition(typeof(IdleState).Name,
			new Transition(new FloatCondition(timer, Condition.Predicate.LESS, 0)),
			typeof(PatrolState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name,
			new Transition( new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name,
			new Transition(new BoolCondition(enemySeen, true), new FloatCondition(health, Condition.Predicate.GREATER, 30)),
			typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(IdleState).Name,
			new Transition(new BoolCondition(enemySeen, true), new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30)),
			typeof(EvadeState).Name);

		// From Chase
		stateMachine.AddTransition(typeof(ChaseState).Name,
			new Transition(new BoolCondition(enemySeen, false)),
			typeof(IdleState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name,
			new Transition(new FloatCondition(enemyDistance, Condition.Predicate.LESS_EQUAL, 1.5f)),
			typeof(AttackState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(ChaseState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30)),
			typeof(EvadeState).Name);

		// From Patrol
		stateMachine.AddTransition(typeof(PatrolState).Name,
			new Transition(new BoolCondition(enemySeen, true)),
			typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(PatrolState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(PatrolState).Name,
			new Transition(new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(RoamState).Name);

		// From Attack
		stateMachine.AddTransition(typeof(AttackState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(AttackState).Name,
			new Transition(new FloatCondition(timer, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(ChaseState).Name);
		stateMachine.AddTransition(typeof(AttackState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 30)),
			typeof(EvadeState).Name);

		// From Evade
		stateMachine.AddTransition(typeof(EvadeState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.GREATER, 30)),
			typeof(IdleState).Name);
		stateMachine.AddTransition(typeof(EvadeState).Name,
			new Transition(new BoolCondition(enemySeen, false)),
			typeof(IdleState).Name);
		stateMachine.AddTransition(typeof(EvadeState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);

		// From Roam
		stateMachine.AddTransition(typeof(RoamState).Name,
			new Transition(new FloatCondition(health, Condition.Predicate.LESS_EQUAL, 0)),
			typeof(DeathState).Name);
		stateMachine.AddTransition(typeof(RoamState).Name,
			new Transition(new BoolCondition(atDestination, true)),
			typeof(IdleState).Name);

		// Starting State
		stateMachine.SetState(stateMachine.StateFromName(typeof(IdleState).Name));
	}

	// Update is called once per frame
	void Update()
	{
		// Update Parameters
		var enemies = perception.GetGameObjects();
		enemySeen.value = enemies.Length != 0;
		Enemy = enemies.Length != 0 ? enemies[0] : null;
		enemyDistance.value = (Enemy != null) ? Vector3.Distance(transform.position, Enemy.transform.position) : float.MaxValue;

		timer.value -= Time.deltaTime;

		stateMachine.Update();

		animator.SetFloat("speed", movement.velocity.magnitude);
	}

	private void OnGUI()
	{
		Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

		GUI.Label(new Rect(screen.x, Screen.height - screen.y, 300, 20), stateMachine.GetStateName());
	}
}

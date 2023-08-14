using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] IntData fixedFPS;
	[SerializeField] IntData destroyCount;
	[SerializeField] StringData fps;
	[SerializeField] StringData collisionInfo;
	[SerializeField] EnumData broadPhaseType;
	[SerializeField] StringData bodyCount;
	[SerializeField] BoolData simulate;

	public List<Force> forces;

	private float timeAccumulator;
	public float FixedDeltaTime { get => 1.0f / fixedFPS.value; }

	BroadPhase[] broadPhases = { new Quadtree(), new BVH(), new NullBroadPhase() };

	public List<Body> bodies { get; set; } = new List<Body>();
	Camera activeCamera;

	BroadPhase broadPhase = new BVH();

	private void Start()
	{
		activeCamera = Camera.main;
	}

	private void Update()
	{

		fps.value = (1 / Time.deltaTime).ToString("F1");
		bodyCount.value = bodies.Count.ToString();

		//Everything after this will not run when simulate is false.
		if (!simulate.value) return;

		broadPhase = broadPhases[broadPhaseType.value];
		timeAccumulator += Time.deltaTime;
		forces.ForEach(force => force.ApplyForce(bodies));

		Vector2 screenSize = GetScreenSize();

		while (timeAccumulator >= FixedDeltaTime)
		{
			// construct broad-phase tree
			broadPhase.Build(new AABB(Vector2.zero, screenSize), bodies);
			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);


			Collision.CreateNarrowPhaseContacts(contacts);

			bodies.ForEach(body => body.shape.color = Color.white);
			//Collision.CreateContacts(bodies, out var contacts);

			Collision.SeparateContacts(contacts);
			Collision.ApplyImpulses(contacts);
			//contacts.ForEach(contact =>
			//{
			//	contact.bodyA.shape.color = Color.red;
			//	contact.bodyB.shape.color = Color.green;
			//});

			bodies.ForEach(b =>
			{
				Integrator.SemiImplicitEuler(b, FixedDeltaTime);
				b.Position = b.Position.Wrap(screenSize * -0.5f, screenSize * 0.5f);
				b.shape.GetAABB(b.Position).Draw(Color.white);
			});

			timeAccumulator -= FixedDeltaTime;
		}

		broadPhase.Draw();
		collisionInfo.value = broadPhase.queryResultCount + "/" + bodies.Count;

		foreach (Body b in bodies)
		{
			b.Acceleration = Vector3.zero;
		}

		for (int i = 0; i < bodies.Count; i++)
		{
			Body b = bodies[i];
			if (b.Position.y < -50 || b.Position.y > 50 || b.Position.x < -50 || b.Position.x > 50)
			{
				Destroy(b.gameObject);
				bodies.Remove(b);
			}
		}
	}

	public Vector2 GetScreenSize()
	{
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
	}

	internal Body GetScreenToBody(Vector3 screen)
	{
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

		if (hit.collider)
		{
			hit.collider.gameObject.TryGetComponent<Body>(out body);
		}

		return body;
	}

	public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector2 world = activeCamera.ScreenToWorldPoint(screen);
		return world;
	}

	public void DestroyAll()
	{
		bodies.ForEach(b => Destroy(b.gameObject));
		bodies.Clear();
	}

	public void DeleteFromStart()
	{
		if (destroyCount.value > bodies.Count) return;
		for (int i = 0; i < destroyCount.value; i++)
		{
			Destroy(bodies[0].gameObject);
			bodies.RemoveAt(0);
		}
	}

	public void DeleteFromEnd()
	{
		if (destroyCount.value > bodies.Count) return;
		for (int i = 0; i < destroyCount.value; i++)
		{
			Destroy(bodies[bodies.Count - 1].gameObject);
			bodies.RemoveAt(bodies.Count - 1);
		}
	}
}

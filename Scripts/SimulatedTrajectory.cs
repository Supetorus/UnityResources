using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulatedTrajectory:MonoBehaviour
{
	[SerializeField] private Transform _obstaclesParent;

	private Scene _simulationScene;
	private PhysicsScene _physicsScene;
	private List<KeyValuePair<Transform, Transform>> _nonStaticObjects = new List<KeyValuePair<Transform, Transform>>();

	private GameObject _ghostProjectile;
	private Rigidbody _projectileRB;

	internal void ResetScene(Transform obstacles)
	{
		_nonStaticObjects.Clear();
		Scene oldScene = _simulationScene;
		oldScene.name = oldScene.name + "-old";
		_obstaclesParent = obstacles;
		string sceneName = obstacles.gameObject.scene.name + "-Simulation";
		_simulationScene = SceneManager.CreateScene(sceneName, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
		_physicsScene = _simulationScene.GetPhysicsScene();
		if (_ghostProjectile != null) SceneManager.MoveGameObjectToScene(_ghostProjectile.gameObject, _simulationScene);

		if (oldScene.name != null) SceneManager.UnloadSceneAsync(oldScene);
		AddTransforms(_obstaclesParent);
	}

	public  void CalculateTrajectory(Transform ballSpawn, Vector3 velocity)
	{
		if (_simulationScene.name == null) return;

		// Update positions of obstacles.
		for (int i = 0; i < _nonStaticObjects.Count; i++)
		{
			if (_nonStaticObjects[i].Key == null)
			{
				Destroy(_nonStaticObjects[i].Value.gameObject);
				_nonStaticObjects.RemoveAt(i);
				continue;
			}
			_nonStaticObjects[i].Value.position = _nonStaticObjects[i].Key.position;
			_nonStaticObjects[i].Value.rotation = _nonStaticObjects[i].Key.rotation;
		}

		// Projectile setup
		_ghostProjectile.transform.position = ballSpawn.position;
		_ghostProjectile.transform.rotation = ballSpawn.rotation;
		_projectileRB.velocity = Vector3.zero;
		_projectileRB.AddForce(velocity, ForceMode.Impulse);

		//lineRenderer.positionCount = steps;

		//// Simulate!
		//for (var i = 0; i < steps; i++)
		//{
		//	_physicsScene.Simulate(Time.fixedDeltaTime);
		//	lineRenderer.SetPosition(i, _ghostProjectile.transform.position);
		//}
	}

	internal void RegisterProjectile(GameObject projectilePrefab)
	{
		if (_ghostProjectile != null) Destroy(_ghostProjectile);
		_ghostProjectile = Instantiate(projectilePrefab);
		SceneManager.MoveGameObjectToScene(_ghostProjectile, _simulationScene);
		foreach (Renderer renderer in _ghostProjectile.GetComponentsInChildren<Renderer>()) renderer.enabled = false;
		_projectileRB = _ghostProjectile.GetComponent<Rigidbody>();
	}

	private void AddTransforms(Transform t)
	{
		if (!t.gameObject.activeInHierarchy) return;

		if (0 == t.childCount)
		{
			var ghostObj = Instantiate(t.gameObject, t.position, t.rotation);
			if (ghostObj.TryGetComponent(out DropObjective drop)) Destroy(drop);
			if (ghostObj.TryGetComponent(out KeepUpObjective keep)) Destroy(keep);
			if (ghostObj.TryGetComponent(out Destructable des)) Destroy(des);
			if (ghostObj.TryGetComponent(out ImpactSound iS)) Destroy(iS);
			if (ghostObj.TryGetComponent(out AudioSource aS)) Destroy(aS);
			//foreach (Component c in ghostObj.GetComponents<Component>())
			//{
			//	if (c as Transform || c as Collider || c as Rigidbody) continue;
			//	Destroy(c);
			//}
			SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
			var renderers = ghostObj.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers) renderer.enabled = false;
			if (!ghostObj.isStatic) _nonStaticObjects.Add(new KeyValuePair<Transform, Transform>(t, ghostObj.transform));
			if (ghostObj.TryGetComponent(out Destructable destructable)) destructable.isGhost = true; // it's possible for the destructable to be called before the component is destroyed for some reason.
		}
		else
		{
			foreach (Transform child in t)
			{
				AddTransforms(child);
			}
		}
	}
}
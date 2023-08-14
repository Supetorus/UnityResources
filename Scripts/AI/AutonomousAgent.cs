using UnityEngine;

public class AutonomousAgent : Agent
{
	[SerializeField] Perception flockPerception;
	[SerializeField] Perception perception;
	[SerializeField] ObstaclePerception obstaclePerception;
	[SerializeField] Steering steering;
	[SerializeField] AutonomousAgentData agentData;

	// Update is called once per frame
	void Update()
	{
		GameObject[] perceivedObjects = perception.GetGameObjects();

		// Seek / Flee
		if (perceivedObjects.Length != 0)
		{
			movement.ApplyForce(steering.Seek(this, perceivedObjects[0]) * agentData.seekWeight);
			movement.ApplyForce(steering.Flee(this, perceivedObjects[0]) * agentData.fleeWeight);
		}

		// Flocking
		perceivedObjects = flockPerception.GetGameObjects();
		if (perceivedObjects.Length != 0)
		{
			movement.ApplyForce(steering.Cohesion(this, perceivedObjects) * agentData.cohesionWeight);
			movement.ApplyForce(steering.Separation(this, perceivedObjects, agentData.separationRadius) * agentData.separationWeight);
			movement.ApplyForce(steering.Alignment(this, perceivedObjects) * agentData.alignmentWeight);
		}

		// Obstacle Avoidance
		if (obstaclePerception.IsObstacleInFront())
		{
			Vector3 direction = obstaclePerception.GetOpenDirection();
			movement.ApplyForce(steering.CalculateSteering(this, direction) * agentData.obstacleWeight);
		}

		// Wander
		if (movement.acceleration.sqrMagnitude <= movement.maxForce * 0.1f){
			movement.ApplyForce(steering.Wander(this));
		}
	}
}

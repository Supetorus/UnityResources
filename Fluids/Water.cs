using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
public class Water : MonoBehaviour
{
	[System.Serializable]
	struct Wave
	{
		[Range(0, 10)] public float amplitude;
		[Range(0, 10)] public float length;
		[Range(0, 10)] public float rate;
	}

	[SerializeField][Range(1.0f, 90.0f)] float fps = 30;
	[SerializeField][Range(0.0f, 1.0f)] float damping = 0.04f;

	[Header("Waves")]
	[SerializeField] Wave wave1;
	[SerializeField] Wave wave2;

	[Header("Mesh Generator")]
	[SerializeField][Range(1.0f, 80.0f)] float xMeshSize = 40.0f;
	[SerializeField][Range(1.0f, 80.0f)] float zMeshSize = 40.0f;
	[SerializeField][Range(2, 80)] int xMeshVertexNum = 2;
	[SerializeField][Range(2, 80)] int zMeshVertexNum = 2;

	MeshFilter meshFilter;
	MeshCollider meshCollider;

	Mesh mesh;
	Vector3[] vertices;

	float time;
	int frame;

	float[,] buffer1;
	float[,] buffer2;

	float timeStep { get => 1.0f / fps; }

	float[,] previousBuffer { get => ((frame % 2) == 0) ? buffer1 : buffer2; }
	float[,] currentBuffer { get => ((frame % 2) == 0) ? buffer2 : buffer1; }

	bool isWave = true;

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();

		MeshGenerator.Plane(meshFilter, xMeshSize, zMeshSize, xMeshVertexNum, zMeshVertexNum);

		mesh = meshFilter.mesh;
		vertices = mesh.vertices;

		buffer1 = new float[xMeshVertexNum, zMeshVertexNum];
		buffer2 = new float[xMeshVertexNum, zMeshVertexNum];
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab)) isWave = !isWave;
		time += Time.deltaTime;
		while (time > timeStep)
		{
			if (isWave) UpdateWave(currentBuffer);
			else
			{
				frame++;
				UpdateSimulation(previousBuffer, currentBuffer, timeStep);
			}

			time -= timeStep;
		}

		// set vertices height from current buffer
		for (int x = 0; x < xMeshVertexNum; x++)
		{
			for (int z = 0; z < zMeshVertexNum; z++)
			{
				vertices[x + z * xMeshVertexNum].y = currentBuffer[x, z];
			}
		}

		// recalculate mesh with new vertices
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		meshCollider.sharedMesh = mesh;
	}

	void UpdateWave(float[,] buffer)
	{
		for (int x = 0; x < xMeshVertexNum; x++)
		{
			for (int z = 0; z < zMeshVertexNum; z++)
			{
				float t1 = x * wave1.length;
				float v1 = Mathf.Sin(t1 * wave1.length + Time.time * wave1.rate) * wave1.amplitude;

				float t2 = (x + z) * wave2.length;
				float v2 = Mathf.Sin(t2 * wave2.length + Time.time * wave2.rate) * wave2.amplitude;

				buffer[x, z] = v1 + v2;
			}
		}
	}

	void UpdateSimulation(float[,] previous, float[,] current, float dt)
	{
		for (int x = 0; x < xMeshVertexNum; x++)
		{
			for (int z = 0; z < zMeshVertexNum; z++)
			{
				// update buffer value 
				float left	= x - 1 < 0				? previous[x, z]: previous[x - 1, z];
				float right	= x == xMeshVertexNum-1	? previous[x, z]: previous[x + 1, z];
				float bottom	= z - 1 < 0				? previous[x, z]: previous[x, z - 1];
				float top = z == zMeshVertexNum-1	? previous[x, z]: previous[x, z + 1];
				float value = left + right + top + bottom;

				value = value * 0.5f;
				value = value - current[x, z];
				value = value * Mathf.Pow(damping, dt); // dampen value 

				current[x, z] = value;
			}
		}
	}

	public void Touch(Ray ray, float offset)
	{
		if (Physics.Raycast(ray, out RaycastHit raycastHit))
		{
			// check if ray cast hit this mesh
			MeshCollider meshCollider = raycastHit.collider as MeshCollider;
			if (meshCollider == this.meshCollider)
			{
				// get hit triangle
				int[] triangles = mesh.triangles;
				// get triangle index hit
				int index = triangles[raycastHit.triangleIndex * 3];
				// get x and z vertex
				int x = index % xMeshVertexNum;
				int z = index / xMeshVertexNum;

				if (x > 1 && x < xMeshVertexNum - 1 && z > 1 && z < zMeshVertexNum - 1)
				{
					currentBuffer[x, z] = offset;
				}
			}
		}
	}
}

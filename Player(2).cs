using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] float speed;

	// Start is called before the first frame update
	void Start()
	{

	}

	void Update()
	{
		Vector3 direction = Vector3.zero;

		direction.x = Input.GetAxis("Horizontal");
		direction.z = Input.GetAxis("Vertical");

		transform.position += direction * speed * Time.deltaTime;
		transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));
	}
}

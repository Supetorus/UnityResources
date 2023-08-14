using UnityEngine;
using UnityEngine.Events;

public class InventoryPickup : MonoBehaviour
{
	[SerializeField] private AudioClip clip;

	[SerializeField] private UnityEvent OnPickup;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Inventory>().Pickup(gameObject);
			AudioSource.PlayClipAtPoint(clip, transform.position);
			//Destroy(gameObject);
			gameObject.SetActive(false);
			OnPickup?.Invoke();
		}
	}
}

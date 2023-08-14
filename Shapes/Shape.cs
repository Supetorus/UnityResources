using UnityEngine;

public abstract class Shape : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteRenderer;
	public abstract float Size { get; set; }
	public abstract float Area { get; }

	public float Mass => Area * Density;
	public float Density { get; set; }

	public Color color { set => spriteRenderer.material.color = value; }

	public abstract AABB GetAABB(Vector2 position);
}

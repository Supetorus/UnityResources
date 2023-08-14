using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max)
    {
        Vector3 result = v;

        if (v.x > max.x) result.x = min.x + (max.x - v.x);
        else if (v.x < min.x) result.x = max.x - (min.x - v.x);

        if (v.y > max.y) result.y = min.y + (max.y - result.y);
        else if (v.y < min.y) result.y = max.y - (min.y - result.y);

        if (v.z > max.z) result.z = min.z + (max.z - result.z);
        else if (v.z < min.z) result.z = max.z - (min.z - result.z);

        return result;
    }

	public static bool ScreenToWorld(Vector2 screen, LayerMask layerMask, ref Vector3 position)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hitInfo, 100))
		{
			// check if hit layer mask
			if (((1 << hitInfo.collider.gameObject.layer) & layerMask) == 0) return false;

			position = hitInfo.point;
		}

		return true;
	}

	public static Vector3 SnapToGrid(Vector3 v, Vector3 grid)
	{
		v.x = (grid.x == 0) ? v.x : Mathf.RoundToInt(v.x / grid.x) * grid.x;
		v.y = (grid.y == 0) ? v.y : Mathf.RoundToInt(v.y / grid.y) * grid.y;
		v.z = (grid.z == 0) ? v.z : Mathf.RoundToInt(v.z / grid.z) * grid.z;

		return v;
	}
}

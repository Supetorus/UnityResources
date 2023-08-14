using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSegment : KinematicSegment
{
    public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float size)
    {
        this.parent = parent;
        this.size = size;
        this.angle = angle;
        this.length = length;

        start = position;
    }

    private void Update()
    {
        // scale segment
        transform.localScale = Vector2.one * size;
        // update rotation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Follow(Vector2 target)
    {
        // compute direction (target <- start) with segment length
        var direction = (target - start).normalized * length;
        
        // convert direction cartesian to polar
        polar = Polar.CartesianToPolar(direction);

        // set start to target - direction
        start = target - direction;
    }
}

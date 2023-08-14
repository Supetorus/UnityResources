using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private FloatData healthData;
    [SerializeField] private HealthUI healthUI;

    public override float CurrentH {
        get { return healthData.value; }
        protected set { healthData.value = value; }
    }

	protected override void Start() {
        healthUI.Change(CurrentH);
    }

	public override void TakeDamage(float damage, Vector3 origin, float knockbackPower)
	{
		base.TakeDamage(damage, origin, knockbackPower);
        healthUI.Change(CurrentH);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOnEnable : MonoBehaviour
{
	private void Start()
	{
		this.GetComponent<Button>().Select();
	}
	private void OnEnable()
	{
		this.GetComponent<Button>().Select();
	}
}

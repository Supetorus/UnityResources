using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteCountDisplay : MonoBehaviour
{
	[SerializeField, Tooltip("The current number of sprites to be shown.")] IntData counter;
	int shownSprites;

	private void Awake()
	{
		shownSprites = (int)counter.max;
	}

	private void Update()
	{
		if (shownSprites < counter.value)
		{
			for (int i = shownSprites; i < counter.value; i++)
			{
				transform.GetChild(i - 1).gameObject.SetActive(true);
				shownSprites++;
			}
		}
		else if (shownSprites > counter.value)
		{
			for (int i = shownSprites; i > counter.value; i--)
			{
				transform.GetChild(i - 1).gameObject.SetActive(false);
				shownSprites--;
			}
		}
	}
}

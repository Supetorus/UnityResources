using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
	[SerializeField] StringData p1Name;
	[SerializeField] StringData p2Name;
	[SerializeField] StringData p3Name;
	[SerializeField] StringData p4Name;

	[SerializeField] TMP_Text p1NameField;
	[SerializeField] TMP_Text p2NameField;
	[SerializeField] TMP_Text p3NameField;
	[SerializeField] TMP_Text p4NameField;

	[SerializeField] EnumData p1Color;
	[SerializeField] EnumData p2Color;
	[SerializeField] EnumData p3Color;
	[SerializeField] EnumData p4Color;

	[SerializeField] UnityEvent success;
	[SerializeField] UnityEvent failure;

	public void DoIt()
	{
		p1Name.value = p1NameField.text;
		p2Name.value = p2NameField.text;
		p3Name.value = p3NameField.text;
		p4Name.value = p4NameField.text;

		List<int> picked = new List<int>();
		picked.Add(p1Color.value);
		bool succeeded = true;
		if (!picked.Contains(p2Color.value)) picked.Add(p2Color.value);
		else
		{
			failure.Invoke();
			succeeded = false;
		}

		if (!picked.Contains(p3Color.value)) picked.Add(p3Color.value);
		else
		{
			failure.Invoke();
			succeeded = false;
		}

		if (!picked.Contains(p4Color.value)) picked.Add(p4Color.value);
		else
		{
			failure.Invoke();
			succeeded = false;
		}

		if (succeeded) success.Invoke();
	}
}

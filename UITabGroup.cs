using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabGroup : MonoBehaviour
{
	public List<UITabButton> tabButtons = new List<UITabButton>();
	public UITabButton defaultSelected;

	public Color idleColor;
	public Color highlightColor;
	public Color selectedColor;

	UITabButton selectedTabButton;

	private void Start()
	{
		if (defaultSelected != null)
		{
			OnTabSelected(defaultSelected);
		}
	}

	public void Subscribe(UITabButton button)
	{
		tabButtons.Add(button);
	}

	public void OnTabEnter(UITabButton button)
	{
		ResetTabs();
		if (selectedTabButton == null || button != selectedTabButton)
		{
			button.color = highlightColor;
		}
	}

	public void OnTabExit(UITabButton button)
	{
		ResetTabs();
	}

	public void OnTabSelected(UITabButton button)
	{
		selectedTabButton = button;
		ResetTabs();
		button.color = selectedColor;

		foreach (var tabButton in tabButtons)
		{
			tabButton.active = (tabButton == button);
		}
	}

	public void ResetTabs()
	{
		foreach (var tabButton in tabButtons)
		{
			if (selectedTabButton != null && tabButton == selectedTabButton) continue;
			tabButton.color = idleColor;
		}
	}

}

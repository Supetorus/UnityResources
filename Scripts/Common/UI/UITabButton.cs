using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
	[SerializeField] UITabGroup tabGroup;
	[SerializeField] Image image;
	[SerializeField] GameObject panel;

	public Color color { set => image.color = value; }
	public bool active { set => panel.SetActive(value); }

	public void OnPointerClick(PointerEventData eventData)
	{
		tabGroup.OnTabSelected(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tabGroup.OnTabEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tabGroup.OnTabExit(this);
	}
}

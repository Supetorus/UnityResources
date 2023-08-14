using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public int betValue;
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] GameObject parent;
    [SerializeField] bool respawn = true;
    [SerializeField] PokerLogic logic;
    [SerializeField] string clearTag;
    
    private Vector2 dragOffset;
    private CanvasGroup canvasGroup;
    private bool valueAdded = false;

    public void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void Update()
    {
        if (playerInfo.chipBalance < betValue && !valueAdded)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
        }
        else if (!valueAdded)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = eventData.position - (Vector2)transform.position;

        if (respawn)
        {
            Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation, parent.transform);
            respawn = false;
        }

        if (!valueAdded)
        {
            logic.AddBet(betValue);
            valueAdded = true;
        }

        gameObject.tag = clearTag;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void ClearChips()
    {
        GameObject[] chips = GameObject.FindGameObjectsWithTag(clearTag);
        foreach (var chip in chips)
        {
            Destroy(chip);
        }
    }
}
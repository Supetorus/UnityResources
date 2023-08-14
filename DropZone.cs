using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] int betValue; 
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] bool destroyOnDrop = false;
    [SerializeField] PokerLogic logic;


    public void OnDrop(PointerEventData eventData)
    {
        DragNDrop chip = eventData.pointerDrag.GetComponent<DragNDrop>();

        if (destroyOnDrop)
        {
            Destroy(eventData.pointerDrag);
        }
        playerInfo.chipBalance += chip.betValue;
        logic.SubtractBet(chip.betValue);

        Debug.Log(playerInfo.chipBalance);
    }
}

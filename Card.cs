using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum eValue
    {
        ACE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK,
        QUEEN,
        KING
    }

    public enum eSuit
    {
        HEARTS,
        SPADES,
        DIAMONDS,
        CLOVERS
    }

    public eSuit suit = eSuit.SPADES;
    public eValue value = eValue.ACE;

    public bool isFaceUp = true;

    public Sprite defaultSprite;
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokerLogic : MonoBehaviour
{
    public CardManager cardManager;
    public PlayerInfo playerInfo;
    public List<Card> playerDebugHand;

    [Header("Displays")]
    public TMP_Text playerAvailableChips;
    public TMP_Text playerAvailableBalance;
    public TMP_Text playerBetValue;
    public TMP_Text playerHandText;
    private List<Card> playerHand = new List<Card>();

    [Header("Locations")]
    public GameObject playerHandLocation;

    private int playerBet = 0;
    private int playerScore = 0;

    private enum GameState
    {
        Betting,
        FirstDraw,
        Redraw,
        GameOver
    }

    private GameState gameState;

    private void Start()
    {
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();

        gameState = GameState.Betting;
    }

    /// <summary>
    /// Shuffles the deck and deals five cards to player. Called when the "Bet" button is clicked
    /// </summary>
    public void Deal()
    {
        if (gameState != GameState.Betting) return;
        gameState = GameState.FirstDraw;
        cardManager.Shuffle();
        cardManager.Deal(5, true, playerHand);

        foreach (Transform card in playerHandLocation.transform) Destroy(card.gameObject);
        if (playerDebugHand.Count == 5) playerHand = playerDebugHand;

        DisplayCards();
    }

    /// <summary>
    /// Updates the card displays and value display
    /// </summary>
    public void DisplayCards()
    {
        if (playerHand.Count < 0) return;
        else if(playerHand.Count > 0) DoesPLayerWin();

        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);

        //GameOver();
    }

    public void DoesPLayerWin()
    {
        playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Poker);
        if (playerScore == 0)
        {
            playerHandText.text = "No win";
        }
        else if (playerScore == 1)
        {
            playerHandText.text = "Pair";
        }
        else if (playerScore == 2)
        {
            playerHandText.text = "Two Pair";
        }
        else if (playerScore == 3)
        {
            playerHandText.text = "Three of a Kind";
        }
        else if (playerScore == 4)
        {
            playerHandText.text = "Straight";
        }
        else if (playerScore == 5)
        {
            playerHandText.text = "Flush";
        }
        else if (playerScore == 6)
        {
            playerHandText.text = "Full House";
        }
        else if (playerScore == 7)
        {
            playerHandText.text = "Four of a Kind";
        }
        else if (playerScore == 8)
        {
            playerHandText.text = "Straight Flush";
        }
        else if (playerScore == 9)
        {
            playerHandText.text = "Royal Flush";
        }
    }

    public void DisplayMoney()
    {
        playerBetValue.text = playerBet.ToString();
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();
    }

    public void ResetGame()
    {
        playerHand.Clear();
        playerBet = 0;
        playerHandText.text = "No Win";

        DisplayCards();
        //DisplayMoney();
        gameState = GameState.Betting;
    }

    public void ResetButton()
    {
        if (gameState != GameState.FirstDraw) return;
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Poker);
        if (playerScore == 0)
        {
            playerBet = 0;
        }
        else if (playerScore == 1)
        {
            playerInfo.chipBalance += playerBet;
            playerBet = 0;
        }
        else if (playerScore == 2)
        {
            playerInfo.chipBalance += playerBet * 5;
            playerBet = 0;
        }
        else if (playerScore == 3)
        {
            playerInfo.chipBalance += playerBet * 10;
            playerBet = 0;
        }
        else if (playerScore == 4)
        {
            playerInfo.chipBalance += playerBet * 25;
            playerBet = 0;
        }
        else if (playerScore == 5)
        {
            playerInfo.chipBalance += playerBet * 50;
            playerBet = 0;
        }
        else if (playerScore == 6)
        {
            playerInfo.chipBalance += playerBet * 100;
            playerBet = 0;
        }
        else if (playerScore == 7)
        {
            playerInfo.chipBalance += playerBet * 1000;
            playerBet = 0;
        }
        else if (playerScore == 8)
        {
            playerInfo.chipBalance += playerBet * 10000;
            playerBet = 0;
        }
        else if (playerScore == 9)
        {
            playerInfo.chipBalance += playerBet * 100000;
            playerBet = 0;
        }

        //DisplayCards();
        DisplayMoney();
        ResetGame();
    }

    public void AddBet(int bet)
    {
        if (gameState != GameState.Betting || playerInfo.chipBalance < bet) return;
        if (playerInfo.chipBalance > bet)
        {
            playerBet += bet;
            playerInfo.chipBalance -= bet;
            DisplayMoney();
        }
    }

    public void SubtractBet(int bet)
    {
        if (gameState != GameState.Betting || playerInfo.chipBalance < bet) return;

        playerBet -= bet;
        playerInfo.chipBalance += bet;
        DisplayMoney();
    }
}

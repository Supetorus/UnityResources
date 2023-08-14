using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackjackManager : MonoBehaviour
{
	public CardManager cardManager;
	public PlayerInfo playerInfo;
	public int minBet = 2;
	public Sprite faceDownCard;
	public bool debugMode = false;
	public List<Card> playerDebugHand;
	public List<Card> dealerDebugHand;

	[Header("Displays")]
	public TMP_Text playerAvailableChips;
	public TMP_Text playerAvailableBalance;
	public TMP_Text playerBetValue;
	public TMP_Text playerSplitBetValue;
	private List<Card> dealerHand = new List<Card>();
	private List<Card> playerHand = new List<Card>();
	private List<Card> playerSplitHand = new List<Card>();
	public TMP_Text playerHandText;
	public TMP_Text playerSplitHandText;
	public TMP_Text dealerHandText;

	[Header("Locations")]
	public GameObject playerHandLocation;
	public GameObject playerSplitHandLocation;
	public GameObject dealerHandLocation;
	private Vector2 splitHandIndicatorLocation1 = new Vector2(415, -175);
	private Vector2 splitHandIndicatorLocation2 = new Vector2(415, -403);

	[Header("Objects")]
	public GameObject splitButton;
	public GameObject doubleButton;
	public GameObject gameEndPanel;
	public GameObject splitHandIndicator;

	private int playerBet = 0;
	private int playerSplitBet = 0; //todo fully implement playersplitbet
	private bool isSplitHand = false;
	private int playerScore = 0;
	private int playerSplitScore = 0;
	private int dealerScore = 0;

	private enum GameState
	{
		Betting,
		Playing,
		GameOver,
	}

	private GameState gameState;

	private void Start()
	{
		DisplayMoney();

		gameState = GameState.Betting;
		playerSplitHandLocation.SetActive(false);
		splitHandIndicator.SetActive(false);
		gameEndPanel.SetActive(false);
	}

	/// <summary>
	/// Called by deal button. Shuffles the deck and deals two cards to player and dealer. Dealer has one card face down.
	/// </summary>
	public void Deal()
	{
		if (gameState != GameState.Betting || playerBet < minBet) return;
		gameState = GameState.Playing;

		// Deal cards
		cardManager.Shuffle();
		if (debugMode)
		{
			playerHand = playerDebugHand;
			dealerHand = dealerDebugHand;
		}
		else
		{
			cardManager.Deal(2, true, playerHand, dealerHand);
		}
		dealerHand[0].isFaceUp = false;

		// Clear visual hand
		foreach (Transform card in playerHandLocation.transform) Destroy(card.gameObject);

		if (playerInfo.chipBalance < playerBet)
		{
			splitButton.SetActive(false);
			doubleButton.SetActive(false);
		}

		if (playerHand[0].value != playerHand[1].value)
		{
			splitButton.SetActive(false);
		}
		else
		{
			splitButton.SetActive(true);
		}

		CheckBlackjack();
		DisplayCards();
	}

	/// <summary>
	/// Doubles the current bet, and deals a card to the player, then the player stands
	/// Option is not available if chip balance is less than current bet
	/// </summary>
	public void DoubleDown()
	{
		playerInfo.chipBalance -= playerBet;
		if (isSplitHand)
		{
			playerSplitBet *= 2;
			cardManager.Deal(1, false, playerSplitHand);
		}
		else
		{
			playerBet *= 2;
			playerHand.Add(cardManager.GetCard());
		}
		PlayerStand();
	}

	/// <summary>
	/// Splits the player's hand into 2, places the current bet on the new hand, and deals a card to each hand
	/// Option is not available if chip balance is less than current bet
	/// </summary>
	public void Split()
	{
		playerSplitHandLocation.SetActive(true);
		playerSplitHand.Add(playerHand[1]);
		playerHand.RemoveAt(1);

		playerInfo.chipBalance -= playerBet;
		playerSplitBet = playerBet;

		cardManager.Deal(1, false, playerHand, playerSplitHand);
		DisplayCards();
		CheckBlackjack();

		isSplitHand = true;

		splitHandIndicator.SetActive(true);
		splitHandIndicator.GetComponent<RectTransform>().localPosition = splitHandIndicatorLocation1;

		if (playerInfo.chipBalance < playerBet)
		{
			doubleButton.SetActive(false);
		}
	}

	/// <summary>
	/// Updates the card displays and value display
	/// </summary>
	public void DisplayCards()
	{
		if (playerHand.Count < 0) return;

		// Player hand
		playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack);
		playerHandText.text = playerScore.ToString();

		if (playerScore == 21) playerHandText.color = Color.green;
		else if (playerScore > 21) playerHandText.color = Color.red;
		else playerHandText.color = Color.white;

		// Player's Split hand
		playerSplitScore = cardManager.CalculateHandValue(playerSplitHand, CardManager.CardRules.Blackjack);
		playerSplitHandText.text = playerSplitScore.ToString();

		if (playerSplitScore == 21) playerSplitHandText.color = Color.green;
		else if (playerSplitScore > 21) playerSplitHandText.color = Color.red;
		else playerSplitHandText.color = Color.white;

		// Dealer's hand
		dealerScore = cardManager.CalculateHandValue(dealerHand, CardManager.CardRules.Blackjack);
		dealerHandText.text = dealerScore == 0 ? "" : dealerScore.ToString();

		if (dealerScore == 21) dealerHandText.color = Color.green;
		else if (dealerScore > 21) dealerHandText.color = Color.red;
		else dealerHandText.color = Color.white;

		foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
		foreach (Transform c in playerSplitHandLocation.transform) Destroy(c.gameObject);
		foreach (Transform c in dealerHandLocation.transform) Destroy(c.gameObject);
		foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);
		foreach (Card c in playerSplitHand) Instantiate(c.gameObject, playerSplitHandLocation.transform);
		foreach (Card c in dealerHand)
		{
			Card newCard = Instantiate(c.gameObject, dealerHandLocation.transform).GetComponent<Card>();
			if (newCard.isFaceUp) newCard.GetComponent<Image>().sprite = newCard.defaultSprite;
			else newCard.GetComponent<Image>().sprite = faceDownCard;
		}
	}

	/// <summary>
	/// Updates the chips and money displays.
	/// </summary>
	public void DisplayMoney()
	{
		playerBetValue.text = playerBet.ToString();
		playerAvailableChips.text = playerInfo.chipBalance.ToString();
		playerAvailableBalance.text = playerInfo.bankBalance.ToString();
	}

	/// <summary>
	/// Method called when player clicks "Hit" button.
	/// </summary>
	public void PlayerHit()
	{
		if (gameState != GameState.Playing) return;

		if (isSplitHand)
		{
			cardManager.Deal(1, false, playerSplitHand);
			DisplayCards();
			if (cardManager.CalculateHandValue(playerSplitHand, CardManager.CardRules.Blackjack) >= 21)
			{
				if (playerInfo.chipBalance >= playerBet && playerInfo.chipBalance >= minBet)
				{
					doubleButton.SetActive(true);
				}
				isSplitHand = false;
				splitHandIndicator.GetComponent<RectTransform>().localPosition = splitHandIndicatorLocation2;
				return;
			}
		}
		else
		{
			cardManager.Deal(1, false, playerHand);
			if (cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack) >= 21) StartCoroutine(GameOver());
			DisplayCards();
		}
	}

	private void CheckBlackjack()
	{
		if (cardManager.CalculateHandValue(playerSplitHand, CardManager.CardRules.Blackjack) == 21)
		{
			isSplitHand = false;
		}

		if (cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack) == 21)
		{
			dealerHand[0].isFaceUp = true;
			StartCoroutine(GameOver());
		}
	}

	public void ResetGame()
	{
		playerSplitHandLocation.SetActive(false);
		splitHandIndicator.SetActive(false);
		isSplitHand = false;
		playerHand.Clear();
		playerSplitHand.Clear();
		dealerHand.Clear();
		playerBet = 0;
		playerSplitBet = 0;

		DisplayCards();
		DisplayMoney();
		gameState = GameState.Betting;
	}

	/// <summary>
	/// The method called when player clicks "Stand" button.
	/// </summary>
	public void PlayerStand()
	{
		if (gameState != GameState.Playing) return;

		if (isSplitHand)
		{
			if (playerInfo.chipBalance >= playerBet)// && playerInfo.chipBalance >= minBet)
			{
				doubleButton.SetActive(true);
			}
			isSplitHand = false;
			splitHandIndicator.GetComponent<RectTransform>().localPosition = splitHandIndicatorLocation2;
		}
		else
		{
			DealerPlay();
		}
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

	public void DealerPlay()
	{
		gameState = GameState.GameOver;
		dealerHand[0].isFaceUp = true;
		playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack);
		dealerScore = cardManager.CalculateHandValue(dealerHand, CardManager.CardRules.Blackjack);
		while (dealerScore < 17 || dealerScore < playerScore)
		{
			cardManager.Deal(1, false, dealerHand);
			dealerScore = cardManager.CalculateHandValue(dealerHand, CardManager.CardRules.Blackjack);
		}

		StartCoroutine(GameOver());
	}

	private IEnumerator GameOver()
	{
		int playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack);
		int dealerScore = cardManager.CalculateHandValue(dealerHand, CardManager.CardRules.Blackjack);

		if (playerScore == 21) // player has 21
		{
			if (dealerScore == 21) playerInfo.chipBalance += playerBet;
			else playerInfo.chipBalance += playerBet * 3;
		}
		else if (dealerScore > 21)
		{
			playerInfo.chipBalance += playerBet * 2;
		}
		else if (dealerScore < 21 && playerScore < 21)
		{
			if (playerScore > dealerScore) playerInfo.chipBalance += playerBet * 2;
			else if (dealerScore == playerScore) playerInfo.chipBalance += playerBet;
		}

		DisplayCards();
		DisplayMoney();

		yield return new WaitForSeconds(2);

		gameEndPanel.SetActive(true);
	}
}

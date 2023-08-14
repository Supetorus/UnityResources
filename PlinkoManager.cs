using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlinkoManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    public TMP_Text playerAvailableChips;

    public TMP_Text playerAvailableBalance;

    public TMP_Text playerBetValue;

    public GameObject gameEndPanel;

    public int minBet = 2;

    public int playerBet = 0;

    private enum GameState
    {
        Betting,
        Playing,
        GameOver,
    }

    private GameState gameState;

    private void Start()
    {
        playerBet = 0;
        UpdateDisplay();
    }

    // Update is called once per frame
    void UpdateDisplay()
    {
        playerBetValue.text = playerBet.ToString();
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();
    }

    public void AddBet(int value)
    {
        if (playerBet + value <= playerInfo.chipBalance)
        {
            playerBet += value;
            UpdateDisplay();
        }
    }

    public void Score(int multiplier)
    {
        playerInfo.chipBalance += playerBet * multiplier;
        playerBet = 0;
        UpdateDisplay();
    }

    public void DropBall()
    {
        playerInfo.chipBalance -= playerBet;
        UpdateDisplay();
    }
}

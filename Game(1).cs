using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
	public GameObject row;
	public GameObject image;
	public GameObject button;
	public Transform buttonsPanel;
	private List<Row> rows = new List<Row>();
	private List<GameObject> buttons = new List<GameObject>();
	[SerializeField] GameObject gameOver;

	private int rowIndex = -1;
	private int rowsCount;

	private int difficultyHolder;

	private string player1;
	private string player2;
	[SerializeField] TextMeshProUGUI nameInput1;
	[SerializeField] TextMeshProUGUI nameInput2;
	[SerializeField] TextMeshProUGUI playerTurn;
	[SerializeField] TextMeshProUGUI winner;
	[SerializeField] TextMeshProUGUI loser;
	private int turn = 0;
	string[] players = new string[2];

	private void Awake()
	{
		foreach (Button b in buttonsPanel.GetComponentsInChildren<Button>())
		{
			b.gameObject.SetActive(false);
			buttons.Add(b.gameObject);
		}
	}

	public void SetNames()
    {
		player1 = "";
		player2 = "";

		player1 += nameInput1.text.Trim();
		player2 += nameInput2.text.Trim();

		if (string.IsNullOrEmpty(player1) || player1 == "​")
		{
			player1 = "Kyle";
		}
		if (string.IsNullOrEmpty(player2) || player2 == "​")
		{
			player2 = "Kylee";
		}

		Debug.Log(player1);
		Debug.Log(player2);

		players[0] = player1;
		players[1] = player2;
    }

	public void NewGame()
	{
		StartGame(difficultyHolder);
	}

	public void StartGame(int difficulty)
	{
		difficultyHolder = difficulty;
		ResetGame();

		switch (difficulty)
		{
			default:
			case 0:
				SetUpRow(1);
				SetUpRow(3);
				SetUpRow(5);
				rowsCount = 3;
				for (int i = 0; i < 3; ++i) { buttons[i].SetActive(true); }
				break;
			case 1:
				SetUpRow(1);
				SetUpRow(3);
				SetUpRow(5);
				SetUpRow(7);
				rowsCount = 4;
				for (int i = 0; i < 4; ++i) { buttons[i].SetActive(true); }
				break;
			case 2:
				SetUpRow(3);
				SetUpRow(5);
				SetUpRow(7);
				SetUpRow(9);
				SetUpRow(11);
				rowsCount = 5;
				for (int i = 0; i < 5; ++i) { buttons[i].SetActive(true); }
				break;
		}
		turn = Random.Range(0, 1);
		playerTurn.text = players[turn];
	}

	private void ResetGame()
	{
		foreach (Row row in rows)
		{
			Destroy(row.gameObject);
		}

		foreach (GameObject b in buttons)
		{
			b.SetActive(false);
		}

		rows.Clear();
	}

	private void SetUpRow(int count)
	{
		Row newRow = Instantiate(row, transform).GetComponent<Row>();

		for (int i = 0; i < count; ++i)
		{
			newRow.images.Add(Instantiate(image, newRow.transform));
		}

		rows.Add(newRow);
	}

	public void EndTurn()
	{
        if (rowIndex == -1)
        {
			return;
        }

		turn = (turn + 1) % 2;
		playerTurn.text = players[turn];
        Debug.Log(turn);
		rowIndex = -1;
	}

	public void Remove(int index)
	{
		if (rowIndex == -1) { rowIndex = index; }

		if (rowIndex == index && rows[index].Remove())
		{
			rows[index].gameObject.SetActive(false);
			buttons[index].gameObject.SetActive(false);
			if(--rowsCount == 0)
			{
				//game end
				gameOver.SetActive(true);
				winner.text = players[(turn + 1) % 2] + " is the best";
				loser.text = players[turn] + " is a loser";
				transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void OnApplicationQuit()
	{
		Application.Quit();
	}
}

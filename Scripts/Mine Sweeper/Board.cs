using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Board : MonoBehaviour
{
	public struct Tile
	{
		public int adjacentMineCount;
		public int adjacentFlagCount;
		public bool isMine;
		public bool isCleared;
		public bool isFlagged;
		public bool isSafe;
	}

	public int m_Width { get; private set; }
	public int m_Height { get; private set; }
	public Tile[,] m_Tiles { get; private set; }
	public float m_TileSize { get; private set; }
	public UnityEvent m_OnLose { get; private set; }
	public UnityEvent m_OnWin { get; private set; }
	public UnityEvent m_OnStart { get; private set; }

	private System.Random rng;
	private int m_TileCount;
	private int m_ClearedTileCount;
	private int m_MineTileCount;
	private bool m_FirstClick;

	private void Awake()
	{
		RectTransform t = GetComponent<RectTransform>();

		m_OnLose = new UnityEvent();
		m_OnLose.AddListener(Detonate);
		m_OnWin = new UnityEvent();
		m_OnStart = new UnityEvent();
		rng = new System.Random();

		m_TileSize = t.sizeDelta.x / 8.0f;

		m_Width = (int)(t.sizeDelta.x / m_TileSize);
		m_Height = (int)(t.sizeDelta.y / m_TileSize);
		m_TileCount = m_Width * m_Height;
		m_Tiles = new Tile[m_Width, m_Height];

		m_MineTileCount = m_Width * m_Height / 5;
		m_ClearedTileCount = 0;
		m_FirstClick = true;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.W))
		{
			for (int x = 0; x < m_Width; ++x)
			{
				for (int y = 0; y < m_Height; ++y)
				{
					m_Tiles[x, y].isCleared = !m_Tiles[x, y].isMine;
				}
			}

			m_ClearedTileCount = m_TileCount - m_MineTileCount;
			m_OnWin.Invoke();
		}
#endif
	}

	private void GenerateTiles()
	{
		int mineCount = m_MineTileCount;

		while (mineCount > 0)
		{
			int x = rng.Next(0, m_Width);
			int y = rng.Next(0, m_Height);

			if (!m_Tiles[x, y].isMine && !m_Tiles[x, y].isSafe)
			{
				m_Tiles[x, y].isMine = true;
				--mineCount;

				if (x > 0)
				{
					++m_Tiles[x - 1, y].adjacentMineCount;
					if (y > 0) { ++m_Tiles[x - 1, y - 1].adjacentMineCount; }
					if (y < m_Height - 1) { ++m_Tiles[x - 1, y + 1].adjacentMineCount; }
				}

				if (x < m_Width - 1)
				{
					++m_Tiles[x + 1, y].adjacentMineCount;
					if (y > 0) { ++m_Tiles[x + 1, y - 1].adjacentMineCount; }
					if (y < m_Height - 1) { ++m_Tiles[x + 1, y + 1].adjacentMineCount; }
				}

				if (y > 0) { ++m_Tiles[x, y - 1].adjacentMineCount; }
				if (y < m_Height - 1) { ++m_Tiles[x, y + 1].adjacentMineCount; }
			}
		}
	}

	public void ClickTile(int x, int y, out bool cascade, out int adjecentMines)
	{
		if (m_FirstClick)
		{
			m_OnStart.Invoke();
			m_FirstClick = false;
			m_Tiles[x, y].isSafe = true;

			if (x > 0)
			{
				m_Tiles[x - 1, y].isSafe = true;
				if (y > 0) { m_Tiles[x - 1, y - 1].isSafe = true; }
				if (y < m_Height - 1) { m_Tiles[x - 1, y + 1].isSafe = true; }
			}

			if (x < m_Width - 1)
			{
				m_Tiles[x + 1, y].isSafe = true;
				if (y > 0) { m_Tiles[x + 1, y - 1].isSafe = true; }
				if (y < m_Height - 1) { m_Tiles[x + 1, y + 1].isSafe = true; }
			}

			if (y > 0) { m_Tiles[x, y - 1].isSafe = true; }
			if (y < m_Height - 1) { m_Tiles[x, y + 1].isSafe = true; }

			GenerateTiles();
		}

		if (m_Tiles[x, y].isCleared && m_Tiles[x, y].adjacentMineCount > 0 && m_Tiles[x, y].adjacentMineCount == m_Tiles[x, y].adjacentFlagCount)
		{
			Stack<Vector2Int> tiles = new Stack<Vector2Int>();
			tiles.Push(new Vector2Int(x - 1, y));
			tiles.Push(new Vector2Int(x + 1, y));
			tiles.Push(new Vector2Int(x, y - 1));
			tiles.Push(new Vector2Int(x, y + 1));
			tiles.Push(new Vector2Int(x - 1, y - 1));
			tiles.Push(new Vector2Int(x - 1, y + 1));
			tiles.Push(new Vector2Int(x + 1, y - 1));
			tiles.Push(new Vector2Int(x + 1, y + 1));
			Cascade(tiles, out cascade, out adjecentMines);
		}
		else
		{
			Stack<Vector2Int> tiles = new Stack<Vector2Int>();
			tiles.Push(new Vector2Int(x, y));
			Cascade(tiles, out cascade, out adjecentMines);
		}
	}

	private void Cascade(Stack<Vector2Int> tiles, out bool cascade, out int adjacentMines)
	{
		cascade = false;
		adjacentMines = 0;

		while (tiles.Count > 0)
		{
			Vector2Int pos = tiles.Pop();

			if (pos.x >= 0 && pos.y >= 0 && pos.x < m_Width && pos.y < m_Height && !m_Tiles[pos.x, pos.y].isCleared)
			{
				if (m_Tiles[pos.x, pos.y].isMine && !m_Tiles[pos.x, pos.y].isFlagged)
				{
					m_OnLose.Invoke();
					return;
				}
				else if(!m_Tiles[pos.x, pos.y].isMine)
				{
					m_Tiles[pos.x, pos.y].isCleared = true;
					m_Tiles[pos.x, pos.y].isFlagged = false;
					++m_ClearedTileCount;
					adjacentMines = Mathf.Max(m_Tiles[pos.x, pos.y].adjacentMineCount, adjacentMines);

					if (m_Tiles[pos.x, pos.y].adjacentMineCount == 0)
					{
						cascade = true;
						tiles.Push(new Vector2Int(pos.x - 1, pos.y));
						tiles.Push(new Vector2Int(pos.x + 1, pos.y));
						tiles.Push(new Vector2Int(pos.x, pos.y - 1));
						tiles.Push(new Vector2Int(pos.x, pos.y + 1));
						tiles.Push(new Vector2Int(pos.x - 1, pos.y - 1));
						tiles.Push(new Vector2Int(pos.x + 1, pos.y + 1));
						tiles.Push(new Vector2Int(pos.x - 1, pos.y + 1));
						tiles.Push(new Vector2Int(pos.x + 1, pos.y - 1));
					}
				}
			}
		}

		if (m_ClearedTileCount == m_TileCount - m_MineTileCount)
		{
			m_OnWin.Invoke();
		}
	}

	public void ToggleFlag(int x, int y, out bool placed)
	{
		placed = false;

		if (!m_Tiles[x, y].isCleared)
		{
			m_Tiles[x, y].isFlagged = !m_Tiles[x, y].isFlagged;
			placed = m_Tiles[x, y].isFlagged;

			int change = m_Tiles[x, y].isFlagged ? 1 : -1;

			if (x > 0)
			{
				m_Tiles[x - 1, y].adjacentFlagCount += change;
				if (y > 0) { m_Tiles[x - 1, y - 1].adjacentFlagCount += change; }
				if (y < m_Height - 1) { m_Tiles[x - 1, y + 1].adjacentFlagCount += change; }
			}

			if (x < m_Width - 1)
			{
				m_Tiles[x + 1, y].adjacentFlagCount += change;
				if (y > 0) { m_Tiles[x + 1, y - 1].adjacentFlagCount += change; }
				if (y < m_Height - 1) { m_Tiles[x + 1, y + 1].adjacentFlagCount += change; }
			}

			if (y > 0) { m_Tiles[x, y - 1].adjacentFlagCount += change; }
			if (y < m_Height - 1) { m_Tiles[x, y + 1].adjacentFlagCount += change; }
		}
	}

	public void ResetBoard()
	{
		m_FirstClick = true;
		m_ClearedTileCount = 0;

		for (int x = 0; x < m_Width; ++x)
		{
			for (int y = 0; y < m_Height; ++y)
			{
				m_Tiles[x, y].isCleared = false;
				m_Tiles[x, y].isMine = false;
				m_Tiles[x, y].isFlagged = false;
				m_Tiles[x, y].isSafe = false;
				m_Tiles[x, y].adjacentMineCount = 0;
				m_Tiles[x, y].adjacentFlagCount = 0;
			}
		}
	}

	private void Detonate()
	{
		for (int x = 0; x < m_Width; ++x)
		{
			for (int y = 0; y < m_Height; ++y)
			{
				m_Tiles[x, y].isCleared |= m_Tiles[x, y].isMine;
				m_Tiles[x, y].isFlagged &= !m_Tiles[x, y].isMine;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
	public UnityEvent onWin = new UnityEvent();
	public UnityEvent onLoss = new UnityEvent();

	[SerializeField] Vector2Int gridSize = new Vector2Int(9, 9);

	[SerializeField] SlotView slotViewPrefab;

	[SerializeField] RectTransform container;

	public int mineCount = 5;
	private Slot[,] slots;

	private bool firstReveal = true;

	private void Start()
	{
		InitializeGrid();
	}

	public void InitializeGrid()
	{
		slots = new Slot[gridSize.x, gridSize.y];

		for (int i = 0; i < gridSize.x; i++)
		{
			for (int j = 0; j < gridSize.y; j++)
			{
				var slot = new Slot(i, j);
				slot.onRevealChanged += OnSlotRevealed;

				var view = Instantiate(slotViewPrefab, container);
				view.SetSlot(slot);
				view.onSlotRevealed += RevealSlot;
				view.name = $"Slot ({i},{j})";
				slots[i, j] = slot;
			}
		}
	}

	public void SpawnMines(int row, int column)
	{
		int mines = mineCount;
		if (mines > gridSize.x * gridSize.y)
		{
			mines = (gridSize.x * gridSize.y) - 1;
		}
		while (mines >= 0)
		{
			int randRow = Random.Range(0, gridSize.x);
			int randColumn = Random.Range(0, gridSize.y);
			if (randRow == row && randColumn == column)
				continue;
			var slot = slots[randRow, randColumn];
			if (slot.hasMine)
				continue;

			slot.hasMine = true;
			--mines;
		}
	}

	public void IdentifyNeighbors()
	{
		for (int i = 0; i < gridSize.x; i++)
		{
			for (int j = 0; j < gridSize.y; j++)
			{
				slots[i, j].mineNeighborCount = NearbyMines(i, j);
			}
		}
	}

	private int NearbyMines(int row, int column)
	{
		int count = 0;

		if (ContainsMine(row - 1, column - 1))
			++count;
		if (ContainsMine(row, column - 1))
			++count;
		if (ContainsMine(row + 1, column - 1))
			++count;
		if (ContainsMine(row - 1, column))
			++count;
		if (ContainsMine(row - 1, column + 1))
			++count;
		if (ContainsMine(row + 1, column + 1))
			++count;
		if (ContainsMine(row + 1, column))
			++count;
		if (ContainsMine(row, column + 1))
			++count;

		return count;
	}

	private bool ContainsMine(int row, int column)
	{
		if (!SlotExits(row, column))
			return false;

		return slots[row, column].hasMine;
	}

	private bool ContainsEmpty(int row, int column)
	{
		if (!SlotExits(row, column))
			return false;


		return slots[row, column].mineNeighborCount == 0 && !slots[row, column].revealed;

	}

	private bool SlotExits(int row, int column)
	{
		if (!HasRow(row))
			return false;
		if (!HasColumn(column))
			return false;

		return true;
	}

	private bool HasRow(int row)
	{
		return row > -1 && row < gridSize.x;
	}

	private bool HasColumn(int column)
	{
		return column > -1 && column < gridSize.y;
	}

	public void RevealSlot(int row, int column)
	{
		if (!HasRow(row) || !HasColumn(column))
			return;

		if (firstReveal)
		{
			SpawnMines(row, column);
			IdentifyNeighbors();
			firstReveal = false;
		}

		var slot = slots[row, column];
		slot.revealed = true;

		if (slot.hasMine)
		{
			onLoss.Invoke();
			return;
		}
	}

	private void OnSlotRevealed(object obj, bool value)
	{
		if (value)
		{
			var slot = obj as Slot;
			if (!slot.hasMine && slot.mineNeighborCount == 0)
			{
				RevealNeighbors(slot.location.x, slot.location.y);
			}
			if (AllSlotsRevealed())
			{
				onWin.Invoke();
			}
		}
	}

	private void RevealNeighbors(int row, int column)
	{
		if (SlotExits(row - 1, column - 1))
			slots[row - 1, column - 1].revealed = true;
		if (SlotExits(row, column - 1))
			slots[row, column - 1].revealed = true;
		if (SlotExits(row + 1, column - 1))
			slots[row + 1, column - 1].revealed = true;
		if (SlotExits(row - 1, column))
			slots[row - 1, column].revealed = true;
		if (SlotExits(row - 1, column + 1))
			slots[row - 1, column + 1].revealed = true;
		if (SlotExits(row + 1, column + 1))
			slots[row + 1, column + 1].revealed = true;
		if (SlotExits(row + 1, column))
			slots[row + 1, column].revealed = true;
		if (SlotExits(row, column + 1))
			slots[row, column + 1].revealed = true;
	}

	public void ResetGrid()
	{
		firstReveal = true;
		for (int i = 0; i < gridSize.x; i++)
		{
			for (int j = 0; j < gridSize.y; j++)
			{
				slots[i, j].Reset();
			}
		}
	}

	private bool AllSlotsRevealed()
	{

		for (int i = 0; i < gridSize.x; i++)
		{
			for (int j = 0; j < gridSize.y; j++)
			{
				if (slots[i, j].hasMine)
					continue;
				if (!slots[i, j].revealed)
					return false;
			}
		}

		return true;
	}
}

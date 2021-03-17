using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
	public Slot(int row, int column)
	{
		_location = new Vector2Int(row, column);
	}

	private Vector2Int _location;
	public Vector2Int location => _location;

	private bool _hasMine;

	public bool hasMine
	{
		get => _hasMine;
		set
		{
			if (_hasMine == value)
				return;

			_hasMine = value;
			onMineChanged?.Invoke(value);
		}
	}

	private bool _hasFlag;

	public bool hasFlag
	{
		get => _hasFlag;
		set
		{
			if (_hasFlag == value)
				return;

			_hasFlag = value;
			onFlagChanged?.Invoke(this, value);
		}
	}

	private bool _revealed;
	public bool revealed
	{
		get => _revealed;
		set
		{
			if (_revealed == value)
				return;

			_revealed = value;
			onRevealChanged?.Invoke(this, value);
		}
	}


	[SerializeField] private int _mineNeighborCount;
	public int mineNeighborCount
	{
		get => _mineNeighborCount;
		set
		{

			_mineNeighborCount = value;
			onMineNeighborCountChanged?.Invoke(value);
		}
	}

	public EventHandler<bool> onRevealChanged;
	public event Action<int> onMineNeighborCountChanged;
	public event Action<bool> onMineChanged;
	public EventHandler<bool> onFlagChanged;

	public void Reset()
	{
		mineNeighborCount = 0;
		hasMine = false;
		revealed = false;
		hasFlag = false;
	}
}

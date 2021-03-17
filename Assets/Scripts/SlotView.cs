using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotView : MonoBehaviour
{
	[System.Serializable]
	public class UnityBoolEvent : UnityEvent<bool> { }

	[System.Serializable]
	public class UnityStringEvent : UnityEvent<string> { }

	public UnityBoolEvent onRevealedInverted = new UnityBoolEvent();
	public UnityBoolEvent onHasMine = new UnityBoolEvent();
	public UnityBoolEvent onHasFlag = new UnityBoolEvent();
	public UnityStringEvent onNeighborCount = new UnityStringEvent();

	public event Action<int, int> onSlotRevealed;
	[SerializeField] private Slot _slot;

	public void SetSlot(Slot slot)
	{
		if (slot == _slot)
			return;
		_slot = slot;
		_slot.onMineChanged += onHasMine.Invoke;
		_slot.onFlagChanged += OnSlotFlagChanged;
		_slot.onRevealChanged += OnSlotRevealChanged;
		_slot.onMineNeighborCountChanged += OnMineNeighborCountCanged;
	}

	public void ClearSlot()
	{
		if (_slot == null)
			return;

		_slot.onMineChanged -= onHasMine.Invoke;
		_slot.onFlagChanged -= OnSlotFlagChanged;
		_slot.onRevealChanged -= OnSlotRevealChanged;
		_slot.onMineNeighborCountChanged -= OnMineNeighborCountCanged;
		_slot = null;
	}

	private void OnSlotFlagChanged(object sender, bool value)
	{
		onHasFlag.Invoke(value);
	}

	private void OnSlotRevealChanged(object context, bool obj)
	{
		onRevealedInverted.Invoke(!obj);
	}

	private void OnMineNeighborCountCanged(int obj)
	{
		if (obj == 0)
		{
			onNeighborCount.Invoke(string.Empty);
		}
		else
		{
			onNeighborCount.Invoke(obj.ToString());
		}
	}

	public void RevealSlot()
	{
		if (_slot == null)
			return;

		onSlotRevealed?.Invoke(_slot.location.x, _slot.location.y);
	}

	public void FlagSlot()
	{
		if (_slot == null)
			return;
		_slot.hasFlag = !_slot.hasFlag;
	}
}

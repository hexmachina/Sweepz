using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RightClickEvent : MonoBehaviour, IPointerClickHandler
{

	public UnityEvent onRightClick = new UnityEvent();

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Right)
			return;

		onRightClick.Invoke();
	}
}

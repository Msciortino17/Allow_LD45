using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedGoalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public int GoalIndex;

	private Player m_PlayerReference;

	[SerializeField]
	private Text m_ButtonText;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_PlayerReference = GameObject.Find("Player").GetComponent<Player>();
	}

	/// <summary>
	/// Fill in goal info.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerEnter(PointerEventData eventData)
	{
		GoalInfo.Enable();
		GoalInfo.UpdateGoalInfo(m_PlayerReference.m_EquippedGoals[GoalIndex]);
	}

	/// <summary>
	/// Turn off goal info.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerExit(PointerEventData eventData)
	{
		GoalInfo.Disable();
	}

	/// <summary>
	/// Update the display text of the button.
	/// </summary>
	/// <param name="_text">The text to display.</param>
	public void UpdateText(string _text)
	{
		m_ButtonText.text = _text;
	}

	/// <summary>
	/// Get rid of this goal.
	/// </summary>
	public void RemoveGoal()
	{
		m_PlayerReference.RemoveGoal(GoalIndex);
	}
}

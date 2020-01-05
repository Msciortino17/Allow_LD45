using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalInfo : MonoBehaviour
{
	private static GoalInfo m_Reference = null;

	[SerializeField]
	private Text m_GoalNameText;

	[SerializeField]
	private Text m_WinConditionText;

	[SerializeField]
	private Text m_PositivesText;

	[SerializeField]
	private Text m_NegativesText;

	[SerializeField]
	private List<EquippedGoalButton> m_EquippedGoalButtons;

	/// <summary>
	/// Turn the UI on.
	/// </summary>
	public static void Enable()
	{
		UpdateReference();
		m_Reference.gameObject.SetActive(true);
	}

	/// <summary>
	/// Turn the UI off.
	/// </summary>
	public static void Disable()
	{
		UpdateReference();
		m_Reference.gameObject.SetActive(false);
	}

	/// <summary>
	/// Fill in info based on the given goal.
	/// </summary>
	/// <param name="_goal"></param>
	public static void UpdateGoalInfo(Goal _goal)
	{
		UpdateReference();

		m_Reference.m_GoalNameText.text = _goal.DisplayName;
		m_Reference.m_WinConditionText.text = _goal.WinCondition;
		m_Reference.m_PositivesText.text = _goal.PositiveChanges;
		m_Reference.m_NegativesText.text = _goal.NegativeChanges;
	}

	/// <summary>
	/// Only show buttons if there are enough goals for them.
	/// </summary>
	/// <param name="_player">Reference to the player. Only the player should be using this method.</param>
	public static void RefreshGoalButtons(Player _player)
	{
		UpdateReference();

		for (int i = 0; i < m_Reference.m_EquippedGoalButtons.Count; i++)
		{
			EquippedGoalButton button = m_Reference.m_EquippedGoalButtons[i];
			bool active = button.GoalIndex < _player.m_EquippedGoals.Count;
			button.gameObject.SetActive(active);
			if (active)
			{
				button.UpdateText("(" + (i + 1) + ") " + _player.m_EquippedGoals[i].DisplayName);
			}
		}
	}

	/// <summary>
	/// Helper method to make sure the reference works.
	/// </summary>
	private static void UpdateReference()
	{
		if (m_Reference == null)
		{
			m_Reference = GameObject.Find("Canvas").transform.Find("Goal Info").GetComponent<GoalInfo>();
		}
	}
}

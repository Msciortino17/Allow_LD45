using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoalPostText : MonoBehaviour
{
	private bool m_Begin;

	private float m_Timer;

	private bool Text1_Flag;
	public TextFader Text1;

	private bool Text2_Flag;
	public TextFader Text2;

	private bool Text3_Flag;
	public TextFader Text3;

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		if (!m_Begin)
		{
			return;
		}

		m_Timer += Time.deltaTime;

		if (!Text1_Flag && m_Timer > 0.5f)
		{
			Text1_Flag = true;
			Text1.FadeIn();
		}

		if (m_Timer > 3f)
		{
			Text1.FadeOut();
		}

		if (!Text2_Flag && m_Timer > 4f)
		{
			Text2_Flag = true;
			Text2.FadeIn();
		}

		if (m_Timer > 7f)
		{
			Text2.FadeOut();
		}

		if (!Text3_Flag && m_Timer > 8f)
		{
			Text3_Flag = true;
			Text2.FadeOut();
			Text3.FadeIn();
		}

		if (m_Timer > 12f)
		{
			Text3.FadeOut();
		}
	}

	/// <summary>
	/// Update the UI to show the goal's info that we're touching.
	/// </summary>
	/// <param name="other">The goal we touched.</param>
	private void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player"))
		{
			m_Begin = true;
		}
	}
}

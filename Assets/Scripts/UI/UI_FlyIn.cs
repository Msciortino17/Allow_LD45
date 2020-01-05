using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyInFrom
{
	Top,
	Bottom,
	Left,
	Right
}

/// <summary>
/// This class will move a UI component it's attached to off screen, then use math to smoothly have it transition in from a designated direction.
/// Speed, and delay are configurable.
/// </summary>
public class UI_FlyIn : MonoBehaviour
{
	private RectTransform m_Rect;

	private Vector2 m_StartingPos;

	[SerializeField]
	private FlyInFrom m_FlyInFrom;

	[SerializeField]
	private float m_Speed;

	[SerializeField]
	private float m_Delay;

	private float m_StartDelta;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_Rect = GetComponent<RectTransform>();
		m_StartingPos = m_Rect.anchoredPosition;

		if (m_FlyInFrom == FlyInFrom.Top)
		{
			m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x, Screen.height * 0.6f);
			m_StartDelta = m_StartingPos.y - m_Rect.anchoredPosition.y;
		}
		else if (m_FlyInFrom == FlyInFrom.Bottom)
		{
			m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x, -Screen.height * 0.6f);
			m_StartDelta = m_StartingPos.y - m_Rect.anchoredPosition.y;
		}
		else if (m_FlyInFrom == FlyInFrom.Left)
		{
			m_Rect.anchoredPosition = new Vector2(-Screen.width * 0.6f, m_Rect.anchoredPosition.y);
			m_StartDelta = m_StartingPos.x - m_Rect.anchoredPosition.x;
		}
		else if (m_FlyInFrom == FlyInFrom.Right)
		{
			m_Rect.anchoredPosition = new Vector2(Screen.width * 0.6f, m_Rect.anchoredPosition.y);
			m_StartDelta = m_StartingPos.x - m_Rect.anchoredPosition.x;
		}
	}

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		if (m_Delay <= 0f)
		{
			if (m_FlyInFrom == FlyInFrom.Top || m_FlyInFrom == FlyInFrom.Bottom)
			{
				float offSet = m_Speed * (m_FlyInFrom == FlyInFrom.Top ? -1f : 1f) * Time.deltaTime;
				float delta = Mathf.Abs(m_StartingPos.y - m_Rect.anchoredPosition.y);
				offSet *= Mathf.Abs(delta / m_StartDelta);
				m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x, m_Rect.anchoredPosition.y + offSet);

				if (delta < 1f)
				{
					m_Rect.anchoredPosition = m_StartingPos;
					Destroy(this);
				}
			}

			if (m_FlyInFrom == FlyInFrom.Left || m_FlyInFrom == FlyInFrom.Right)
			{
				float offSet = m_Speed * (m_FlyInFrom == FlyInFrom.Left ? 1f : -1f) * Time.deltaTime;
				float delta = Mathf.Abs(m_StartingPos.x - m_Rect.anchoredPosition.x);
				offSet *= Mathf.Abs(delta / m_StartDelta);
				m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x + offSet, m_Rect.anchoredPosition.y);

				if (delta < 1f)
				{
					m_Rect.anchoredPosition = m_StartingPos;
					Destroy(this);
				}
			}
		}
		else
		{
			m_Delay -= Time.deltaTime;
		}
	}
}

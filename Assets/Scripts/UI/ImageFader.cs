using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
	private Image m_MyImage;

	private FadeState m_CurrentFadeState;

	[SerializeField]
	private float m_FadeTime;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_CurrentFadeState = FadeState.Idle;
		m_MyImage = GetComponent<Image>();
	}

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		if (m_CurrentFadeState == FadeState.FadingIn)
		{
			Color oldColor = m_MyImage.color;
			if (oldColor.a < 1f)
			{
				oldColor.a += (1f / m_FadeTime) * Time.deltaTime;
				if (oldColor.a > 1f)
				{
					oldColor.a = 1f;
				}
				m_MyImage.color = oldColor;
			}
		}

		else if (m_CurrentFadeState == FadeState.FadingOut)
		{
			Color oldColor = m_MyImage.color;
			if (oldColor.a > 0f)
			{
				oldColor.a -= (1f / m_FadeTime) * Time.deltaTime;
				if (oldColor.a < 0f)
				{
					oldColor.a = 0f;
				}
				m_MyImage.color = oldColor;
			}
		}
	}

	/// <summary>
	/// Trigger to fade in.
	/// </summary>
	public void FadeIn()
	{
		m_CurrentFadeState = FadeState.FadingIn;
	}

	/// <summary>
	/// Trigger to fade out.
	/// </summary>
	public void FadeOut()
	{
		m_CurrentFadeState = FadeState.FadingOut;
	}
}

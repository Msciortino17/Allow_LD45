using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLevel : MonoBehaviour
{

	private float m_Timer;

	private bool Text1_Flag;
	public TextFader Text1;

	private bool Text2_Flag;
	public TextFader Text2;

	private bool Text3_Flag;
	public TextFader Text3;

	private bool Text4_Flag;
	public TextFader Text4;

	private bool Text5_Flag;
	public TextFader Text5;

	private bool Text6_Flag;
	public TextFader Text6;

	private bool Image_Flag;
	public ImageFader IdImage;

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
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

		if (m_Timer > 9f)
		{
			Text2.FadeOut();
		}

		if (!Text3_Flag && m_Timer > 10f)
		{
			Text3_Flag = true;
			Text3.FadeIn();
		}

		if (m_Timer > 13f)
		{
			Text3.FadeOut();
		}

		if (!Image_Flag && m_Timer > 14f)
		{
			Image_Flag = true;
			IdImage.FadeIn();
		}

		if (m_Timer > 20f)
		{
			IdImage.FadeOut();
		}

		if (!Text4_Flag && m_Timer > 21f)
		{
			Text4_Flag = true;
			Text4.FadeIn();
		}

		if (m_Timer > 24f)
		{
			Text4.FadeOut();
		}

		if (!Text5_Flag && m_Timer > 25f)
		{
			Text5_Flag = true;
			Text5.FadeIn();
		}

		if (m_Timer > 29f)
		{
			Text5.FadeOut();
		}

		if (!Text6_Flag && m_Timer > 30f)
		{
			Text6_Flag = true;
			Text6.FadeIn();
		}
	}

	/// <summary>
	/// Link to my website!
	/// </summary>
	public void OpenWebsite()
	{
		Application.OpenURL("https://msciortino17.wixsite.com/portfolio");
	}
}

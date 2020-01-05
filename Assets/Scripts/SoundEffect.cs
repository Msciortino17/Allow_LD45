using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
	private float m_Timer;

	/// <summary>
	/// Simply destroy itself after a second.
	/// </summary>
	void Update()
	{
		m_Timer += Time.deltaTime;
		if (m_Timer > 1f)
		{
			Destroy(gameObject);
		}
	}
}

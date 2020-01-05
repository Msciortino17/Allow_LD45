using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPost : MonoBehaviour
{
	public Transform CompletedSound;

	private float m_Timer;

	private bool m_Triggered;

	void Update()
	{
		if (m_Triggered && m_Timer > 0f)
		{
			m_Timer -= Time.deltaTime;
			if (m_Timer <= 0f)
			{
				string sceneName = SceneManager.GetActiveScene().name;
				if (sceneName == "Level_Tutorial")
				{
					SceneManager.LoadScene("Level_0");
				}
				else
				{
					int currentLevel = (int)char.GetNumericValue(sceneName[6]);
					SceneManager.LoadScene("Level_" + (currentLevel + 1));
				}
			}
		}
	}

	/// <summary>
	/// Update the UI to show the goal's info that we're touching.
	/// </summary>
	/// <param name="other">The goal we touched.</param>
	private void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") && _other.GetComponent<Player>().CanWinWithGoalPost())
		{
			Instantiate(CompletedSound);
			m_Triggered = true;
			m_Timer = 1f;
		}
	}
}

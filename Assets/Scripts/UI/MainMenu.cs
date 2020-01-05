using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private Transform m_ExitButton;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		// Disable exit button for web browser, player's can just close out the tab/window.
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			m_ExitButton.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Loads the intro scene.
	/// </summary>
	public void Play()
	{
		SceneManager.LoadScene("Level_Intro");
	}

	/// <summary>
	/// Exits the application
	/// </summary>
	public void Exit()
	{
		Application.Quit();
	}
}

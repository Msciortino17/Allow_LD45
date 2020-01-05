using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lord forgive me, this is going to be one messy class...
/// (For anyone reading this, I think you should use timelines. I just don't know how to and think it'll take too much time to learn. So I made this abomination instead... x_x
/// </summary>
public class IntroManager : MonoBehaviour
{
	private float m_Timer;

	public Animator PlayerAnimator;

	/// <summary>
	/// Text fading stuff
	/// </summary>
	private bool Text_Name_Flag;
	public TextFader Text_Name;

	private bool Text_Goals_Flag;
	public TextFader Text_Goals;

	private bool Text_Change_Flag;
	public TextFader Text_Change;

	private bool Text_Spacebar_Flag;
	public TextFader Text_Spacebar;

	// Goal info UI
	public GoalInfo GoalInfoUI;

	// Dark overlay
	public Transform DarkOveraly;

	// Intro goal
	public Goal IntroGoal;

	// Goal collectable reference
	private Rigidbody GoalCollectable;

	// Player running flag
	private bool m_PlayerRunFlag;

	// Final flag
	private bool m_FinalFlag;

	public Transform GlassBreakSound;
	public Transform JumpSound;

	/// <summary>
	/// Prefabs
	/// </summary>
	private bool GoalPrefab_Flag;
	public Transform GoalPrefab;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		PlayerAnimator.SetFloat("Speed_Mod", 0);
	}

	/// <summary>
	/// Slightly delayed start.
	/// </summary>
	void Startup()
	{
	}

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		m_Timer += Time.deltaTime;

		// First, fade the texts
		if (!Text_Name_Flag && m_Timer > 5f)
		{
			Text_Name.FadeIn();
			Text_Name_Flag = true;
		}
		if (m_Timer > 8f)
		{
			Text_Name.FadeOut();
		}

		if (!Text_Goals_Flag && m_Timer > 9f)
		{
			Text_Goals.FadeIn();
			Text_Goals_Flag = true;
		}
		if (m_Timer > 12f)
		{
			Text_Goals.FadeOut();
		}

		if (!Text_Change_Flag && m_Timer > 13f)
		{
			Text_Change.FadeIn();
			Text_Change_Flag = true;
		}
		if (m_Timer > 16f)
		{
			Text_Change.FadeOut();
		}

		// Next, spawn in the goal prefab
		if (!GoalPrefab_Flag && m_Timer > 18f)
		{
			// Create the goal and throw it in
			GoalPrefab_Flag = true;
			GoalCollectable = Instantiate(GoalPrefab).GetComponent<Rigidbody>();
			GoalCollectable.transform.position = new Vector3(10f, -3, 0f);
			GoalCollectable.AddForce(Vector3.left * 20f + Vector3.up * 20f, ForceMode.Impulse);
			GoalCollectable.AddTorque(transform.forward * 10f, ForceMode.Impulse);

			// Glass break sound
			Instantiate(GlassBreakSound);

			// Startle the player
			PlayerAnimator.SetFloat("Speed_Mod", 1);

			// Brighten the room
			DarkOveraly.gameObject.SetActive(false);
		}

		// Now wait for the player to press space bar
		if (!Text_Spacebar_Flag && m_Timer > 22f)
		{
			GoalInfoUI.gameObject.SetActive(true);
			GoalInfo.UpdateGoalInfo(IntroGoal);

			Text_Spacebar.FadeIn();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Text_Spacebar.FadeOut();
				GoalInfoUI.gameObject.SetActive(false);
				Text_Spacebar_Flag = true;
				Destroy(GoalCollectable.gameObject);

				// Reset the timer and mark the final flag
				m_Timer = 0f;
				m_FinalFlag = true;
			}
		}

		// Have the player jump out of beb and out the door!
		if (m_FinalFlag && !m_PlayerRunFlag && m_Timer > 1f)
		{
			m_PlayerRunFlag = true;
			var rigidBody = PlayerAnimator.gameObject.AddComponent<Rigidbody>();
			rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			rigidBody.AddForce(Vector3.left * 5.55f + Vector3.up * 20f, ForceMode.Impulse);
			Instantiate(JumpSound);
		}

		// If the final flag has been set, move onto the next level after a few seconds.
		if (m_FinalFlag && m_Timer > 4f)
		{
			SceneManager.LoadScene("Level_Tutorial");
		}
	}
}

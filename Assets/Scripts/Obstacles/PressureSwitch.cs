using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
	private int m_CollisionMask;

	[SerializeField]
	private bool m_CurrentState;

	public ActiveState m_ConnectedObstacle;

	private SpriteRenderer m_SpriteRenderer;

	private bool m_PrevState;

	public Transform ToggleSound;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		// Setup the grounded layer mask
		m_CollisionMask = (1 << 8);
		m_CollisionMask |= (1 << 9);
		m_CollisionMask |= (1 << 12);

		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_PrevState = m_CurrentState;
	}

	/// <summary>
	/// Delayed startup.
	/// </summary>
	void Start()
	{
		// Apply the starting state
		UpdateState(m_CurrentState, false);
	}

	/// <summary>
	/// Standard update, check for anything above the switch.
	/// </summary>
	void Update()
	{
		bool state = FullRaycastUp();
		if (state != m_PrevState)
		{
			UpdateState(state);
		}
		m_PrevState = state;
	}

	/// <summary>
	/// Save the given state, notify the obstacle this is connected to, and update visuals.
	/// </summary>
	/// <param name="_state">The new state.</param>
	public void UpdateState(bool _state, bool doSound = true)
	{
		m_CurrentState = _state;
		m_ConnectedObstacle.SetState(m_CurrentState);
		m_SpriteRenderer.color = m_CurrentState ? Color.green : Color.red;

		if (doSound)
		{
			Instantiate(ToggleSound);
		}
	}

	private bool FullRaycastUp()
	{
		int numRayChecks = (int)(transform.localScale.x * 3 + 0.5f) + 1;

		float yOffset = transform.localScale.y * 0.5f;
		for (int i = 0; i < numRayChecks; i++)
		{
			float xOffset = -(transform.localScale.x * 0.5f) + (i / 3f);
			if (Physics.Raycast(transform.position + new Vector3(xOffset, 0f, 0f), Vector3.up, yOffset + 0.05f, m_CollisionMask))
			{
				return true;
			}
		}

		return false;
	}
}

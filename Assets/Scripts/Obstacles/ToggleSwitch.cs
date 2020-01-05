using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
	[SerializeField]
	private bool m_CurrentState;

	public ActiveState m_ConnectedObstacle;

	[SerializeField]
	private float m_ActivationSpeed;

	private SpriteRenderer m_SpriteRenderer;

	public Transform ToggleSound;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
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
	/// Whatever we collide with needs to be falling
	/// </summary>
	/// <param name="_collision"></param>
	void OnCollisionEnter(Collision _collision)
	{
		if (_collision.relativeVelocity.y < -m_ActivationSpeed)
		{
			if (_collision.gameObject.CompareTag("Player") && !_collision.gameObject.GetComponent<Player>().CanActivateSwitches())
			{
				return;
			}

			UpdateState(!m_CurrentState);
		}
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
}

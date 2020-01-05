using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ActiveState
{
	[SerializeField]
	private bool m_InvertState;

	private BoxCollider m_BoxCollider;
	private SpriteRenderer m_SpriteRenderer;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_BoxCollider = GetComponent<BoxCollider>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Toggle the collider and sprite renderer on/off.
	/// </summary>
	/// <param name="_state">The state to set it to.</param>
	public override void SetState(bool _state)
	{
		m_BoxCollider.enabled = m_InvertState ? _state : !_state;
		m_SpriteRenderer.enabled = m_InvertState ? _state : !_state;
	}
}

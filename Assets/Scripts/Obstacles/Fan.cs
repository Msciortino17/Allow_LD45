using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : ActiveState
{
	[SerializeField]
	private float m_Power;

	[SerializeField]
	private bool m_InvertState;

	[SerializeField]
	private bool m_OnlyAffectCrates = false;

	private BoxCollider m_BoxCollider;
	private SpriteRenderer m_SpriteRenderer;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_BoxCollider = GetComponent<BoxCollider>();
		m_SpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// Apply a continuous force to any rigid bodies in the area of effect, if active.
	/// </summary>
	/// <param name="other">The area to affect.</param>
	private void OnTriggerStay(Collider _other)
	{
		var otherRigidBody = _other.GetComponent<Rigidbody>();
		if (otherRigidBody != null)
		{
			if (m_OnlyAffectCrates && !_other.CompareTag("Crate"))
			{
				return;
			}

			otherRigidBody.AddForce(transform.up * m_Power * Time.deltaTime, ForceMode.Force);
		}
	}

	/// <summary>
	/// Set/On off
	/// TODO: Update graphics once we have that working
	/// </summary>
	/// <param name="_state">The state to set it to.</param>
	public override void SetState(bool _state)
	{
		m_BoxCollider.enabled = m_InvertState ? _state : !_state;
		m_SpriteRenderer.color = m_InvertState ? Color.magenta : Color.cyan;
	}
}

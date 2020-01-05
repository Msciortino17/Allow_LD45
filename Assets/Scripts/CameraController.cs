using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Transform m_PlayerReference;
	private Rigidbody m_MyRigidBody;

	private float m_HorizontalEdge;
	private float m_VerticalEdge;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		m_PlayerReference = GameObject.Find("Player").transform;
		m_MyRigidBody = GetComponent<Rigidbody>();

		// TODO: Make these based off of screen size
		m_HorizontalEdge = 4f;
		m_VerticalEdge = 6f;
	}

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		Vector3 delta = m_PlayerReference.position - transform.position;
		delta.z = 0f;

		if (Mathf.Abs(delta.x) > m_HorizontalEdge)
		{
			m_MyRigidBody.AddForce(new Vector3(delta.x * 20f, 0f, 0f), ForceMode.Force);
		}

		if (Mathf.Abs(delta.y) > m_VerticalEdge)
		{
			m_MyRigidBody.AddForce(new Vector3(0f, delta.y * 20f, 0f), ForceMode.Force);
		}
	}
}

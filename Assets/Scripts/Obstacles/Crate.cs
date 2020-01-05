using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2 hours left, no time for comments or clean code
/// </summary>
public class Crate : MonoBehaviour
{

	[SerializeField]
	private float m_MaxVerticalSpeed;


	private Rigidbody m_MyRigidBbody;

	// Start is called before the first frame update
	void Awake()
	{

		m_MyRigidBbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

		// Read in the current velocity
		var velocity = m_MyRigidBbody.velocity;

		// Limit falling speed
		if (velocity.y < -m_MaxVerticalSpeed)
		{
			m_MyRigidBbody.velocity = new Vector3(velocity.x, -m_MaxVerticalSpeed, velocity.z);
		}

		// Limit rising speed
		if (velocity.y > m_MaxVerticalSpeed)
		{
			m_MyRigidBbody.velocity = new Vector3(velocity.x, m_MaxVerticalSpeed, velocity.z);
		}

	}
}

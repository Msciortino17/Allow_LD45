using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private int m_GroundLayerMask;

	[Header("Movement")]
	[SerializeField]
	private float m_GroundedMoveSpeed;

	[SerializeField]
	private float m_AerielMoveSpeed;

	[SerializeField]
	private float m_MaxHorizontalSpeed;

	[SerializeField]
	private float m_MaxVerticalSpeed;

	private bool m_Grounded;

	private bool m_Crouching;

	[SerializeField]
	private float m_JumpPower;

	private float m_ShortHopTimer;

	private float m_WallSlideStickTimer;

	private float m_WallJumpNoAerialMovementTimer;

	private bool m_PrevWallSlideStickFlag;

	[SerializeField]
	public bool m_CanDoubleJump;

	[Header("References and prefabs")]
	[SerializeField]
	private GameObject m_GoalPrefab;

	private Rigidbody m_MyRigidBbody;

	[Header("Goals")]
	public List<Goal> m_EquippedGoals;

	private float m_FreeFallTimer;
	bool m_TriggerFreeFallWin = false;
	float m_FreeFallWinTimer = 0f;

	// Starting values
	private float m_Starting_GroundedMoveSpeed;
	private float m_Starting_AerielMoveSpeed;
	private float m_Starting_MaxHorizontalSpeed;
	private float m_Starting_MaxVerticalSpeed;
	private float m_Starting_JumpPower;
	private float m_Starting_Mass;

	// Sounds
	public Transform JumpSound;
	public Transform CrouchSound;
	public Transform LandingSound;
	public Transform WinSound;

	/// <summary>
	/// Standard startup
	/// </summary>
	void Awake()
	{
		// Setup lists and default value
		m_EquippedGoals = new List<Goal>();
		m_CanDoubleJump = true;

		// Setup references
		m_MyRigidBbody = GetComponent<Rigidbody>();

		// Setup the grounded layer mask
		m_GroundLayerMask = (1 << 9);
		m_GroundLayerMask |= (1 << 12);

		// Save starting values
		m_Starting_GroundedMoveSpeed = m_GroundedMoveSpeed;
		m_Starting_AerielMoveSpeed = m_AerielMoveSpeed;
		m_Starting_MaxHorizontalSpeed = m_MaxHorizontalSpeed;
		m_Starting_MaxVerticalSpeed = m_MaxVerticalSpeed;
		m_Starting_JumpPower = m_JumpPower;
		m_Starting_Mass = m_MyRigidBbody.mass;
	}

	/// <summary>
	/// Standard update
	/// </summary>
	void Update()
	{
		UpdateInput();

		// Read in the current velocity
		var velocity = m_MyRigidBbody.velocity;

		// Limit falling speed
		if (velocity.y < -m_MaxVerticalSpeed)
		{
			m_MyRigidBbody.velocity = new Vector3(velocity.x, -m_MaxVerticalSpeed, velocity.z);
		}

		// Update helpers
		UpdateGrounded();
		UpdateWallStick();

		if (!m_Grounded)
		{
			m_FreeFallTimer += Time.deltaTime;
		}

		// Free fall win condition
		if (!m_TriggerFreeFallWin && m_FreeFallTimer > FreeFallWinTime())
		{
			m_TriggerFreeFallWin = true;
			Instantiate(WinSound);
		}

		if (m_TriggerFreeFallWin)
		{
			m_FreeFallWinTimer += Time.deltaTime;
			if (m_FreeFallWinTimer > 1f)
			{
				string sceneName = SceneManager.GetActiveScene().name;
				int currentLevel = (int)char.GetNumericValue(sceneName[6]);
				SceneManager.LoadScene("Level_" + (currentLevel + 1));
			}
		}

		//DebugText.SetText("Velocity: " + velocity);
		//DebugText.SetText("Grounded: " + m_Grounded);
		//DebugText.SetText("Goals: " + m_EquippedGoals.Count);
		//DebugText.SetText("Jump power: " + m_JumpPower);
		DebugText.SetText("Free fall timer: " + m_FreeFallTimer);
	}

	/// <summary>
	/// Handles reading input from the user and performing appropriate actions.
	/// </summary>
	private void UpdateInput()
	{
		// Read in the current velocity
		var velocity = m_MyRigidBbody.velocity;

		// Horizontal movement
		if (m_WallJumpNoAerialMovementTimer > 0f)
		{
			m_WallJumpNoAerialMovementTimer -= Time.deltaTime;
		}
		else
		{
			float moveSpeed = m_Grounded ? m_GroundedMoveSpeed : m_AerielMoveSpeed;
			if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && velocity.x > -m_MaxHorizontalSpeed)
			{
				MoveHorizontally(-moveSpeed * (FullRaycastLeft() ? 0.3f : 1f));
			}
			if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && velocity.x < m_MaxHorizontalSpeed)
			{
				MoveHorizontally(moveSpeed * (FullRaycastRight() ? 0.3f : 1f));
			}
		}

		// Jumping
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_ShortHopTimer = 0.12f;
		}
		if (m_ShortHopTimer > 0f)
		{
			if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				Jump(0.75f);
			}
			m_ShortHopTimer -= Time.deltaTime;
			if (m_ShortHopTimer <= 0f || !m_Grounded)
			{
				Jump(1f);
			}
		}

		// Crouching
		if (CanCrouch() && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
		{
			Crouch();
		}
		else
		{
			UnCrouch();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		// Check input for removing goals
		RemoveGoalInput();
	}

	#region Movement helpers

	/// <summary>
	/// Handles all jumping logic
	/// </summary>
	private void Jump(float _modifier)
	{
		// Handle wall jumping
		if (!m_Grounded && FullRaycastLeft() && CanWallJumpGoal())
		{
			float wallJumpPower = Mathf.Min(m_JumpPower, 20f);

			Vector3 velocity = m_MyRigidBbody.velocity;
			velocity.y = wallJumpPower * _modifier;
			velocity.x = wallJumpPower * _modifier;
			m_MyRigidBbody.velocity = velocity;

			// After jumping off the wall, reset the wall stick timer.
			m_WallSlideStickTimer = 0f;

			// We need to turn this on for a bit so player's can't use wall jumping to climb vertical walls
			m_WallJumpNoAerialMovementTimer = 0.25f;

			// No double jumping after wall jumping
			m_CanDoubleJump = false;

			Instantiate(JumpSound);
		}
		else if (!m_Grounded && FullRaycastRight() && CanWallJumpGoal())
		{
			float wallJumpPower = Mathf.Min(m_JumpPower, 20f);

			Vector3 velocity = m_MyRigidBbody.velocity;
			velocity.y = wallJumpPower * _modifier;
			velocity.x = -wallJumpPower * _modifier;
			m_MyRigidBbody.velocity = velocity;

			// After jumping off the wall, reset the wall stick timer.
			m_WallSlideStickTimer = 0f;

			// We need to turn this on for a bit so player's can't use wall jumping to climb vertical walls
			m_WallJumpNoAerialMovementTimer = 0.25f;

			// No double jumping after wall jumping
			m_CanDoubleJump = false;

			Instantiate(JumpSound);
		}
		// Only jump when grounded or if double jump is availabble
		else if (m_Grounded || (m_CanDoubleJump && m_MyRigidBbody.velocity.y < 7f))
		{
			if (!m_Grounded)
			{
				m_CanDoubleJump = false;
			}

			Vector3 velocity = m_MyRigidBbody.velocity;
			velocity.y = m_JumpPower * _modifier / m_MyRigidBbody.mass;
			m_MyRigidBbody.velocity = velocity;

			Instantiate(JumpSound);
		}
	}

	/// <summary>
	/// Helper for handling wall stick
	/// </summary>
	private void UpdateWallStick()
	{
		bool stick = FullRaycastLeft() || FullRaycastRight();
		if (stick && !m_PrevWallSlideStickFlag)
		{
			m_WallSlideStickTimer = 0.25f;
		}

		if (m_WallSlideStickTimer >= 0f)
		{
			m_WallSlideStickTimer -= Time.deltaTime;
		}

		m_PrevWallSlideStickFlag = stick;
		//DebugText.SetText("Wall stick timer: " + m_WallSlideStickTimer);
	}

	/// <summary>
	/// For entering the crouch state
	/// </summary>
	private void Crouch()
	{
		if (!m_Crouching)
		{
			m_Crouching = true;
			transform.localScale = new Vector3(1f, 0.5f, 1f);
			transform.Translate(0f, -0.25f, 0f);
			Instantiate(CrouchSound);
		}
	}

	/// <summary>
	/// For exiting the crouch state
	/// </summary>
	private void UnCrouch()
	{
		if (m_Crouching && !FullRaycastUp())
		{
			m_Crouching = false;
			transform.localScale = new Vector3(1f, 1f, 1f);
			transform.Translate(0f, 0.25f, 0f);
			Instantiate(CrouchSound);
		}
	}

	/// <summary>
	/// Helper method for moving horizontally
	/// </summary>
	/// <param name="_speed">The speed to move by.</param>
	private void MoveHorizontally(float _speed)
	{
		if (m_WallSlideStickTimer <= 0f)
		{
			m_MyRigidBbody.AddForce(new Vector3(_speed, 0f, 0f) * Time.deltaTime, ForceMode.Force);
		}
	}

	/// <summary>
	/// Helper method that does a simple raycast down below for walls.
	/// </summary>
	private void UpdateGrounded()
	{
		bool changed = m_Grounded;
		m_Grounded = FullRaycastDown();

		// Only do certain things if we changed being grounded or not this frame.
		if (changed != m_Grounded)
		{
			if (m_Grounded)
			{
				// Restore double jump, unless a goal prevents this.
				m_CanDoubleJump = true;
				foreach (Goal goal in m_EquippedGoals)
				{
					if (!goal.CanDoubleJump)
					{
						m_CanDoubleJump = false;
					}
				}

				// Reset free fall timer
				m_FreeFallTimer = 0f;
			}
		}
	}

	/// <summary>
	/// Make a sound when landing
	/// </summary>
	/// <param name="_collision">What we collided with.</param>
	void OnCollisionEnter(Collision _collision)
	{
		Instantiate(LandingSound);
	}

	#endregion

	#region Goals and stats

	/// <summary>
	/// Resets all stats to their starting values
	/// </summary>
	public void ResetStats()
	{
		m_GroundedMoveSpeed = m_Starting_GroundedMoveSpeed;
		m_AerielMoveSpeed = m_Starting_AerielMoveSpeed;
		m_MaxHorizontalSpeed = m_Starting_MaxHorizontalSpeed;
		m_MaxVerticalSpeed = m_Starting_MaxVerticalSpeed;
		m_JumpPower = m_Starting_JumpPower;
		m_MyRigidBbody.mass = m_Starting_Mass;
	}

	/// <summary>
	/// Sets the stats based on the current equipped win conditions
	/// </summary>
	public void CalculateStats()
	{
		ResetStats();

		foreach (Goal goal in m_EquippedGoals)
		{
			m_GroundedMoveSpeed *= goal.GroundedMoveSpeedModifier;
			m_AerielMoveSpeed *= goal.AerielMoveSpeedModifier;
			m_MaxHorizontalSpeed *= goal.MaxHorizontalSpeedModifier;
			m_MaxVerticalSpeed *= goal.MaxVerticalSpeedModifier;
			m_JumpPower *= goal.JumpPowerModifier;
			m_MyRigidBbody.mass = m_MyRigidBbody.mass * goal.MassModifier;
		}
	}

	/// <summary>
	/// Looks through all equipped goals to see if touching the end flag is a valid win condition.
	/// </summary>
	/// <returns>If we can touch the flag to win.</returns>
	public bool CanWinWithGoalPost()
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (goal.TriggersEndFlag)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Helper method for checking if we can wall jump with current goals.
	/// </summary>
	/// <returns>If we can wall jump.</returns>
	private bool CanWallJumpGoal()
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (!goal.CanWallJump)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Helper method for checking if we can crouch.
	/// </summary>
	/// <returns>If we can crouch.</returns>
	private bool CanCrouch()
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (!goal.CanCrouch)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Check goals to see if we can't activate switches.
	/// </summary>
	/// <returns>If we can activate switches.</returns>
	public bool CanActivateSwitches()
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (!goal.CanActivateSwitches)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns a goal with a reasonable free fall time.
	/// </summary>
	/// <returns>Free fall time to win.</returns>
	private float FreeFallWinTime()
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (goal.FreeFallTime < 999f)
			{
				return goal.FreeFallTime;
			}
		}

		return 9999f;
	}

	/// <summary>
	/// Looks through the equipped goals to see if the requested goal is equipped.
	/// </summary>
	/// <param name="_goalName"></param>
	/// <returns></returns>
	public bool HasGoal(string _goalName)
	{
		foreach (Goal goal in m_EquippedGoals)
		{
			if (goal.GoalName == _goalName)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Attempt to add the given goal, if there's enough room.
	/// </summary>
	/// <param name="_goal">The goal to add.</param>
	/// <returns>Whether or not it was successfully added.</returns>
	private bool AddGoal(Goal _goal)
	{
		if (m_EquippedGoals.Count >= 5)
		{
			return false;
		}

		// Add the new goal and update anything that needs to be immediately changed.
		m_EquippedGoals.Add(_goal);
		if (!_goal.CanDoubleJump)
		{
			m_CanDoubleJump = false;
		}

		GoalInfo.RefreshGoalButtons(this);
		CalculateStats();

		return true;
	}

	/// <summary>
	/// Remove the goal at the requested index and spawn it into the world at the player's current position.
	/// </summary>
	/// <param name="_index"></param>
	public void RemoveGoal(int _index)
	{
		GoalScript goal = Instantiate(m_GoalPrefab).transform.GetChild(1).GetComponent<GoalScript>();
		goal.MyGoal = m_EquippedGoals[_index];
		goal.transform.parent.position = transform.position + Vector3.up * 0.5f;
		m_EquippedGoals.RemoveAt(_index);
		GoalInfo.RefreshGoalButtons(this);
		GoalInfo.Disable();
		CalculateStats();
	}

	/// <summary>
	/// Each alpha numeric key should eject the goal.
	/// </summary>
	private void RemoveGoalInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) && m_EquippedGoals.Count >= 1)
		{
			RemoveGoal(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && m_EquippedGoals.Count >= 2)
		{
			RemoveGoal(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && m_EquippedGoals.Count >= 3)
		{
			RemoveGoal(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && m_EquippedGoals.Count >= 4)
		{
			RemoveGoal(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && m_EquippedGoals.Count >= 5)
		{
			RemoveGoal(5);
		}
	}

	/// <summary>
	/// Update the UI to show the goal's info that we're touching.
	/// </summary>
	/// <param name="other">The goal we touched.</param>
	private void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Goal"))
		{
			GoalInfo.Enable();
			GoalInfo.UpdateGoalInfo(_other.GetComponent<GoalScript>().MyGoal);
		}
	}

	/// <summary>
	/// Handle picking up new goals
	/// </summary>
	/// <param name="other">The goal we touched.</param>
	private void OnTriggerStay(Collider _other)
	{
		if (_other.CompareTag("Goal"))
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (AddGoal(_other.GetComponent<GoalScript>().MyGoal))
				{
					Destroy(_other.transform.parent.gameObject);
					GoalInfo.Disable();
				}
			}
		}
	}

	/// <summary>
	/// Clear out the UI if we're no longer touching a goal.
	/// </summary>
	/// <param name="other">The goal we touched.</param>
	private void OnTriggerExit(Collider _other)
	{
		if (_other.CompareTag("Goal"))
		{
			GoalInfo.Disable();
		}
	}

	#endregion

	// These are some quick and dirty methods, ideally I would have liked to use Physics.BoxCast but that's not been working and I don't want to waste time making it work.
	// Each one will do a raycast in a main direction from the 2 edges and center
	#region 3 line ray casts
	private bool FullRaycastUp()
	{
		return (Physics.Raycast(transform.position, Vector3.up, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(-0.49f, 0f, 0f), Vector3.up, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0.49f, 0f, 0f), Vector3.up, 0.55f, m_GroundLayerMask));
	}

	private bool FullRaycastDown()
	{
		return (Physics.Raycast(transform.position, Vector3.down, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(-0.49f, 0f, 0f), Vector3.down, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0.49f, 0f, 0f), Vector3.down, 0.55f, m_GroundLayerMask));
	}

	private bool FullRaycastLeft()
	{
		return (Physics.Raycast(transform.position, Vector3.left, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0f, -0.49f * transform.localScale.y, 0f), Vector3.left, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0f, 0.49f * transform.localScale.y, 0f), Vector3.left, 0.55f, m_GroundLayerMask));
	}

	private bool FullRaycastRight()
	{
		return (Physics.Raycast(transform.position, Vector3.right, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0f, -0.49f * transform.localScale.y, 0f), Vector3.right, 0.55f, m_GroundLayerMask) ||
				Physics.Raycast(transform.position + new Vector3(0f, 0.49f * transform.localScale.y, 0f), Vector3.right, 0.55f, m_GroundLayerMask));
	}
	#endregion

}

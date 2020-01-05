using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goal", menuName = "Allow/Goal", order = 1)]
public class Goal : ScriptableObject
{
	/// <summary>
	/// Goals will all modify base stats of the player, be default they will keep each stat the same.
	/// </summary>
	public float GroundedMoveSpeedModifier = 1f;
	public float AerielMoveSpeedModifier = 1f;
	public float MaxHorizontalSpeedModifier = 1f;
	public float MaxVerticalSpeedModifier = 1f;
	public float JumpPowerModifier = 1f;
	public float MassModifier = 1f;
	public bool CanDoubleJump = true;
	public bool CanWallJump = true;
	public bool CanActivateSwitches = true;
	public bool CanCrouch = true;

	/// <summary>
	/// Goals will also keep track of win conditions.
	/// </summary>
	public bool TriggersEndFlag = false;
	public float FreeFallTime = 9999f;

	/// <summary>
	/// The name of this goal, for internal use.
	/// </summary>
	public string GoalName = "";

	/// <summary>
	/// These values should be used for UIs and formatted cleanly.
	/// </summary>
	public string DisplayName = "";
	public string WinCondition = "";
	public string PositiveChanges = "";
	public string NegativeChanges = "";
}

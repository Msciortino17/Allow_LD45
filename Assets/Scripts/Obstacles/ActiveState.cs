using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anything that derives from this should have an active and inactive state.
/// </summary>
public abstract class ActiveState : MonoBehaviour
{
	public abstract void SetState(bool _state);
}

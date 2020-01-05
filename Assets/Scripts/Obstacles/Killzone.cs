using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{

	/// <summary>
	/// Restart the level if we collide with the player.
	/// </summary>
	/// <param name="_collision">What we collided with.</param>
	void OnCollisionEnter(Collision _collision)
	{
		if (_collision.gameObject.CompareTag("Player"))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}

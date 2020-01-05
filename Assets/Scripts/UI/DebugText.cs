using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
	private static Text textReference = null;

	/// <summary>
	/// Can be called anywhere in the code to set debug text.
	/// </summary>
	/// <param name="_text">The information to display</param>
	public static void SetText(string _text)
	{
		if (textReference == null)
		{
			textReference = GameObject.Find("Canvas").transform.Find("DebugText").GetComponent<Text>();
		}

		textReference.text = _text;
	}
}

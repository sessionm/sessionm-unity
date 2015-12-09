// SampleApp/Assets/SessionM Sample Code/SimpleSpriteButton.cs
using UnityEngine;
using System.Collections;

public class SimpleSpriteButton : MonoBehaviour {

	public GameObject targetObject;
	public string functionName;

	private void OnMouseDown()
	{
		targetObject.SendMessage(functionName);
	}

}

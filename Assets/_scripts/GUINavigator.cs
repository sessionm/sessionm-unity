using UnityEngine;
using System.Collections;

public class GUINavigator : MonoBehaviour 
{

	public GameObject chooserRoot;
	public GameObject prime31Root;
	public GameObject sessionMRoot;

	public void BackToMainMenu()
	{
		NGUITools.SetActive (chooserRoot, true);
		NGUITools.SetActive (prime31Root, false);
		NGUITools.SetActive (sessionMRoot, false);
	}

	public void ToPrime31TestScreen()
	{
		NGUITools.SetActive (chooserRoot, false);
		NGUITools.SetActive (prime31Root, true);
		NGUITools.SetActive (sessionMRoot, false);
	}

	public void ToSessionMTestScreen()
	{
		NGUITools.SetActive (chooserRoot, false);
		NGUITools.SetActive (prime31Root, false);
		NGUITools.SetActive (sessionMRoot, true);
	}

}

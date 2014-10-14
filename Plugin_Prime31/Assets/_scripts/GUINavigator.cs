﻿using UnityEngine;
using System.Collections;

public class GUINavigator : MonoBehaviour 
{

	public GameObject chooserRoot;
	public GameObject prime31Root;
	public GameObject sessionMRoot;
	public GameObject flurryRoot;
	public FlurryTest flurry;

	public void BackToMainMenu()
	{
		NGUITools.SetActive (chooserRoot, true);
		NGUITools.SetActive (prime31Root, false);
		NGUITools.SetActive (sessionMRoot, false);
		NGUITools.SetActive (flurryRoot, false);
	}

	public void ToPrime31TestScreen()
	{
		NGUITools.SetActive (chooserRoot, false);
		NGUITools.SetActive (prime31Root, true);
		NGUITools.SetActive (sessionMRoot, false);
		NGUITools.SetActive (flurryRoot, false);
	}

	public void ToSessionMTestScreen()
	{
		NGUITools.SetActive (chooserRoot, false);
		NGUITools.SetActive (prime31Root, false);
		NGUITools.SetActive (sessionMRoot, true);
		NGUITools.SetActive (flurryRoot, false);
	}

	public void ToFlurryTestScreen()
	{
		NGUITools.SetActive (chooserRoot, false);
		NGUITools.SetActive (prime31Root, false);
		NGUITools.SetActive (sessionMRoot, false);
		NGUITools.SetActive (flurryRoot, true);
		flurry.ShowAds();
	}

}

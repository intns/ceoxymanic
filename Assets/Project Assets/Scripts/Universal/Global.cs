/*
 * Global.cs
 * Created by: Kman
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Functions and variables used across many scripts
 */

using UnityEngine;

public static class Global
{
	//Variables
	public static GameObject _Player;

	//Functions
	public static void RecursiveSetColliders(Transform root, bool value)
	{
		Collider thisCollider = root.GetComponent<Collider>();
		if (thisCollider != null)
		{
			thisCollider.enabled = value;
		}

		// Loops through all of the gun's parts
		foreach (Transform child in root)
		{
			RecursiveSetColliders(child, value);
		}
	}
}


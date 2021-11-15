/*
 * IInteractable.cs
 * Created by: intns
 * Created on: 20/2/2020 (dd/mm/yy)
 * Created for: needing an easy to manage interactable class
 */

using UnityEngine;

public interface IInteractable
{
	/// <summary>
	/// Handles when the player tries to interact with a given object
	/// </summary>
	/// <param name="interactingParent"></param>
	void OnInteractStart(GameObject interactingParent);
}

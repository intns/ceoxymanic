/*
 * Rotator.cs
 * Created by: intns
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Rotator : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] float _Speed;

	void Update()
	{
		transform.Rotate(0, _Speed * Time.deltaTime, 0, Space.Self);
	}
}

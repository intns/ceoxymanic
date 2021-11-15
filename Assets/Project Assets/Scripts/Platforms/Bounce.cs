/*
 * Bounce.cs
 * Created by: intns
 * Created on: #CREATIONDATE# (dd/mm/yy)
 * Created for: #PURPOSE#
 */

using UnityEngine;

public class Bounce : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] Vector3 _Force;

	private void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if (rb == null)
		{
			return;
		}

		rb.AddForce(_Force, ForceMode.Impulse);
	}
}

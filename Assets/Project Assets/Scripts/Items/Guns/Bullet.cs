/*
 * Bullet.cs
 * Created by: Kman 
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: Physical Bullets
 */

using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int _Damage;
	public int _Force;
	public float _Speed;

	void Update()
	{
		transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		HealthManager targetHealthManager = other.GetComponent<HealthManager>();
		if (targetHealthManager != null)
		{
			targetHealthManager.RemoveHealth(_Damage);
			targetHealthManager._HurtOrigin = transform.position;
		}

		Rigidbody targetRigidbody = other.attachedRigidbody;
		if (targetRigidbody != null)
		{
			targetRigidbody.AddForce(transform.forward * _Force, ForceMode.Impulse);
		}

		Destroy(gameObject);
	}

}

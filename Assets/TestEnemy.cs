using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IHealthImpl
{
	[SerializeField] Transform _Player = null;
	[SerializeField] float _Height = 5;
	Vector3 _TargetPosition = Vector3.zero;
	float _Timer = 0;

	public void Die()
	{
		Destroy(gameObject);
	}

	void Update()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
		{
			_TargetPosition.x = hitInfo.point.x;
			_TargetPosition.y = hitInfo.point.y + _Height;
			_TargetPosition.z = hitInfo.point.z;
		}

		_Timer += Time.deltaTime;
		_TargetPosition.y += Mathf.Sin(_Timer);

		transform.position = Vector3.MoveTowards(transform.position, _TargetPosition, 0.05f);
	}
}

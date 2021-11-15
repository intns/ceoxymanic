using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IHealthImpl
{
	[SerializeField] Transform _HeadOne = null;
	[SerializeField] Transform _HeadTwo = null;
	[SerializeField] LayerMask _GroundLayer = 0;

	[SerializeField] Transform _Player = null;
	[SerializeField] float _Height = 5;
	[SerializeField] float _Speed = 5;

	Vector3 _OriginalPoint1;
	Vector3 _OriginalPoint2;
	bool _ChangePoint = false;

	Vector3 _TargetPosition = Vector3.zero;
	float _Timer = 0;
	float _GrabTimer = 0;

	public void Die()
	{
		Destroy(gameObject);
	}

	void Update()
	{
		// IK
		RaycastHit hitInfo;
		if (!_ChangePoint)
		{
			float headOneDist = Vector3.Distance(_HeadOne.position, _OriginalPoint1);
			if (headOneDist > 2.5f)
			{
				_ChangePoint = true;
			}

			float headTwoDist = Vector3.Distance(_HeadTwo.position, _OriginalPoint2);
			if (headTwoDist > 2.5f)
			{
				_ChangePoint = true;
			}
		}
		else
		{
			_HeadOne.position = _OriginalPoint1;
			_HeadTwo.position = _OriginalPoint2;
		}

		bool touchingPlayer = Vector3.Distance(transform.position, _Player.position) <= 4;
		if (!touchingPlayer)
		{
			if (Physics.Raycast(transform.position + transform.right * 2, Vector3.down, out hitInfo, float.PositiveInfinity, _GroundLayer))
			{
				if (_ChangePoint)
				{
					_OriginalPoint1 = hitInfo.point;
				}
			}

			if (Physics.Raycast(transform.position - transform.right * 2, Vector3.down, out hitInfo, float.PositiveInfinity, _GroundLayer))
			{
				if (_ChangePoint)
				{
					_OriginalPoint2 = hitInfo.point;
				}
			}

			_GrabTimer = 0;
		}
		else
		{
			_HeadOne.position = _Player.position;
			_HeadTwo.position = _Player.position;

			_GrabTimer += Time.deltaTime;
			if (_GrabTimer > 0.25f)
			{
				_Player.GetComponent<HealthManager>().RemoveHealth(5.0f);
				_GrabTimer = 0;
			}
		}

		// Rotation

		Vector3 deltaRotation = (_Player.position - transform.position).normalized;
		deltaRotation.y = 0;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(deltaRotation), 0.05f);

		// Movement
		if (!touchingPlayer)
		{
			_Timer += Time.deltaTime;
			Vector3 nextPosition = _Player.position;

			if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
			{
				nextPosition.y = hitInfo.point.y + _Height;
			}

			nextPosition.y += 1 + Mathf.Sin(_Timer) * 7.5f;

			_TargetPosition = Vector3.Lerp(_TargetPosition, nextPosition, 0.001f);
			transform.position = Vector3.MoveTowards(transform.position, _TargetPosition, _Speed * Time.deltaTime);
		}
	}

	void OnDrawGizmosSelected()
	{
		if (Vector3.Distance(transform.position, _Player.position) <= 3)
		{
			return;
		}

		if (Physics.Raycast(transform.position + transform.right * 2, Vector3.down, out RaycastHit hitInfo))
		{
			Gizmos.DrawSphere(hitInfo.point, 1);
		}

		if (Physics.Raycast(transform.position - transform.right * 2, Vector3.down, out hitInfo))
		{
			Gizmos.DrawSphere(hitInfo.point, 1);
		}
	}
}

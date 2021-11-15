/*
 * HealthManager.cs
 * Created by: Kman
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Managing health
 */
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	public float _MaxHealth;

	[HideInInspector] public float _HealthPercent;
	float _CurrentHealth;
	[HideInInspector] public Vector3 _HurtOrigin = Vector3.zero;

	private void Awake()
	{
		_CurrentHealth = _MaxHealth;
	}

	void Update()
	{
		_HealthPercent = _CurrentHealth / _MaxHealth;

		//Health is at 0 so the object has died
		if (_CurrentHealth <= 0)
		{
			IHealthImpl healthImpl = GetComponent<IHealthImpl>();
			if (healthImpl != null)
			{
				healthImpl.Die();
			}
		}
	}

	// Health Functions
	public void AddHealth(float toAdd)
	{
		_CurrentHealth += toAdd;

		if (_CurrentHealth > _MaxHealth)
		{
			_CurrentHealth = _MaxHealth;
		}
	}
	public void RemoveHealth(float toRem)
	{
		_CurrentHealth -= toRem;

		if (_CurrentHealth < 0)
		{
			_CurrentHealth = 0;
		}
	}
	public float GetHealth()
	{
		return _CurrentHealth;
	}
}
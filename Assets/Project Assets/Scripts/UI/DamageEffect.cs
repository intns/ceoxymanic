/*
 * DamageEffect.cs
 * Created by: Kman 
 * Created on: 23/2/2020 (dd/mm/yy)
 * Created for: Tinting the screen red upon taking damage
 */

using UnityEngine;
using UnityEngine.UI;
public class DamageEffect : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] HealthManager _Manager;
	Image _Image;

	[Header("Settings")]
	[SerializeField] float _EffectStrength;
	[SerializeField] float _BaseIncreasePerPointLost;
	[SerializeField] float _PermanentTintThreshold;
	[SerializeField] float _ChangeSpeed;

	float _LastHealth;
	float _PlayerMaxHealth;

	private void Awake()
	{
		_Image = GetComponent<Image>();
		_Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, 0);
	}
	private void Start()
	{
		_LastHealth = _PlayerMaxHealth = _Manager.GetHealth();
	}

	private void LateUpdate()
	{
		float currentHealth = _Manager.GetHealth();
		float baseOpacity = currentHealth <= _PermanentTintThreshold ? _BaseIncreasePerPointLost * (_PlayerMaxHealth - (currentHealth + _PlayerMaxHealth - _PermanentTintThreshold)) : 0;

		float targetOpacity = currentHealth < _LastHealth ?
				(_LastHealth - currentHealth) * _EffectStrength + baseOpacity :
				Mathf.Lerp(_Image.color.a, baseOpacity, _ChangeSpeed);

		_LastHealth = currentHealth;
		_Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, targetOpacity);
	}
}

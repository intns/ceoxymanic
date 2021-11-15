/*
 * HealthBar.cs
 * Created by: Kman
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Drawing a healthbar on screen
 */

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] HealthManager _HealthManager;
	[SerializeField] Image _Bar;
	[SerializeField] Text _Amount;

	[Header("Misc")]
	[SerializeField] [Range(0, 1)] float _ChangeSpeed;

	void Update()
	{
		_Bar.fillAmount = Mathf.Lerp(_Bar.fillAmount, _HealthManager._HealthPercent, _ChangeSpeed);
		_Amount.text = _HealthManager.GetHealth().ToString();
	}
}

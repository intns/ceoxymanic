/*
 * AmmoBar.cs
 * Created by: Kman 
 * Created on: 21/1/2020 (dd/mm/yy)
 * Created for: Displaying the ammobar
 */

using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Player _Player;
	Gun _CurrentGun = null;

	[SerializeField] Text _CurrentAmmoTotal;
	[SerializeField] Text _CurrentInClip;

	[SerializeField] Image _Bar;

	void Awake()
	{
		// As the player doesn't start with a gun in their hand
		// we can safely turn this off
		_Bar.transform.parent.gameObject.SetActive(false);
	}

	void Update()
	{
		// Check if the gun was changed

		if (_Player._CurrentItem != _CurrentGun)
		{
			Gun playerGun = null;
			if (_Player._CurrentItem != null)
			{
				playerGun = _Player._CurrentItem.GetComponent<Gun>();
			}

			_CurrentGun = playerGun;

			ToggleUI(_CurrentGun != null);
		}

		// As there is no gun, exit out early
		if (_CurrentGun == null)
		{
			return;
		}

		_CurrentInClip.text = _CurrentGun._CurrentInClip.ToString();
		_CurrentAmmoTotal.text = _CurrentGun._CurrentAmmoTotal.ToString();


		_Bar.fillAmount = _CurrentGun._IsReloading
																	 ? _CurrentGun._ReloadTime - _CurrentGun._ReloadTimeLeft / _CurrentGun._ReloadTime
																	 : _CurrentGun._CurrentInClip / (float)_CurrentGun._ClipSize;
	}

	void ToggleUI(bool active)
	{
		GameObject parent = _Bar.transform.parent.gameObject;
		parent.SetActive(active);
	}
}
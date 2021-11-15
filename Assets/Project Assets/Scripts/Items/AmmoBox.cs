/*
 * AmmoBox.cs
 * Created by: intns
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: adding ammo to the current gun
 */

using UnityEngine;

public class AmmoBox : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _AmmoInBox = 25;

    public void OnInteractStart(GameObject interactingParent)
    {
        var playerComponent = interactingParent.GetComponent<Player>();
        if (playerComponent)
        {
            var gun = playerComponent._CurrentItem.GetComponent<Gun>(); // Add check for if the current item is a gun here
            if (gun && gun._CurrentAmmoTotal != gun._MaxAmmo)
            {
                int potential = gun._MaxAmmo - gun._CurrentAmmoTotal;
                int actual = _AmmoInBox < potential ? _AmmoInBox : potential;

                gun._CurrentAmmoTotal += actual;
                _AmmoInBox -= actual;

                if (_AmmoInBox <= 0)
                    Destroy(gameObject);
            }
        }
    }
}

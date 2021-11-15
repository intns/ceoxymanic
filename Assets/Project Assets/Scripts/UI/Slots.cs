/*
 * Slots.cs
 * Created by: Kman 
 * Created on: 23/2/2020 (dd/mm/yy)
 * Created for: Displaying the player's slots on screen
 */

using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Image[] _Slots;
    [SerializeField] Image[] _Icons;
    [SerializeField] Player _Player;


	[Header("Settings")]
	[SerializeField] Color _SelectedColor;
    [SerializeField] Color _UnselectedColor;
	
	private void LateUpdate()
	{
        for (int index = 0; index < _Slots.Length; index++)
        {
            _Slots[index].color = index == _Player._CurrentSlotIndex ? _SelectedColor : _UnselectedColor;
            if (_Player._Inventory[index] != null)
            {
                _Icons[index].enabled = true;
                _Icons[index].sprite = _Player._Inventory[index]._Icon;
            }
            else
            {
                _Icons[index].enabled = false;
            }
        }
    }
}

/*
 * Crosshair.cs
 * Created by: Kman
 * Created on: 3/2/2020 (dd/mm/yy)
 * Created for: Drawing the crosshair
 */
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player _Player;
    [SerializeField] RectTransform _Left;
    [SerializeField] RectTransform _Right;
    [SerializeField] RectTransform _Top;
    [SerializeField] RectTransform _Bottom;

    [SerializeField] float _DistancePerUnitOfSpread = 15;
    [SerializeField] float _BaseDistance = 15;

    Gun _PlayerGun;

    void Update()
    {
        if(_PlayerGun != _Player._CurrentItem) // The player's gun has changed, switch to the new gun
        {
            Gun playerGun = null;
            if (_Player._CurrentItem != null)
            {
                playerGun = _Player._CurrentItem.GetComponent<Gun>();
            }

            _PlayerGun = playerGun;

            if (_PlayerGun == null)
            {
                _Left.anchoredPosition = new Vector2(-_BaseDistance, 0);
                _Right.anchoredPosition = new Vector2(_BaseDistance, 0);
                _Bottom.anchoredPosition = new Vector2(0, -_BaseDistance);
                _Top.anchoredPosition = new Vector2(0, _BaseDistance);
            }
        }

        if (_PlayerGun != null)
        {
            float spread = _PlayerGun._CurrentSpread;
            _Left.anchoredPosition = new Vector2(-spread * _DistancePerUnitOfSpread - _BaseDistance, 0);
            _Right.anchoredPosition = new Vector2(spread * _DistancePerUnitOfSpread + _BaseDistance, 0);
            _Bottom.anchoredPosition = new Vector2(0, -spread * _DistancePerUnitOfSpread - _BaseDistance);
            _Top.anchoredPosition = new Vector2(0, spread * _DistancePerUnitOfSpread + _BaseDistance);
        }
    }
}

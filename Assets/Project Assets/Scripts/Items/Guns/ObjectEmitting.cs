/*
 * ObjectEmitting.cs
 * Created by: Kman 
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: Guns that shoot physical bullets
 */

using UnityEngine;

public class ObjectEmitting : Gun
{
    [Header("Object Emitting Components")]
    [SerializeField] GameObject _Bullet;
    [SerializeField] float _BulletSpeed;

    public override void Shoot()
    {
        float shotsLeft = _BulletsPerShot;
        while (shotsLeft-- > 0)
        {
            float spreadX = Random.Range(-_CurrentSpread, _CurrentSpread);
            float spreadY = Random.Range(-_CurrentSpread, _CurrentSpread);

            Quaternion targetRotation = Quaternion.Euler(spreadX + transform.rotation.x, spreadY + transform.rotation.y, transform.rotation.z);

            GameObject projectile = Instantiate(_Bullet, _BulletSpawnPoint.position, transform.rotation);
            var bullet = projectile.GetComponent<Bullet>();
            bullet._Damage = _BulletDamage;
            bullet._Force = _BulletForce;
            bullet._Speed = _BulletSpeed;
        }

        _AudioSource.PlayOneShot(_ShootNoise, _ShotVolume);

        _CurrentInClip--;
        _CurrentSpread += _SpreadPerShot;

        _TimeTillNextShot = _ShotDelay;
        transform.localPosition += Vector3.back * _ShotRecoil;
    }
}

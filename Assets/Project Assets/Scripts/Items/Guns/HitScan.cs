/*
 * HitScan.cs
 * Created by: Kman 
 * Created on: 22/2/2020 (dd/mm/yy)
 * Created for: Guns that use hitscan
 */

using UnityEngine;
public class HitScan : Gun
{
    [Header("HitScan Components")]
    [SerializeField] GameObject _BulletRay;

    public override void Shoot()
    {
        float shotsLeft = _BulletsPerShot;

        while (shotsLeft-- > 0)
        {
            float spreadX = Random.Range(-_CurrentSpread, _CurrentSpread);
            float spreadY = Random.Range(-_CurrentSpread, _CurrentSpread);

            Vector3 spread = new Vector3(spreadX, spreadY);
            Vector3 direction = _BulletSpawnPoint.forward + (transform.InverseTransformDirection(spread) / _BulletMaxDistance);

            if (Physics.Raycast(_BulletSpawnPoint.position, direction, out RaycastHit hit, _BulletMaxDistance))
            {
                CreateRay(hit.point);

                var targetHealthManager = hit.collider.gameObject.GetComponent<HealthManager>();
                if (targetHealthManager != null)
                {
                    targetHealthManager.RemoveHealth(_BulletDamage);
                    targetHealthManager._HurtOrigin = transform.position;
                    break;
                }

                var targetRigidbody = hit.rigidbody;
                if (targetRigidbody != null)
                {
                    targetRigidbody.AddForce(_BulletSpawnPoint.forward * _BulletForce, ForceMode.Impulse);
                    break;
                }
            }
            else
            {
                CreateRay(transform.position + (direction.normalized * _BulletMaxDistance));
            }
        }

        _AudioSource.PlayOneShot(_ShootNoise, _ShotVolume);

        _CurrentInClip--;
        _CurrentSpread += _SpreadPerShot;

        _TimeTillNextShot = _ShotDelay;
        transform.localPosition += Vector3.back * _ShotRecoil;
    }

    void CreateRay(Vector3 point) // Creates the line the bullet follows
    {
        GameObject go = Instantiate(_BulletRay, _BulletSpawnPoint.position, Quaternion.identity);

        BulletRay ray = go.GetComponent<BulletRay>();
        ray.SetRendererPosition(point);
        StartCoroutine(ray.WaitThenDestroy(0.1f));
    }
}

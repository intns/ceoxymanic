/*
 * HealthPack.cs
 * Created by: intns
 * Created on: 20/2/2020 (dd/mm/yy)
 * Created for: needing the player to gather health
 */

using UnityEngine;

public class HealthPack : Carriable
{
    [Header("Settings")]
    [SerializeField] int _HealthToGain = 10;
    [SerializeField] AudioClip _HealthPickupNoise;

    void Get(HealthManager playerHealth)
    {
        if (playerHealth.GetHealth() >= playerHealth._MaxHealth)
            return;

        AudioSource.PlayClipAtPoint(_HealthPickupNoise, transform.position);
        playerHealth.AddHealth(_HealthToGain);
        Destroy(gameObject);
    }

    public override void Drop()
    {
        base.Drop();
    }
    public override void UseOne(int type, GameObject caller)
    {
        if (type == 1)
            Get(caller.GetComponent<HealthManager>());
    }
    public override void UseTwo(int type, GameObject caller)
    {
        return;
    }
    public override void UseThree(int type, GameObject caller)
    {
        return;
    }

}

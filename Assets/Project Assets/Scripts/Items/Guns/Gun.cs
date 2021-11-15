/*
 * Gun.cs
 * Created by: Kman
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Controlling all guns' actions
 */

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Gun : Carriable
{
	[Header("Gun Components")]
	[SerializeField] protected AudioClip _ShootNoise;
	[SerializeField] protected Transform _BulletSpawnPoint;
	protected AudioSource _AudioSource;

	//Variables that don't fit any category
	[HideInInspector] public bool _CanShoot;

	[Header("Gun Positioning")]
	[SerializeField] Vector3 _AimingPosition;

	[Header("Aiming")]
	bool _IsAiming;

	[Header("Spread")]
	[SerializeField] float _DefaultSpread; // Starting Spread (Not aiming)
	[SerializeField] float _AimingSpread; // The gun's spread while aiming
	[SerializeField] [Range(0, 1)] float _DefaultSpreadTime; // How quickly the gun returns to its target spread
	[SerializeField] [Range(0, 1)] float _AimingSpreadTime; // How quickly the gun returns to its target spread while aiming
	[SerializeField] protected float _SpreadPerShot; // How much the spread increases by per shot
	float _TargetSpread; // Spread the gun is trying to reach
	[HideInInspector] public float _CurrentSpread; // The spread the gun fires with

	[Header("Shooting")]
	public bool _IsAutomatic;

	// Variables that affect the bullet's functions
	[SerializeField] protected float _BulletMaxDistance;
	[SerializeField] protected int _BulletForce;
	[SerializeField] protected int _BulletsPerShot = 1;
	[SerializeField] protected int _BulletDamage;

	// Variables that are used when limiting how fast a gun can shoot
	public float _ShotDelay; // Minimum amount of time between shots
	protected float _TimeTillNextShot;

	[Header("Ammo")]
	public int _ClipSize; // How much ammo can be used before needing to reload
	public int _MaxAmmo;  // The maximum amount of ammo that can be held by the gun
	[HideInInspector] public int _CurrentInClip;
	[HideInInspector] public int _CurrentAmmoTotal;

	[Header("Reloading")]
	public float _ReloadTime;
	[HideInInspector] public bool _IsReloading;
	[HideInInspector] public float _ReloadTimeLeft;

	[Header("Feedback")]
	// Visual feedback
	[SerializeField] protected float _ShotRecoil;
	// Audio Feedback
	[SerializeField] [Range(0, 1)] protected float _ShotVolume;

	void Awake()
	{
		_AudioSource = GetComponent<AudioSource>();

		Aim(false); // Sets default position
		_CurrentInClip = _ClipSize;
		_CurrentAmmoTotal = _MaxAmmo;
	}
	protected override void Update()
	{
		base.Update();

		CalculateSpread();

		if (_CurrentInClip < 1 && !_IsReloading)
		{
			StartReloading();
		}

		if (_TimeTillNextShot > 0)
		{
			_TimeTillNextShot -= Time.deltaTime;
		}
		else
		{
			_TimeTillNextShot = 0;
		}

		_CanShoot = !_IsReloading && _TimeTillNextShot <= 0; // Determine whether this gun can shoot

		if (_ReloadTimeLeft <= 0)
		{
			if (_IsReloading) // Ensure the gun is still reloading
			{
				FinishReloading();
			}

			_ReloadTimeLeft = 0;
		}
		else
		{
			_ReloadTimeLeft -= Time.deltaTime;
		}
	}
	void FixedUpdate()
	{
		if (_IsEquipped)
		{
			_CurrentSpread = Mathf.Lerp(_CurrentSpread, _TargetSpread, (_IsAiming) ? _AimingSpreadTime : _DefaultSpreadTime);
		}
	}

	public override void UseOne(int type, GameObject caller)
	{
		if (_CurrentInClip < 1)
		{
			return;
		}

		if (_CanShoot)
		{
			if (type == 1 && !_IsAutomatic)
			{
				Shoot();
			}
			else if (type == 3 && _IsAutomatic)
			{
				Shoot();
			}
		}
	}
	public override void UseTwo(int type, GameObject caller)
	{
		if (type == 1)
		{
			Aim(true);
		}
		else if (type == 2)
		{
			Aim(false);
		}
	}
	public override void UseThree(int type, GameObject caller)
	{
		StartReloading();
	}
	public override void Drop()
	{
		Aim(false);
		base.Drop();

		_IsReloading = false;
	}
	public void Aim(bool isAiming)
	{
		_IsAiming = isAiming;
		_TargetPosition = _IsAiming ? _AimingPosition : _DefaultPosition;
	}
	public void StartReloading()
	{
		// Can't start reloading if you're already reloading, so this
		// check is required
		if (!_IsReloading && _CurrentInClip != _ClipSize && _CurrentAmmoTotal != 0)
		{
			_IsReloading = true;
			_ReloadTimeLeft = _ReloadTime;
		}
	}
	void FinishReloading()
	{
		int fillPotential = _ClipSize - _CurrentInClip; // Max amount of ammo that can be filled
		int fillActual = _CurrentAmmoTotal < fillPotential ? _CurrentAmmoTotal : fillPotential; // Actual amount of ammo filled

		_CurrentInClip += fillActual;
		_CurrentAmmoTotal -= fillActual;

		_IsReloading = false;
	}
	void CalculateSpread()
	{
		if (!_IsEquipped)
		{
			_TargetSpread = 0;
			return;
		}

		if (_IsAiming)
		{
			_TargetSpread = _AimingSpread;
		}
		else
		{
			_TargetSpread = _DefaultSpread;
		}
	}
	public abstract void Shoot();
}

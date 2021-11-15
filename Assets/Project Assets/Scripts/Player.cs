/*
 * Player.cs
 * Created by: Kman, intns
 * Created on: 9/2/2020 (dd/mm/yy)
 * Created for: Controlling the player
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public enum MouseButton
{
	LMB,
	RMB,
	MMB
}

public class Player : MonoBehaviour, IHealthImpl
{
	[Header("Controls")]
	[SerializeField] KeyCode _SprintKey = KeyCode.LeftShift;
	[SerializeField] KeyCode _JumpKey = KeyCode.Space;
	[SerializeField] KeyCode _ThrowKey = KeyCode.F;
	[SerializeField] KeyCode _InteractKey = KeyCode.E;
	[SerializeField] MouseButton _SideButton = MouseButton.RMB;
	[SerializeField] MouseButton _MainButton = MouseButton.LMB;
	[SerializeField] KeyCode _ReloadKey = KeyCode.R;

	[Header("Grounded")]
	[SerializeField] float _GroundRadius;
	[SerializeField] Vector3 _GroundOffset;
	[SerializeField] LayerMask _WhatIsGround;


	[Header("Movement")]
	[SerializeField] float _MovementSpeed;
	[SerializeField] float _Acceleration;
	[SerializeField] float _Deceleration;
	[SerializeField] float _SprintSpeedMult = 2;
	bool _IsSprinting;

	[Header("Jumping")]
	[SerializeField] float _JumpForce;
	bool _ReadyToJump = false;

	[Header("Crouching")]
	[SerializeField] [Range(0, 1)] float _CrouchSpeedMult = 0.5f;
	[SerializeField] float _CrouchScale;
	[SerializeField] float _CrouchGravityMult;
	bool _IsStartingCrouch;
	Vector3 _MainScale;
	Vector3 _TargetScale;

	[Header("Interacting")]
	[SerializeField] float _InteractRange;

	[Header("Items")]
	[SerializeField] float _ThrowForce = 5;
	[HideInInspector] public Carriable _CurrentItem = null;

	[Header("Inventory")]
	[SerializeField] int _InventorySize;
	[HideInInspector] public int _CurrentSlotIndex;
	public Carriable[] _Inventory;


	[Header("Camera")]
	[SerializeField] float _RotationSpeed = 1f;
	[SerializeField] float _YRotationMin = -90f;
	[SerializeField] float _YRotationMax = 90f;
	float _XRotation = 0;
	float _YRotation = 0;

	[Header("View Bobbing")]
	[SerializeField] float _BobbingSpeed = 0.18f;
	[SerializeField] float _BobbingAmount = 0.2f;
	float _Midpoint = 0.5f;
	float _Timer = 0.0f;

	Camera _MainCamera;
	Rigidbody _Rigidbody;
	HealthManager _HealthManager;
	float _SavedHealth;

	void Awake()
	{
		_HealthManager = GetComponent<HealthManager>();

		Global.RecursiveSetColliders(transform, true);

		_Rigidbody = GetComponent<Rigidbody>();
		_Rigidbody.useGravity = true;

		_MainScale = transform.localScale;
		_TargetScale = _MainScale;

		Cursor.lockState = CursorLockMode.Locked;
		_MainCamera = Camera.main;

		_XRotation = transform.eulerAngles.y;
		_YRotation = transform.eulerAngles.x;

		_CurrentSlotIndex = 0;
	}
	void Update()
	{
		HandleCamera();
		HandleCrouching();
		HandleSprinting();
		ApplyViewBobbing();

		if (Input.GetKeyDown(_JumpKey) && IsGrounded())
		{
			_ReadyToJump = true;
		}

		if (Input.GetKeyDown(_InteractKey))
		{
			Interact();
		}

		if (_CurrentItem != null) // All the things the player can do with items
		{
			// TODO: make not cancer
			if (Input.GetMouseButtonDown((int)_MainButton))
			{
				_CurrentItem.UseOne(1, gameObject);
			}
			else if (Input.GetMouseButtonUp((int)_MainButton))
			{
				_CurrentItem.UseOne(2, gameObject);
			}
			else if (Input.GetMouseButton((int)_MainButton))
			{
				_CurrentItem.UseOne(3, gameObject);
			}

			if (Input.GetMouseButtonDown((int)_SideButton))
			{
				_CurrentItem.UseTwo(1, gameObject);
			}
			else if (Input.GetMouseButtonUp((int)_SideButton))
			{
				_CurrentItem.UseTwo(2, gameObject);
			}
			else if (Input.GetMouseButton((int)_SideButton))
			{
				_CurrentItem.UseTwo(3, gameObject);
			}

			if (Input.GetKeyDown(_ReloadKey))
			{
				_CurrentItem.UseThree(1, gameObject);
			}
			else if (Input.GetKeyUp(_ReloadKey))
			{
				_CurrentItem.UseThree(2, gameObject);
			}
			else if (Input.GetKey(_ReloadKey))
			{
				_CurrentItem.UseThree(3, gameObject);
			}

			if (Input.GetKeyDown(_ThrowKey))
			{
				Drop();
			}
		}

		if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				ChangeSlot(_CurrentSlotIndex - 1);
			}
			else
			{
				ChangeSlot(_CurrentSlotIndex + 1);
			}
		}

		// Handle dynamically pressing 123456789 for accessing inventory
		for (int i = 0; i < _InventorySize; i++)
		{
			if (Input.GetKeyDown(i + KeyCode.Alpha1))
			{
				ChangeSlot(i);
			}
		}

		_Inventory[_CurrentSlotIndex] = _CurrentItem;
	}
	private void FixedUpdate()
	{
		HandleMovement();

		if (_ReadyToJump)
		{
			Jump();
		}
	}
	#region movement functions
	void HandleMovement()
	{
		// Calculate how fast we should be moving
		Vector3 targetVelocity = Vector3.ClampMagnitude(transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _MovementSpeed, _MovementSpeed);

		// Apply a force that attempts to reach our target velocity
		Vector3 velocityChange = targetVelocity - _Rigidbody.velocity;
		velocityChange.x = Mathf.Clamp(velocityChange.x, -_Deceleration, _Acceleration);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -_Deceleration, _Acceleration);
		velocityChange.y = 0;
		_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}
	void HandleSprinting()
	{
		if (Input.GetKeyDown(_SprintKey) && !_IsSprinting)
		{
			_IsSprinting = true;
			_MovementSpeed *= _SprintSpeedMult;
			_BobbingSpeed *= _SprintSpeedMult;
		}
		if (Input.GetKeyUp(_SprintKey) && _IsSprinting)
		{
			_IsSprinting = false;
			_MovementSpeed /= _SprintSpeedMult;
			_BobbingSpeed /= _SprintSpeedMult;
		}
	}
	void Jump()
	{
		_Rigidbody.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
		_ReadyToJump = false;
	}
	void HandleCrouching()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			_IsStartingCrouch = true; // Is only set to true here because we don't want more gravity while returning to normal size
			_TargetScale = new Vector3(_TargetScale.x, _CrouchScale, _TargetScale.z);
			_Midpoint *= _CrouchScale;
			_MovementSpeed *= _CrouchSpeedMult;

		}
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			_TargetScale = _MainScale;
			_Midpoint /= _CrouchScale;
			_MovementSpeed /= _CrouchSpeedMult;
		}

		if (Mathf.Abs(transform.localScale.y - _TargetScale.y) < .1) // No longer transitioning if scales are close enough
		{
			_IsStartingCrouch = false;
		}

		if (_IsStartingCrouch) // Increase gravity while crouching to fall faster, exists to make starting crouching faster
		{
			_Rigidbody.AddForce(Physics.gravity * _CrouchGravityMult);
		}

		transform.localScale = Vector3.Lerp(transform.localScale, _TargetScale, 0.4f); // Smoothly transition into and out of crouching
	}
	void Interact()
	{
		if (Physics.Raycast(_MainCamera.transform.position, _MainCamera.transform.forward, out RaycastHit hit, _InteractRange))
		{
			IInteractable interactable = hit.transform.GetComponent<IInteractable>();

			if (interactable != null)
			{
				interactable.OnInteractStart(gameObject);
			}
		}
	}
	public void Drop()
	{
		_CurrentItem.Drop(); // Drop item gun
		_CurrentItem._Rigidbody.AddForce(_Rigidbody.velocity + _MainCamera.transform.forward * _ThrowForce, ForceMode.Impulse); // Adds a force to the gun when you throw it
		_CurrentItem = null; // Set current item to none because it's no longer held
	}
	#endregion

	#region camera functions
	void HandleCamera()
	{
		Vector2 lookDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // Gets player input

		if (lookDirection == Vector2.zero) //Only run if the player's mouse has moved
		{
			return;
		}

		lookDirection *= _RotationSpeed;
		_XRotation += lookDirection.x;
		_YRotation += lookDirection.y;
		ClampRotation(); //Prevents the camera from over rotating

		_MainCamera.transform.rotation = Quaternion.Euler(-_YRotation, _MainCamera.transform.eulerAngles.y, 0); // Rotate the camera around the X axis to look up or down
		transform.rotation = Quaternion.Euler(0, _XRotation, 0); // Rotates the player around the Y axis to look left or right. Doing this rotates the camera so we don't need to rotate the camera.
	}
	void ClampRotation()
	{
		if (_YRotation < _YRotationMin)
		{
			_YRotation = _YRotationMin;
		}
		else if (_YRotation > _YRotationMax)
		{
			_YRotation = _YRotationMax;
		}
	}
	void ApplyViewBobbing()
	{
		//Code based from http://wiki.unity3d.com/index.php/Headbobber, converted to C#

		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0 || !IsGrounded() || _Rigidbody.velocity.magnitude <= 0.25f)
		{
			_Timer = 0.0f;
		}
		else
		{
			waveslice = Mathf.Sin(_Timer);
			_Timer += _BobbingSpeed;
			if (_Timer > Mathf.PI * 2)
			{
				_Timer -= Mathf.PI * 2;
			}
		}

		Vector3 cameraPosition = _MainCamera.transform.localPosition;
		if (waveslice != 0)
		{
			float translateChange = waveslice * _BobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange *= totalAxes;
			cameraPosition.y = _Midpoint + translateChange;
		}
		else
		{
			cameraPosition.y = _Midpoint;
		}

		_MainCamera.transform.localPosition = cameraPosition;
	}

	#endregion
	// Inventory Functions
	void ChangeSlot(int newIndex)
	{
		if (_CurrentSlotIndex == newIndex)
		{
			return;
		}

		if (_CurrentItem != null) // Store the current item in the inventory
		{
			Gun gun = _CurrentItem.GetComponent<Gun>();
			if (gun != null)
			{
				gun.Aim(false);
			}

			_CurrentItem.transform.localPosition = _CurrentItem._UnequippedPosition;
			_CurrentItem.gameObject.SetActive(false);
		}

		_CurrentSlotIndex = newIndex;

		if (_CurrentSlotIndex > _InventorySize - 1)
		{
			_CurrentSlotIndex = 0;
		}
		else if (_CurrentSlotIndex < 0)
		{
			_CurrentSlotIndex = _InventorySize - 1;
		}

		// Retrieve the stored item and equip it
		_CurrentItem = _Inventory[_CurrentSlotIndex];
		if (_CurrentItem != null)
		{
			_CurrentItem.gameObject.SetActive(true);
		}
	}


	// Misc Functions
	bool IsGrounded()
	{
		return Physics.OverlapSphere(_GroundOffset + transform.position, _GroundRadius, _WhatIsGround).Length != 0;
	}
	public void Die()
	{
		_HealthManager.AddHealth(_SavedHealth - _HealthManager.GetHealth()); // Need to reset health here to prevent endless scene switching
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

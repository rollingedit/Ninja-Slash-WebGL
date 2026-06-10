using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class CharacterMotor : MonoBehaviour
{
	public float gravity;

	public float minSpeed;

	public float maxSpeed;

	public float timeToReachMaxSpeed;

	public float jumpSpeed;

	public float dashSpeed;

	public float dashDistance;

	public float stompSpeed;

	public float bounceSpeed;

	private CharacterController controller;

	private Vector3 moveDirection = Vector3.zero;

	private float distanceToDash;

	private RaycastHit hit;

	private float downwardDirection = 5f;

	private float currentForwardSpeed;

	private float acceleration;

	private bool isJumping;

	private bool isMoving;

	private bool isDoingTutorial;

	private Transform myTransform;

	public float forwardSpeed
	{
		get
		{
			return currentForwardSpeed;
		}
	}

	private void Start()
	{
		Component component = ((Component)this).GetComponent(typeof(CharacterController));
		controller = (CharacterController)(object)((component is CharacterController) ? component : null);
		acceleration = (maxSpeed - minSpeed) / timeToReachMaxSpeed;
		myTransform = ((Component)this).transform;
	}

	private void OnGetObjectFromPool()
	{
		currentForwardSpeed = minSpeed;
	}

	private void Update()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		if (!isMoving)
		{
			return;
		}
		Vector3 position = myTransform.position;
		if (Mathf.Abs(distanceToDash) != 0f)
		{
			float num = Mathf.Abs(distanceToDash);
			float num2 = dashSpeed * Time.deltaTime;
			if (num <= num2)
			{
				moveDirection.x = Mathf.Sign(distanceToDash) * num / Time.deltaTime;
				distanceToDash = 0f;
				((Component)this).BroadcastMessage("OnEndDash");
			}
			else
			{
				moveDirection.x = Mathf.Sign(distanceToDash) * dashSpeed;
				num -= num2;
				distanceToDash = Mathf.Sign(distanceToDash) * num;
			}
		}
		else
		{
			moveDirection.x = 0f;
		}
		if (currentForwardSpeed < maxSpeed)
		{
			currentForwardSpeed = Mathf.Clamp(currentForwardSpeed + ((!isDoingTutorial) ? acceleration : 0f) * Time.deltaTime, minSpeed, maxSpeed);
		}
		moveDirection.z = currentForwardSpeed;
		if (!isJumping && Physics.Raycast(myTransform.position, -Vector3.up, out hit, 10f) && Vector3.Angle(hit.normal, Vector3.up) > 0f)
		{
			Vector3 val = default(Vector3);
			val = new Vector3(0f, 0f - hit.normal.y, moveDirection.z);
			Vector3.Normalize(val);
			CharacterMotor characterMotor = this;
			characterMotor.moveDirection.y = characterMotor.moveDirection.y + val.y * currentForwardSpeed;
		}
		CharacterMotor characterMotor2 = this;
		characterMotor2.moveDirection.y = characterMotor2.moveDirection.y - gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		if (controller.isGrounded)
		{
			moveDirection.y = 0f - downwardDirection;
			if (isJumping)
			{
				isJumping = false;
				((Component)this).BroadcastMessage("OnGround");
			}
			((Component)((Component)Camera.main).transform.parent).SendMessage("AdjustHeight");
		}
		else if (!isJumping)
		{
			isJumping = true;
			((Component)this).BroadcastMessage("OnFalling");
		}
		if (distanceToDash != 0f && (controller.collisionFlags & CollisionFlags.Sides) != 0)
		{
			float num3 = dashSpeed * Time.deltaTime - Mathf.Abs(myTransform.position.x - position.x);
			distanceToDash = (0f - Mathf.Sign(distanceToDash)) * (dashDistance - Mathf.Abs(distanceToDash) - num3);
			if (distanceToDash > 0f)
			{
				((Component)this).BroadcastMessage("OnDoRightBump");
			}
			else
			{
				((Component)this).BroadcastMessage("OnDoLeftBump");
			}
		}
	}

	private void OnGameStart()
	{
		isMoving = true;
		isDoingTutorial = !MonoSingleton<UserData>.instance.IsTutorialDone;
	}

	private void OnPause()
	{
		isMoving = false;
	}

	private void OnResume()
	{
		isMoving = true;
	}

	private void OnRestart()
	{
		OnGameStart();
	}

	private void OnFinishTutorial()
	{
		isDoingTutorial = false;
	}

	private void OnJump()
	{
		isJumping = true;
		moveDirection.y = jumpSpeed;
	}

	private void OnDash(bool isLeft)
	{
		if (Mathf.Abs(distanceToDash) == 0f)
		{
			if (isLeft)
			{
				distanceToDash = 0f - dashDistance;
			}
			else
			{
				distanceToDash = dashDistance;
			}
		}
	}

	private void OnLeftDash()
	{
		OnDash(true);
	}

	private void OnRightDash()
	{
		OnDash(false);
	}

	private void Reset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		moveDirection = Vector3.zero;
		distanceToDash = 0f;
		isMoving = false;
	}

	private void OnDead()
	{
		Reset();
	}

	private void OnPutObjectIntoPool()
	{
		Reset();
	}

	private void OnStomp()
	{
		moveDirection.y = stompSpeed;
	}

	private void OnKilledZombie(ZombieType type)
	{
		if (type == ZombieType.fatso)
		{
			isJumping = true;
			moveDirection.y = bounceSpeed;
			((Component)this).BroadcastMessage("OnBounce");
			((Component)((Component)Camera.main).transform.parent).BroadcastMessage("OnBounce");
		}
	}

	private void OnShieldUsed()
	{
		moveDirection.y = downwardDirection;
		distanceToDash = 0f;
		((Component)this).BroadcastMessage("OnEndDash");
	}

	private void OnSetSpeedToLowest()
	{
		currentForwardSpeed = minSpeed;
	}

	private void OnSetSpeedToHighest()
	{
		currentForwardSpeed = maxSpeed;
	}
}

using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class BerserkerMotor : MonoBehaviour
{
	public AudioClip berserkerSound;

	private CharacterController controller;

	private Vector3 playerPos;

	private Vector3 originalPos;

	private Vector3 moveVelocity = Vector3.zero;

	private Transform myTransform;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		controller = ((Component)this).GetComponent<CharacterController>();
		originalPos = ((Component)this).transform.localPosition;
		myTransform = ((Component)this).transform;
	}

	private void Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		moveVelocity += Physics.gravity * Time.deltaTime;
		controller.Move(moveVelocity * Time.deltaTime);
		if (controller.isGrounded)
		{
			moveVelocity.y = 0f;
		}
		if (myTransform.position.z < playerPos.z)
		{
			moveVelocity = Vector3.zero;
		}
	}

	private void OnStartRush(GameObject player)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		MonoSingleton<SoundManager>.instance.PlaySound(berserkerSound);
		playerPos = player.transform.position;
		moveVelocity = new Vector3(0f, 0f, 0f - player.GetComponent<CharacterMotor>().forwardSpeed);
		((Component)Camera.main).SendMessage("OnStartRush");
	}

	private void OnEnterZombie(ZombieData zData)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		zData.zombieObject.SendMessage("OnCrashedByBoomer", (object)((Component)this).GetComponent<CharacterController>().velocity, (SendMessageOptions)1);
	}

	private void OnEnterDadamiDoor(GameObject door)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		door.SendMessage("OnCrashedByBoomer", (object)((Component)this).GetComponent<CharacterController>().velocity, (SendMessageOptions)1);
	}

	private void OnPutObjectIntoPool()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		moveVelocity = Vector3.zero;
		((Component)this).transform.localPosition = originalPos;
		playerPos = Vector3.zero;
	}
}

using UnityEngine;
using Cinemachine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Camera")]
	public class PlayerCamera : MonoBehaviour
	{
		[Header("Camera Settings")]
		public Player player;
		public float maxDistance = 15f;
		public float initialAngle = 20f;
		public float heightOffset = 1f;

		[Header("Following Settings")]
		public float verticalUpDeadZone = 0.15f;
		public float verticalDownDeadZone = 0.15f;
		public float verticalAirUpDeadZone = 4f;
		public float verticalAirDownDeadZone = 0;
		public float maxVerticalSpeed = 10f;
		public float maxAirVerticalSpeed = 100f;

		[Header("Orbit Settings")]
		public bool canOrbit = true;
		public bool canOrbitWithVelocity = true;
		public float orbitVelocityMultiplier = 5;

		[Range(0, 90)]
		public float verticalMaxRotation = 80;

		[Range(-90, 0)]
		public float verticalMinRotation = -20;

		[Header("Collision Settings")]
		public bool canDetectCollisions = true;
		public LayerMask collisionLayer;
		public float collisionRadius = 0.25f;

		protected float m_cameraDistance;
		protected float m_cameraTargetYaw;
		protected float m_cameraTargetPitch;

		protected Vector3 m_cameraTargetPosition;

		protected CinemachineVirtualCamera m_camera;
		protected Cinemachine3rdPersonFollow m_cameraBody;
		protected CinemachineBrain m_brain;

		protected Transform m_target;

		protected string k_targetName = "Player Follower Camera Target";

		protected virtual void InitializeComponents()
		{
			if (!player)
			{
				player = FindObjectOfType<Player>();
			}

			m_camera = GetComponent<CinemachineVirtualCamera>();
			m_cameraBody = m_camera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
			m_brain = Camera.main.GetComponent<CinemachineBrain>();
		}

		protected virtual void InitializeFollower()
		{
			m_target = new GameObject(k_targetName).transform;
			m_target.position = player.transform.position;
		}

		protected virtual void InitializeCamera()
		{
			m_camera.Follow = m_target.transform;
			m_camera.LookAt = m_target.transform;

			Reset();
		}

		protected virtual bool VerticalFollowingStates()
		{
			return player.states.IsCurrentOfType(typeof(SwimPlayerState)) ||
				player.states.IsCurrentOfType(typeof(PoleClimbingPlayerState)) ||
				player.states.IsCurrentOfType(typeof(WallDragPlayerState)) ||
				player.states.IsCurrentOfType(typeof(LedgeHangingPlayerState)) ||
				player.states.IsCurrentOfType(typeof(LedgeClimbingPlayerState));
		}

		public virtual void Reset()
		{
			m_cameraDistance = maxDistance;
			m_cameraTargetPitch = initialAngle;
			m_cameraTargetYaw = player.transform.rotation.eulerAngles.y;
			m_cameraTargetPosition = player.unsizedPosition + Vector3.up * heightOffset;
			MoveTarget();
			m_brain.ManualUpdate();
		}

		protected virtual void HandleOffset()
		{
			var target = player.unsizedPosition + Vector3.up * heightOffset;
			var previousPosition = m_cameraTargetPosition;
			var targetHeight = previousPosition.y;

			if (player.isGrounded || VerticalFollowingStates())
			{
				if (target.y > previousPosition.y + verticalUpDeadZone)
				{
					var offset = target.y - previousPosition.y - verticalUpDeadZone;
					targetHeight += Mathf.Min(offset, maxVerticalSpeed * Time.deltaTime);
				}
				else if (target.y < previousPosition.y - verticalDownDeadZone)
				{
					var offset = target.y - previousPosition.y + verticalDownDeadZone;
					targetHeight += Mathf.Max(offset, -maxVerticalSpeed * Time.deltaTime);
				}
			}
			else if (target.y > previousPosition.y + verticalAirUpDeadZone)
			{
				var offset = target.y - previousPosition.y - verticalAirUpDeadZone;
				targetHeight += Mathf.Min(offset, maxAirVerticalSpeed * Time.deltaTime);
			}
			else if (target.y < previousPosition.y - verticalAirDownDeadZone)
			{
				var offset = target.y - previousPosition.y + verticalAirDownDeadZone;
				targetHeight += Mathf.Max(offset, -maxAirVerticalSpeed * Time.deltaTime);
			}

			m_cameraTargetPosition = new Vector3(target.x, targetHeight, target.z);
		}

		protected virtual void HandleOrbit()
		{
			if (canOrbit)
			{
				var direction = player.inputs.GetLookDirection();

				if (direction.sqrMagnitude > 0)
				{
					var usingMouse = player.inputs.IsLookingWithMouse();
					float deltaTimeMultiplier = usingMouse ? Time.timeScale : Time.deltaTime;

					m_cameraTargetYaw += direction.x * deltaTimeMultiplier;
					m_cameraTargetPitch -= direction.z * deltaTimeMultiplier;
					m_cameraTargetPitch = ClampAngle(m_cameraTargetPitch, verticalMinRotation, verticalMaxRotation);
				}
			}
		}

		protected virtual void HandleVelocityOrbit()
		{
			if (canOrbitWithVelocity && player.isGrounded)
			{
				var localVelocity = m_target.InverseTransformVector(player.velocity);
				m_cameraTargetYaw += localVelocity.x * orbitVelocityMultiplier * Time.deltaTime;
			}
		}

		protected virtual void MoveTarget()
		{
			m_target.position = m_cameraTargetPosition;
			m_target.rotation = Quaternion.Euler(m_cameraTargetPitch, m_cameraTargetYaw, 0.0f);
			m_cameraBody.CameraDistance = m_cameraDistance;
		}

		protected virtual void HandleCollision()
		{
			if (canDetectCollisions)
			{
				var obstruction = Physics.SphereCast(player.unsizedPosition, collisionRadius, -transform.forward,
					out var hit, maxDistance, collisionLayer, QueryTriggerInteraction.Ignore);

				m_cameraDistance = obstruction ? hit.distance : maxDistance;
			}
		}

		protected virtual void Start()
		{
			InitializeComponents();
			InitializeFollower();
			InitializeCamera();
		}

		protected virtual void LateUpdate()
		{
			HandleOrbit();
			HandleVelocityOrbit();
			HandleOffset();
			MoveTarget();
			HandleCollision();
		}

		private float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360)
			{
				angle += 360;
			}

			if (angle > 360)
			{
				angle -= 360;
			}

			return Mathf.Clamp(angle, min, max);
		}
	}
}

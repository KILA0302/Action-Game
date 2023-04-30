using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Swim Player State")]
	public class SwimPlayerState : PlayerState
	{
		protected override void OnEnter(Player player) => player.velocity *= player.stats.current.waterConversion;

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			if (player.onWater)
			{
				var inputDirection = player.inputs.GetMovementCameraDirection();

				player.WaterAcceleration(inputDirection);
				player.WaterFaceDirection(player.lateralVelocity);

				if (player.position.y < player.water.bounds.max.y)
				{
					if (player.isGrounded)
					{
						player.verticalVelocity = Vector3.zero;
					}

					player.verticalVelocity += Vector3.up * player.stats.current.waterUpwardsForce * Time.deltaTime;
				}
				else
				{
					player.verticalVelocity = Vector3.zero;

					if (player.inputs.GetJumpDown())
					{
						player.Jump(player.stats.current.waterJumpHeight);
						player.states.Change<FallPlayerState>();
					}
				}

				if (!player.isGrounded && player.inputs.GetDive())
				{
					player.verticalVelocity += Vector3.down * player.stats.current.swimDiveForce * Time.deltaTime;
				}

				if (inputDirection.sqrMagnitude == 0)
				{
					player.Decelerate(player.stats.current.swimDeceleration);
				}
			}
			else
			{
				player.states.Change<WalkPlayerState>();
			}
		}

		public override void OnContact(Player player, Collider other)
		{
			player.PushRigidbody(other);
		}
	}
}

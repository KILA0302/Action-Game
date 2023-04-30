using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Spring")]
	public class Spring : MonoBehaviour, IEntityContact
	{
		public float force = 25f;
		public AudioClip clip;

		private AudioSource m_audio;
		protected Collider m_collider;

		/// <summary>
		/// Applies spring force to a given Player.
		/// </summary>
		/// <param name="player">The Player you want to apply force to.</param>
		public void ApplyForce(Player player)
		{
			if (player.verticalVelocity.y <= 0)
			{
				m_audio.PlayOneShot(clip);
				player.verticalVelocity = Vector3.up * force;
			}
		}

		public void OnEntityContact(Entity entity)
		{
			if (entity is Player)
			{
				if ((entity as Player).isAlive)
				{
					if (entity.IsPointUnderStep(m_collider.bounds.max))
					{
						ApplyForce(entity as Player);
						(entity as Player).SetJumps(1);
						(entity as Player).ResetAirSpins();
						(entity as Player).ResetAirDash();
						(entity as Player).states.Change<FallPlayerState>();
					}
				}
			}
		}

		private void Start()
		{
			tag = GameTags.Spring;
			m_collider = GetComponent<Collider>();

			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}
		}
	}
}

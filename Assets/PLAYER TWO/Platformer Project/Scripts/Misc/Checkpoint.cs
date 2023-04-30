using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Checkpoint")]
	public class Checkpoint : MonoBehaviour
	{
		public Transform respawn;
		public AudioClip clip;

		/// <summary>
		/// Invoked when the Checkpoint is activated.
		/// </summary>
		public UnityEvent OnActivate;

		private Collider m_collider;
		private AudioSource m_audio;

		/// <summary>
		/// Returns true if the Checkpoint is activated.
		/// </summary>
		public bool activated { get; private set; }

		/// <summary>
		/// Activates this Checkpoint and set Player respawn transform.
		/// </summary>
		/// <param name="player">The Player you want to set the respawn.</param>
		public virtual void Activate(Player player)
		{
			if (!activated)
			{
				activated = true;
				m_audio.PlayOneShot(clip);
				player.SetRespawn(respawn.position, respawn.rotation);
				OnActivate?.Invoke();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!activated && other.CompareTag(GameTags.Player))
			{
				if (other.TryGetComponent<Player>(out var player))
				{
					Activate(player);
				}
			}
		}

		private void Awake()
		{
			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}

			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}
	}
}

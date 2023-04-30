using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider), typeof(AudioSource))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Breakable")]
	public class Breakable : MonoBehaviour
	{
		public GameObject display;
		public AudioClip clip;

		/// <summary>
		/// Called when this object breaks.
		/// </summary>
		public UnityEvent OnBreak;

		private Collider m_collider;
		private AudioSource m_audio;
		private Rigidbody m_rigidBody;

		public bool broken { get; protected set; }

		public virtual void Break()
		{
			if (!broken)
			{
				if (m_rigidBody)
				{
					m_rigidBody.isKinematic = true;
				}

				broken = true;
				display.SetActive(false);
				m_collider.enabled = false;
				m_audio.PlayOneShot(clip);
				OnBreak?.Invoke();
			}
		}

		private void Start()
		{
			m_audio = GetComponent<AudioSource>();
			m_collider = GetComponent<Collider>();
			TryGetComponent(out m_rigidBody);
		}
	}
}

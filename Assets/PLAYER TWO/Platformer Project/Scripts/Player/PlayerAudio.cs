using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Player))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Audio")]
	public class PlayerAudio : MonoBehaviour
	{
		[Header("Voices")]
		public AudioClip[] jump;
		public AudioClip[] hurt;
		public AudioClip[] attack;
		public AudioClip[] lift;
		public AudioClip[] maneuver;

		[Header("Effects")]
		public AudioClip spin;
		public AudioClip pickUp;
		public AudioClip drop;
		public AudioClip airDive;
		public AudioClip stompSpin;
		public AudioClip stompLanding;
		public AudioClip ledgeGrabbing;
		public AudioClip dash;

		protected Player m_player;
		protected AudioSource m_audio;

		protected virtual void InitializePlayer() => m_player = GetComponent<Player>();

		protected virtual void InitializeAudio()
		{
			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}
		}

		protected virtual void PlayRandom(AudioClip[] clips)
		{
			if (clips != null && clips.Length > 0)
			{
				var index = Random.Range(0, clips.Length);

				if (clips[index])
				{
					m_audio.Stop();
					m_audio.PlayOneShot(clips[index]);
				}
			}
		}

		protected virtual void InitializeCallbacks()
		{
			m_player.playerEvents.OnJump.AddListener(() => PlayRandom(jump));
			m_player.playerEvents.OnHurt.AddListener(() => PlayRandom(hurt));
			m_player.playerEvents.OnThrow.AddListener(() => m_audio.PlayOneShot(drop));
			m_player.playerEvents.OnStompStarted.AddListener(() => m_audio.PlayOneShot(stompSpin));
			m_player.playerEvents.OnStompLanding.AddListener(() => m_audio.PlayOneShot(stompLanding));
			m_player.playerEvents.OnLedgeGrabbed.AddListener(() => m_audio.PlayOneShot(ledgeGrabbing));
			m_player.playerEvents.OnLedgeClimbing.AddListener(() => PlayRandom(lift));
			m_player.playerEvents.OnBackflip.AddListener(() => PlayRandom(maneuver));
			m_player.playerEvents.OnDashStarted.AddListener(() => m_audio.PlayOneShot(dash));

			m_player.playerEvents.OnPickUp.AddListener(() =>
			{
				PlayRandom(lift);
				m_audio.PlayOneShot(pickUp);
			});


			m_player.playerEvents.OnSpin.AddListener(() =>
			{
				PlayRandom(attack);
				m_audio.PlayOneShot(spin);
			});

			m_player.playerEvents.OnAirDive.AddListener(() =>
			{
				PlayRandom(attack);
				m_audio.PlayOneShot(airDive);
			});
		}

		protected virtual void Start()
		{
			InitializeAudio();
			InitializePlayer();
			InitializeCallbacks();
		}
	}
}

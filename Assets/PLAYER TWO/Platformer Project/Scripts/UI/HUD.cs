using UnityEngine;
using UnityEngine.UI;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/UI/HUD")]
	public class HUD : MonoBehaviour
	{
		public string retriesFormat = "00";
		public string coinsFormat = "000";
		public string healthFormat = "0";

		[Header("UI Elements")]
		public Text retries;
		public Text coins;
		public Text health;
		public Text timer;
		public Image[] starsImages;

		protected Game m_game;
		protected LevelScore m_score;
		protected Player m_player;

		protected float timerStep;
		protected static float timerRefreshRate = .1f;

		/// <summary>
		/// Set the coin counter to a given value.
		/// </summary>
		protected virtual void UpdateCoins(int value)
		{
			coins.text = value.ToString(coinsFormat);
		}

		/// <summary>
		/// Set the retries counter to a given value.
		/// </summary>
		protected virtual void UpdateRetries(int value)
		{
			retries.text = value.ToString(retriesFormat);
		}

		/// <summary>
		/// Called when the Player Health changed.
		/// </summary>
		protected virtual void UpdateHealth()
		{
			health.text = m_player.health.current.ToString(healthFormat);
		}

		/// <summary>
		/// Set the stars images enabled state to match a boolean array.
		/// </summary>
		protected virtual void UpdateStars(bool[] value)
		{
			for (int i = 0; i < starsImages.Length; i++)
			{
				starsImages[i].enabled = value[i];
			}
		}

		/// <summary>
		/// Set the timer text to the Level Score time.
		/// </summary>
		protected virtual void UpdateTimer()
		{
			timerStep += Time.deltaTime;

			if (timerStep >= timerRefreshRate)
			{
				var time = m_score.time;
				timer.text = GameLevel.FormattedTime(m_score.time);
				timerStep = 0;
			}
		}

		/// <summary>
		/// Called to force an updated on the HUD.
		/// </summary>
		public virtual void Refresh()
		{
			UpdateCoins(m_score.coins);
			UpdateRetries(m_game.retries);
			UpdateHealth();
			UpdateStars(m_score.stars);
		}

		protected virtual void Awake()
		{
			m_game = Game.instance;
			m_score = LevelScore.instance;
			m_player = FindObjectOfType<Player>();

			m_score.OnScoreLoaded.AddListener(() =>
			{
				m_score.OnCoinsSet.AddListener(UpdateCoins);
				m_score.OnStarsSet.AddListener(UpdateStars);
				m_game.OnRetriesSet.AddListener(UpdateRetries);
				m_player.health.onChange.AddListener(UpdateHealth);
				Refresh();
			});
		}

		protected virtual void Update() => UpdateTimer();
	}
}

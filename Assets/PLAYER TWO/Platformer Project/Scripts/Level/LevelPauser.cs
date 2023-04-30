using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Level/Level Pauser")]
	public class LevelPauser : Singleton<LevelPauser>
	{
		/// <summary>
		/// Called when the Level is Paused.
		/// </summary>
		public UnityEvent OnPause;

		/// <summary>
		/// Called when the Level is unpaused.
		/// </summary>
		public UnityEvent OnUnpause;

		public UIAnimator pauseScreen;

		/// <summary>
		/// Returns true if it's possible to pause the Level.
		/// </summary>
		public bool canPause { get; set; }

		/// <summary>
		/// Returns true if the Level is paused.
		/// </summary>
		public bool paused { get; protected set; }

		/// <summary>
		/// Sets the pause state based on a given value.
		/// </summary>
		/// <param name="value">The state you want to set the pause to.</param>
		public virtual void Pause(bool value)
		{
			if (paused != value)
			{
				if (!paused)
				{
					if (canPause)
					{
						Game.LockCursor(false);
						paused = true;
						Time.timeScale = 0;
						pauseScreen.SetActive(true);
						pauseScreen?.Show();
						OnPause?.Invoke();
					}
				}
				else
				{
					Game.LockCursor();
					paused = false;
					Time.timeScale = 1;
					pauseScreen?.Hide();
					OnUnpause?.Invoke();
				}
			}
		}
	}
}

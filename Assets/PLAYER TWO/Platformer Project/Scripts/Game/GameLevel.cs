using System;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[Serializable]
	public class GameLevel
	{
		public bool locked;
		public string scene;
		public string name;
		public string description;
		public Sprite image;

		/// <summary>
		/// Returns the amount of coins collected in the level.
		/// </summary>
		public int coins { get; set; }

		/// <summary>
		/// Returns the time in which this level has been beaten.
		/// </summary>
		/// <value></value>
		public float time { get; set; }

		/// <summary>
		/// Returns the array of collected or non collected stars.
		/// </summary>
		public bool[] stars { get; set; } = new bool[StarsPerLevel];

		/// <summary>
		/// The amount of existent stars on each Level.
		/// </summary>
		public static readonly int StarsPerLevel = 3;

		/// <summary>
		/// Loads this Game Level state from a given Game Data.
		/// </summary>
		/// <param name="data">The Game Data to read the state from.</param>
		public virtual void LoadState(LevelData data)
		{
			locked = data.locked;
			coins = data.coins;
			time = data.time;
			stars = data.stars;
		}

		/// <summary>
		/// Returns this Level Data of this Game Level to be used by the Data Layer.
		/// </summary>
		public virtual LevelData ToData()
		{
			return new LevelData()
			{
				locked = this.locked,
				coins = this.coins,
				time = this.time,
				stars = this.stars
			};
		}

		/// <summary>
		/// Returns a given time in string fromatted as 00'00"00.
		/// </summary>
		/// <param name="time">The time you want to fromat.</param>
		public static string FormattedTime(float time)
		{
			var minutes = Mathf.FloorToInt(time / 60f);
			var seconds = Mathf.FloorToInt(time % 60f);
			var milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
			return minutes.ToString("0") + "'" + seconds.ToString("00") + "\"" + milliseconds.ToString("00");
		}
	}
}

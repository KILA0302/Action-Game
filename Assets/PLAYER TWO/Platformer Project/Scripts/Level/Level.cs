using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Level/Level")]
	public class Level : Singleton<Level>
	{
		protected Player m_player;

		/// <summary>
		/// Returns the Player activated in the current Level.
		/// </summary>
		public Player player
		{
			get
			{
				if (!m_player)
				{
					m_player = FindObjectOfType<Player>();
				}

				return m_player;
			}
		}
	}
}

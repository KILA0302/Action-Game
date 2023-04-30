using System;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[Serializable]
	public class EntityEvents
	{
		/// <summary>
		/// Called when the Entity lands on the ground.
		/// </summary>
		public UnityEvent OnGroundEnter;

		/// <summary>
		/// Called when the Entity leaves the ground.
		/// </summary>
		public UnityEvent OnGroundExit;
	}
}

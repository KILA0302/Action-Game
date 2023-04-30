using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Rotator")]
	public class Rotator : MonoBehaviour
	{
		public Space space;
		public Vector3 eulers = new Vector3(0, -180, 0);

		private void LateUpdate()
		{
			transform.Rotate(eulers * Time.deltaTime, space);
		}
	}
}

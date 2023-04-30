using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(CapsuleCollider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Pole")]
	public class Pole : MonoBehaviour
	{
		/// <summary>
		/// Returns the Collider of this Pole.
		/// </summary>
		public new CapsuleCollider collider { get; private set; }

		/// <summary>
		/// The radius of this Pole.
		/// </summary>
		public float radius => collider.radius;

		/// <summary>
		/// The center point of this Pole.
		/// </summary>
		public Vector3 center => transform.position;

		/// <summary>
		/// Returns the direction of a given Transform to face this Pole.
		/// </summary>
		/// <param name="other">The transform you want to use.</param>
		/// <returns>The direction from the Transform to the Pole.</returns>
		public Vector3 GetDirectionToPole(Transform other) => GetDirectionToPole(other, out _);

		/// <summary>
		/// Returns the direction of a given Transform to face this Pole.
		/// </summary>
		/// <param name="other">The transform you want to use.</param>
		/// <param name="distance">The distance from the pole center.</param>
		/// <returns>The direction from the Transform to the Pole.</returns>
		public Vector3 GetDirectionToPole(Transform other, out float distance)
		{
			var target = new Vector3(center.x, other.position.y, center.z) - other.position;
			distance = target.magnitude;
			return target / distance;
		}

		/// <summary>
		/// Returns a point clamped to the Pole height.
		/// </summary>
		/// <param name="point">The point you want to clamp.</param>
		/// <param name="offset">Offset to adjust min and max height.</param>
		/// <returns>The point within the Pole height.</returns>
		public Vector3 ClampPointToPoleHeight(Vector3 point, float offset)
		{
			var minHeight = collider.bounds.min.y + offset;
			var maxHeight = collider.bounds.max.y - offset;
			var clampedHeight = Mathf.Clamp(point.y, minHeight, maxHeight);
			return new Vector3(point.x, clampedHeight, point.z);
		}

		private void Awake()
		{
			tag = GameTags.Pole;
			collider = GetComponent<CapsuleCollider>();
		}
	}
}

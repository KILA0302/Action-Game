using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Waypoint/Waypoint Manager")]
	public class WaypointManager : MonoBehaviour
	{
		public WaypointMode mode;
		public float waitTime;
		public List<Transform> waypoints;

		private Transform m_current;

		private bool m_pong;
		private bool m_changing;

		/// <summary>
		/// The instance of the current Waypoint.
		/// </summary>
		public Transform current
		{
			get
			{
				if (!m_current)
				{
					m_current = waypoints[0];
				}

				return m_current;
			}

			private set { m_current = value; }
		}

		/// <summary>
		/// Returns the index of the current Waypoint.
		/// </summary>
		public int index => waypoints.IndexOf(current);

		/// <summary>
		/// Changes the current Waypoint to the next one based on the Waypoint Mode.
		/// </summary>
		public virtual void Next()
		{
			if (m_changing)
			{
				return;
			}

			if (mode == WaypointMode.PingPong)
			{
				if (!m_pong)
				{
					m_pong = (index + 1 == waypoints.Count);
				}
				else
				{
					m_pong = (index - 1 >= 0);
				}

				var next = !m_pong ? index + 1 : index - 1;
				StartCoroutine(Change(next));
			}
			else if (mode == WaypointMode.Loop)
			{
				if (index + 1 < waypoints.Count)
				{
					StartCoroutine(Change(index + 1));
				}
				else
				{
					StartCoroutine(Change(0));
				}
			}
			else if (mode == WaypointMode.Once)
			{
				if (index + 1 < waypoints.Count)
				{
					StartCoroutine(Change(index + 1));
				}
			}
		}

		private IEnumerator Change(int to)
		{
			m_changing = true;
			yield return new WaitForSeconds(waitTime);
			current = waypoints[to];
			m_changing = false;
		}
	}
}

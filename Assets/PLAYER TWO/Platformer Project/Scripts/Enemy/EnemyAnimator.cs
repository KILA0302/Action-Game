using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Enemy))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Enemy/Enemy Animator")]
	public class EnemyAnimator : MonoBehaviour
	{
		public Animator animator;

		private Enemy m_enemy;

		private void Start()
		{
			m_enemy = GetComponent<Enemy>();
		}

		private void LateUpdate()
		{
			var lateralSpeed = m_enemy.lateralVelocity.magnitude;
			var verticalSpeed = m_enemy.verticalVelocity.y;
			var health = m_enemy.health.current;
			animator.SetFloat("Lateral Speed", lateralSpeed);
			animator.SetFloat("Vertical Speed", verticalSpeed);
			animator.SetInteger("Health", health);
		}
	}
}

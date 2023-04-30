using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Entity/Entity Hitbox")]
	public class EntityHitbox : MonoBehaviour
	{
		[Header("Attack Settings")]
		public bool breakObjects;
		public int damage = 1;

		[Header("Rebound Settings")]
		public bool rebound;
		public float reboundMinForce = 10f;
		public float reboundMaxForce = 25f;

		[Header("Push Back Settings")]
		public bool pushBack;
		public float pushBackMinMagnitude = 5f;
		public float pushBackMaxMagnitude = 10f;

		protected Entity m_entity;
		protected Collider m_collider;

		protected virtual void InitializeEntity()
		{
			if (!m_entity)
			{
				m_entity = GetComponentInParent<Entity>();
			}
		}

		protected virtual void InitializeCollider()
		{
			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}

		protected virtual void HandleCollision(Collider other)
		{
			if (other != m_entity.controller)
			{
				if (other.TryGetComponent(out Entity target))
				{
					HandleEntityAttack(target);
					HandleRebound();
					HandlePushBack();
				}
				else if (other.TryGetComponent(out Breakable breakable))
				{
					HandleBreakableObject(breakable);
				}
			}
		}

		protected virtual void HandleEntityAttack(Entity other)
		{
			other.ApplyDamage(damage);
		}

		protected virtual void HandleBreakableObject(Breakable breakable)
		{
			if (breakObjects)
			{
				breakable.Break();
			}
		}

		protected virtual void HandleRebound()
		{
			if (rebound)
			{
				var force = -m_entity.velocity.y;
				force = Mathf.Clamp(force, reboundMinForce, reboundMaxForce);
				m_entity.verticalVelocity = Vector3.up * force;
			}
		}

		protected virtual void HandlePushBack()
		{
			if (pushBack)
			{
				var force = m_entity.lateralVelocity.magnitude;
				force = Mathf.Clamp(force, pushBackMinMagnitude, pushBackMaxMagnitude);
				m_entity.lateralVelocity = -transform.forward * force;
			}
		}

		protected virtual void HandleCustomCollision(Collider other) { }

		protected virtual void Start()
		{
			InitializeEntity();
			InitializeCollider();
		}

		protected void OnTriggerEnter(Collider other)
		{
			HandleCollision(other);
			HandleCustomCollision(other);
		}
	}
}

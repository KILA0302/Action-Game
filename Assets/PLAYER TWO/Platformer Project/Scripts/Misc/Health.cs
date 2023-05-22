using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Health")]
	public class Health : MonoBehaviour
	{
        //HP�̏����l
		public int initial = 3;
        //HP�̍ő�l
		public int max = 3;
        //�_���[�W��������Ă���̖��G����
		public float coolDown = 1f;

		/// <summary>
		/// Called when the health count changed.
		/// </summary>
		public UnityEvent onChange;

		/// <summary>
		/// Called when it receives damage.
		/// </summary>sa
		public UnityEvent onDamage;

        //���݂�HP
		protected int m_currentHealth;
        //�_���[�W��������Ă���̌o�ߎ���
		protected float m_lastDamageTime;

		/// <summary>
		/// Returns the current amount of health.
		/// </summary>
		public int current
		{
			get { return m_currentHealth; }

			protected set
			{
				var last = m_currentHealth;

				if (value != last)
				{
					m_currentHealth = Mathf.Clamp(value, 0, max);
					onChange?.Invoke();
				}
			}
		}

		/// <summary>
		/// HP�����邩��Ԃ�
		/// </summary>
		public virtual bool isEmpty => current == 0;

		/// <summary>
		/// �U����������Ă���̖��G���ԃt���O�𗧂Ă�
		/// </summary>
		public virtual bool recovering => Time.time < m_lastDamageTime + coolDown;

		/// <summary>
		/// HP�������l�ɕύX
		/// </summary>
		/// <param name="amount">The total health you want to set.</param>
		public virtual void Set(int amount) => current = amount;

		/// <summary>
		/// HP�𑝉�������
		/// </summary>
		/// <param name="amount">The amount you want to increase.</param>
		public virtual void Increase(int amount) => current += amount;

		/// <summary>
		/// �_���[�W��^����
		/// </summary>
		/// <param name="amount">The amount you want to decrease.</param>
		public virtual void Damage(int amount)
		{
			if (!recovering)
			{
				current -= Mathf.Abs(amount);
				m_lastDamageTime = Time.time;
				onDamage?.Invoke();
			}
		}

		/// <summary>
		/// HP�̏�����
		/// </summary>
		public virtual void Reset() => current = initial;

		protected virtual void Awake() => current = initial;
	}
}

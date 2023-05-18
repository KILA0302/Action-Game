using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Health")]
	public class Health : MonoBehaviour
	{
        //HPの初期値
		public int initial = 3;
        //HPの最大値
		public int max = 3;
        //ダメージをくらってからの無敵時間
		public float coolDown = 1f;

		/// <summary>
		/// Called when the health count changed.
		/// </summary>
		public UnityEvent onChange;

		/// <summary>
		/// Called when it receives damage.
		/// </summary>sa
		public UnityEvent onDamage;

        //現在のHP
		protected int m_currentHealth;
        //ダメージをくらってからの経過時間
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
		/// HPがあるかを返す
		/// </summary>
		public virtual bool isEmpty => current == 0;

		/// <summary>
		/// 攻撃をくらってからの無敵時間フラグを立てる
		/// </summary>
		public virtual bool recovering => Time.time < m_lastDamageTime + coolDown;

		/// <summary>
		/// HPを引数値に変更
		/// </summary>
		/// <param name="amount">The total health you want to set.</param>
		public virtual void Set(int amount) => current = amount;

		/// <summary>
		/// HPを増加させる
		/// </summary>
		/// <param name="amount">The amount you want to increase.</param>
		public virtual void Increase(int amount) => current += amount;

		/// <summary>
		/// ダメージを与える
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
		/// HPの初期化
		/// </summary>
		public virtual void Reset() => current = initial;

		protected virtual void Awake() => current = initial;
	}
}

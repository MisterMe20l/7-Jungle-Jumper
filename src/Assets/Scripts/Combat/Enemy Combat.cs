using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyCombat : MonoBehaviour, ICombat
    {
        [SerializeField] Enemy enemy1;

        [SerializeField] Animator m_anmEnemy;
        [SerializeField] Animator m_anmDeath;
        [SerializeField] SpriteRenderer m_srEnemy;
        [SerializeField] BoxCollider2D m_cldEnemy;

        public void TakeDamage(int _damage)
        {
            if (enemy1.awake)
            {
                enemy1.m_health -= _damage;
                m_anmEnemy.Play("TakeDmg");
                if (enemy1.m_health <= 0)
                {
                    enemy1.awake = false;
                    m_srEnemy.color = new Color(255, 255, 255, 0);
                    Destroy(m_cldEnemy);

                    m_anmDeath.Play("Death");

                    Destroy(this);
                }
            }
        }
    }
}

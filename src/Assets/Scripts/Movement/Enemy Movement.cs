using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IStartFight
{
    [SerializeField] Enemy enemy1;

    [SerializeField] LayerMask m_groundLayer;
    [SerializeField] Transform m_trfJumpCheck;
    [SerializeField] Transform m_trfPlayer;

    [SerializeField] AudioSource m_running;
    [SerializeField] AudioClip m_runningClip;
    [SerializeField] Vector2 m_runningPitchRange;

    [SerializeField] Animator m_anmEnemy;
    [SerializeField] SpriteRenderer m_srEnemy;
    [SerializeField] AudioSource m_roar;

    [SerializeField] float m_jumpForce;
    [SerializeField] float m_speed;
    [SerializeField] float m_jumpCD_s;
    [SerializeField] int maxHealth;

    Rigidbody2D rbEnemy;
    float timer;
    bool canJump;
    Vector2 startPos;

    private void Awake()
    {
        enemy1.m_health = maxHealth;
        enemy1.awake = false;
        enemy1.alive = true;
        rbEnemy = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (enemy1.awake)
        {
            HorizontalMovement();
            VerticalMovement();
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (rbEnemy.velocity.x < 0.1f)
            m_srEnemy.flipX = true;
        else if (rbEnemy.velocity.x > 0.1f)
            m_srEnemy.flipX = false;

        m_anmEnemy.SetFloat("xSpeed", rbEnemy.velocity.x);
        m_anmEnemy.SetFloat("ySpeed", rbEnemy.velocity.y);

        if (!enemy1.alive)
        {
            Destroy(m_anmEnemy);
        }
    }

    private void VerticalMovement()
    {
        if (!canJump)
        {
            timer += Time.deltaTime;
            if (timer > m_jumpCD_s)
            {
                canJump = true;
                timer = 0;
            }
        }

        if (canJump && JumpCheck())
        {
            canJump = false;
            rbEnemy.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HorizontalMovement()
    {
        var direction = m_trfPlayer.position - transform.position;
        var velocity = new Vector2(direction.x, direction.y).normalized * m_speed;
        rbEnemy.velocity = new Vector2(velocity.x, rbEnemy.velocity.y);
    }

    private bool JumpCheck()
    {
        return Physics2D.OverlapBox(m_trfJumpCheck.position,
            m_trfJumpCheck.lossyScale, 0, m_groundLayer) != null;
    }

    public void PlayIndividualStepSound()
    {
        m_running.pitch = Random.Range(m_runningPitchRange.x, m_runningPitchRange.y);
        m_running.PlayOneShot(m_runningClip);
    }

    public void StartFight()
    {
        StartCoroutine(IStartFight());
    }

    private IEnumerator IStartFight()
    {
        m_roar.volume = 1.0f;
        m_roar.Play();

        yield return new WaitForSeconds(3);

        enemy1.awake = true;
        m_roar.volume = 0.5f;

        yield return new WaitForSeconds(3);

        while (enemy1.awake)
        {
            m_roar.Play();
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    public void RestartFight()
    {
        StartCoroutine(IRestartFight());
    }

    private IEnumerator IRestartFight()
    {
        enemy1.awake = false;
        yield return new WaitForSeconds(2);
        enemy1.m_health = maxHealth;
        transform.position = startPos;
    }
}

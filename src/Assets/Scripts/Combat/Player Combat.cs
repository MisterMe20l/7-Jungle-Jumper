using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour, ICombat, IStartFight
{
    GameInput gameInput;

    [SerializeField] Weapons weapon;
    [SerializeField] Player player1;

    [SerializeField] AudioSource m_takingDamage;
    [SerializeField] AudioSource m_dying;
    [SerializeField] AudioSource m_shooting;

    [SerializeField] SpriteRenderer m_srPlayer;
    [SerializeField] Animator m_anmPlayer;
    [SerializeField] Animator m_anmDeath;

    [SerializeField] Transform m_trfEnemyCheck;
    [SerializeField] LayerMask m_enemyLayer;

    [SerializeField] Transform m_rotationPoint;
    [SerializeField] Transform m_shootingPoint;
    [SerializeField] SpriteRenderer m_srAimer;

    [SerializeField] float m_damageCD_s;
    [SerializeField] int maxHealth;
    bool shootInput = false; 
    bool canFire;
    float shootingTimer;
    bool canHurt;
    float damageTimer;

    Color normalColor = new Color(255, 255, 255, 255);
    Color invisibleColor = new Color(255, 255, 255, 0);

    InputAction shoot;
    InputAction aim;

    Camera mainCam;
    Vector3 mousePos;
    Vector2 aimInput;

    private void Start()
    {
        player1.health = maxHealth;
        player1.waiting = false;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        shoot = gameInput.Player.Shoot;
        shoot.Enable();
        shoot.performed += OnShootInput;
        shoot.canceled += OnShootInput;

        aim = gameInput.Player.Aim;
        aim.Enable();
        aim.performed += OnAimInput;
    }

    private void OnDisable()
    {
        shoot.Disable();
        aim.Disable();
    }

    private void Update()
    {
        if (!player1.waiting)
        {
            UpdateAimer();
            if (player1.canShoot) ShootInput();
            EnemyMeleeHit();
        }
    }

    private void EnemyMeleeHit()
    {
        if (!canHurt)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > m_damageCD_s)
            {
                canHurt = true;
                damageTimer = 0;
            }
        }

        if (canHurt && EnemyMeleeCheck())
        {
            canHurt = false;
            TakeDamage(10);
        }
    }

    private void UpdateAimer()
    {
        mousePos = mainCam.ScreenToWorldPoint(aimInput);

        var rot = mousePos - m_rotationPoint.position;
        var rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;

        m_rotationPoint.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void OnShootInput(InputAction.CallbackContext context) // SHOOT
    {
        shootInput = context.performed;
    }

    private void OnAimInput(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }

    private void ShootInput()
    {
        if (!canFire)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer > weapon.m_firingRate_s)
            {
                canFire = true;
                shootingTimer = 0;
            }
        }

        if (canFire && shootInput)
        {
            canFire = false;
            m_shooting.Play();
            Instantiate(weapon.m_bulletPrefab, m_shootingPoint.position, m_rotationPoint.rotation);
        }
    }

    public void TakeDamage(int _damage)
    {
        player1.health -= _damage;
        m_anmPlayer.Play("TakeDmg");
        m_takingDamage.Play();
    }


    private bool EnemyMeleeCheck()
    {
        return Physics2D.OverlapBox(m_trfEnemyCheck.position,
            m_trfEnemyCheck.lossyScale, 0, m_enemyLayer) != null;
    }

    public void StartFight()
    {
        StartCoroutine(IStartFight());
    }

    private IEnumerator IStartFight()
    {
        player1.waiting = true;
        yield return new WaitForSeconds(3);
        player1.waiting = false;
    }

    public void RestartFight()
    {
        m_dying.Play();
        StartCoroutine(IRespawn());
    }

    private IEnumerator IRespawn()
    {
        yield return new WaitForEndOfFrame();

        player1.waiting = true;

        m_srPlayer.color = invisibleColor;
        m_srAimer.color = invisibleColor;
        m_anmDeath.Play("Death");

        yield return new WaitForSeconds(1.5f);

        transform.position = player1.currentCheckpoint.transform.position;
        m_srPlayer.color = normalColor;
        m_srAimer.color = normalColor;

        yield return new WaitForSeconds(1.5f);

        player1.health = maxHealth;
        player1.waiting = false;
    }
}

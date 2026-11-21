using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    GameInput gameInput;
    [SerializeField] Player player1;

    [SerializeField] AudioSource m_jumpingLanding;
    [SerializeField] AudioSource m_running;
    [SerializeField] AudioClip m_runningClip;
    [SerializeField] Vector2 m_runningPitchRange;

    [SerializeField] SpriteRenderer m_srPlayer;
    [SerializeField] Animator m_anmPlayer;
    [SerializeField] Rigidbody2D m_rbPlayer;

    [SerializeField] Transform m_trfGroundCheck;
    [SerializeField] Transform m_trfWallCheck;
    [SerializeField] LayerMask m_groundLayer;

    [SerializeField] float m_speed;
    [SerializeField] float m_acceleration;
    [SerializeField] float m_deceleration;
    [SerializeField] float m_jumpForce;

    InputAction move;
    InputAction jump;
    InputAction hold;
    Vector2 moveInput;
    int jumpCount;
    int wallHoldCount;
    bool holdInput = false;
    bool holdingWall;
    bool playingRunning;
    bool landed;

    bool p_holdingWall
    {
        get => holdingWall;
        set
        {
            if (holdingWall == value) return;
            holdingWall = value;
            if (holdingWall) wallHoldCount++;
        }
    }

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        move = gameInput.Player.Move;
        move.Enable();
        move.performed += OnMoveInput;
        move.canceled += OnMoveInput;

        jump = gameInput.Player.Jump;
        jump.Enable();
        jump.performed += OnJumpInput;

        hold = gameInput.Player.Hold;
        hold.Enable();
        hold.performed += OnHoldInput;
        hold.canceled += OnHoldInput;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        hold.Disable();
    }

    private void FixedUpdate()
    {
        if (!player1.waiting)
        {
            HorizontalMove();
            GroundAndWallCheck();
        }
        else
        {
            m_rbPlayer.velocity = Vector3.zero;
            moveInput = Vector3.zero;
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        m_anmPlayer.SetFloat("ySpeed", m_rbPlayer.velocity.y);
        m_anmPlayer.SetFloat("xSpeed", Mathf.Abs(m_rbPlayer.velocity.x));
        m_anmPlayer.SetBool("Walled", holdingWall);
    }

    private void GroundAndWallCheck()
    {
        bool isGrounded = IsGrounded();
        bool isWalled = IsWalled();

        if (isGrounded)
        {
            jumpCount = 0;
            wallHoldCount = 0;
        }
        else if (!isGrounded && jumpCount == 0) jumpCount = 1;

        if (holdInput && isWalled && wallHoldCount <= 1)
        {
            m_rbPlayer.velocity = Vector2.zero;
            jumpCount = 0;
            p_holdingWall = true;
        }
        else
            p_holdingWall = false;
    }

    private void HorizontalMove()
    {
        var moving = Mathf.Abs(m_rbPlayer.velocity.x) > 0f;
        if (Mathf.Abs(moveInput.x) > 0f)
        {
            if (moveInput.x < 0f)
                m_srPlayer.flipX = true;
            else //if (moveInput.x > 0f)
                m_srPlayer.flipX = false;

            m_rbPlayer.AddForce(new Vector2(moveInput.x * m_acceleration, 0));
        }
        else
        {
            m_rbPlayer.velocity = new Vector2(Mathf.MoveTowards(m_rbPlayer.velocity.x, 0f, m_deceleration * Time.fixedDeltaTime), m_rbPlayer.velocity.y);
        }
        m_rbPlayer.velocity = new Vector2(Mathf.Clamp(m_rbPlayer.velocity.x, -m_speed, m_speed), m_rbPlayer.velocity.y);

        UpdateMovementSound(moving);
    }

    private void UpdateMovementSound(bool _moving)
    {
        var grounded = IsGrounded();

        m_running.volume = Mathf.Abs(m_rbPlayer.velocity.x) / m_speed;

        if (!grounded) landed = false;

        if (!landed && grounded)
        {
            landed = true;
            m_jumpingLanding.Play();
        }
    }

    public void PlayIndividualStepSound()
    {
        m_running.pitch = Random.Range(m_runningPitchRange.x, m_runningPitchRange.y);
        m_running.PlayOneShot(m_runningClip);
    }

    public void OnMoveInput(InputAction.CallbackContext context) // WALK
    {
        if (!player1.waiting)
            moveInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context) // JUMP
    {
        if (!player1.waiting && jumpCount < 2)
        {
            holdInput = false;
            m_jumpingLanding.Play();
            m_rbPlayer.velocity = new Vector2(m_rbPlayer.velocity.x, 0f);
            m_rbPlayer.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            if (jumpCount == 2) m_anmPlayer.Play("DJump");
        }
    }

    public void OnHoldInput(InputAction.CallbackContext context) // HOLD
    {
        if (!player1.waiting)
            holdInput = context.performed;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(m_trfGroundCheck.position,
            m_trfGroundCheck.lossyScale, 0, m_groundLayer) != null;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapBox(m_trfWallCheck.position,
            m_trfWallCheck.lossyScale, 0, m_groundLayer) != null;
    }
}

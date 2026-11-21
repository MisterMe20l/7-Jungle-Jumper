using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Player player1;

    [SerializeField] Transform m_playerCheck;
    [SerializeField] LayerMask m_playerLayer;
    GameObject player;

    [SerializeField] Animator m_animator;

    private bool reachedBefore = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (IsReached() && !reachedBefore)
        {
            reachedBefore = true;
            m_animator.Play("Active");
            player1.currentCheckpoint = gameObject;
        }
    }

    private bool IsReached()
    {
        return Physics2D.OverlapBox(m_playerCheck.position,
            m_playerCheck.lossyScale, 0, m_playerLayer) != null;
    }
}

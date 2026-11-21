using UnityEngine;
using UnityEngine.InputSystem;

public class ExitDoor : MonoBehaviour, IWinGame
{
    GameInput gameInput;

    [SerializeField] Enemy enemy1;

    [SerializeField] Animator m_anmDoor;
    [SerializeField] Animator m_anmIcon;

    [SerializeField] Transform m_door; 
    [SerializeField] LayerMask m_playerLayer;

    bool active = false;
    bool showingIcon = false;
    InputAction interact;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        interact = gameInput.Player.Interact;
        interact.Enable();
        interact.performed += OnInteractInput;
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void Update()
    {
        m_anmIcon.SetBool("Showing", showingIcon);
        if (active)
        {
            Showing();
        }
    }

    private void Showing()
    {
        var exitable = OnExitRange();
        if (!exitable) // Out of range
        {
            showingIcon = false;
        }
        else if (exitable && !showingIcon) // On range
        {
            showingIcon = true;
        }
    }

    public void WinGame()
    {
        m_anmDoor.Play("Active");
        active = true;
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (showingIcon)
        {
            SceneLoader.instance.LoadSceneAsync(true, "Main Menu");
        }
    }

    private bool OnExitRange()
    {
        return Physics2D.OverlapBox(m_door.position,
            m_door.lossyScale, 0, m_playerLayer) != null;
    }
}

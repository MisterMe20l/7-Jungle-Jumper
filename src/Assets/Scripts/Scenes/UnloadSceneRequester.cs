using UnityEngine;
using UnityEngine.InputSystem;

public class UnloadSceneRequester : MonoBehaviour
{
    [SerializeField] string m_sceneName;
    GameInput gameInput;
    InputAction unload;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        unload = gameInput.Menu.Return;
        unload.Enable();
        unload.performed += OnReturnInput;
    }

    public void RequestSceneUnload()
    {
        if (m_sceneName == "Pause Menu") Time.timeScale = 1.0f;
        SceneLoader.instance.UnloadSceneAsync(m_sceneName);
    }

    private void OnReturnInput(InputAction.CallbackContext context)
    {
        RequestSceneUnload();
    }
}

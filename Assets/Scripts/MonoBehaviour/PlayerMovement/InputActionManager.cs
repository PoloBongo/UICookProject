using UnityEngine;

public class InputActionManager : MonoBehaviour
{
    public static InputActionManager Instance;
    private PlayerInputAction playerInputAction;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }
}

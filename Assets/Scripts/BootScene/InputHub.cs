using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    GamePlay,
    UI
}

public class InputHub : MonoBehaviour
{
    private PlayerInput input;
    public InputAction inputAction => input.actions["Global"];
    
    void Awake()
    {
        
    }
}

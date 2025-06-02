using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager {
   
    private PlayerControls playerControls;

    public float Movement_W_S => playerControls.Jogador.Movement_W_S.ReadValue<float>(); 
    public float Movement_A_D => playerControls.Jogador.Movement_A_D.ReadValue<float>();
    

    public InputManager(){
        playerControls = new PlayerControls();
        playerControls.Jogador.Enable();
    }
}

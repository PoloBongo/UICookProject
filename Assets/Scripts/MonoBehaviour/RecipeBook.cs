using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecipeBook : MonoBehaviour
{
    private InputActionManager inputActionManager;
    private PlayerInputAction playerInputAction;

    private bool isOpen;

    private void Start()
    {
        inputActionManager = InputActionManager.Instance;
        playerInputAction = inputActionManager.GetPlayerInputAction();
        playerInputAction.Player.RecipeBook.performed += OpenRecipeBook;
        
        isOpen = false;
    }

    private void OpenRecipeBook(InputAction.CallbackContext context)
    {
        isOpen = !isOpen;
    }
}

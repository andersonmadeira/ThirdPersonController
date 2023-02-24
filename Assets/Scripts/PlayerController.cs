using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.HandleAllInput();
    }

    void FixedUpdate()
    {
        playerMovement.HandleAllMovement();    
    }
}

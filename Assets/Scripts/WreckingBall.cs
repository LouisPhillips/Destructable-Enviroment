using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WreckingBall : MonoBehaviour
{
    private InputController controller;

    private bool left;
    private bool right;

    // Start is called before the first frame update
    void Awake()
    {
        controller = new InputController();

        controller.Player.TurnLeft.performed += context => left = true;
        controller.Player.TurnLeft.canceled += context => left = false;

        controller.Player.TurnRIght.performed += context => right = true;
        controller.Player.TurnRIght.canceled += context => right = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (left)
        {
            transform.RotateAround(transform.position, -Vector3.up, 10 * Time.deltaTime);
        }

        if (right)
        {
            transform.RotateAround(transform.position, Vector3.up, 10 * Time.deltaTime);
        }

    }

    void OnEnable()
    {
        controller.Enable();
    }
    void OnDisable()
    {
        controller.Disable();
    }
}

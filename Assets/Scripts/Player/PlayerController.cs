using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed = 10;
    [SerializeField]
    private CharacterController _controller;
    
    private void Update()
    {
        PollInput();
    }
    private void PollInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }

        Move(direction);
    }
    private void Move(Vector2 direction)
    {
        _controller.Move(direction * _movementSpeed * Time.deltaTime);
    }
    private void OnValidate()
    {
        _controller = GetComponent<CharacterController>();
    }
}

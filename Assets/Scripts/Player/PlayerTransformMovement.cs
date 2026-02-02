using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformMovement : MonoBehaviour
{
    public PlayerInputHandler inputHandler;

    public Transform targetTransform;

    public float moveSpeed = 5f;

    public float turnSpeed = 15f;

    void Update()
    {
        Vector2 move = inputHandler.moveInput * moveSpeed * Time.deltaTime;
        targetTransform.position += new Vector3(move.x, 0f, move.y);
    }
}
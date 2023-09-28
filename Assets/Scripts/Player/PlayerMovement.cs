using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Pathfinding;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 6f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CinemachineVirtualCamera virtualCamera;

    private float horizontalInput;
    private float verticalInput;
    private float lastHorizontalInput;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        ProceduralGridMover gridMover = FindObjectOfType<ProceduralGridMover>();

        if (virtualCamera != null)
        {
            virtualCamera.Follow = gameObject.transform;
        }

        if (gridMover != null)
        {
            gridMover.target = gameObject.transform;
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(horizontalInput, verticalInput, 0);

        animator.SetFloat("isMoving", (Mathf.Abs(horizontalInput)) + (Mathf.Abs(verticalInput)));

        movementVector = movementSpeed * Time.fixedDeltaTime * movementVector.normalized;
        rigidBody.MovePosition(transform.position + movementVector);

        if (horizontalInput > 0 || horizontalInput < 0)
        {
            lastHorizontalInput = horizontalInput;
        }

        if (horizontalInput == -1 || lastHorizontalInput == -1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


    }
}
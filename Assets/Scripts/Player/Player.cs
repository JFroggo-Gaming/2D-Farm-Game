using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    // Movement parameters for the player
    private float xInput; // Input value for horizontal movement
    private float yInput; // Input value for vertical movement
    private bool isWalking; // Flag indicating if the player is walking
    private bool isRunning; // Flag indicating if the player is running
    private bool isIdle; // Flag indicating if the player is idle (not moving)
    private bool isCarrying; // Flag indicating if the player is carrying something
    private ToolEffect toolEffect = ToolEffect.none; // Effect of the tool the player is using
    private bool isUsingToolRight; // Flag indicating if the player is using a tool to the right
    private bool isUsingToolLeft; // Flag indicating if the player is using a tool to the left
    private bool isUsingToolUp; // Flag indicating if the player is using a tool upward
    private bool isUsingToolDown; // Flag indicating if the player is using a tool downward
    private bool isLiftingToolRight; // Flag indicating if the player is lifting a tool to the right
    private bool isLiftingToolLeft; // Flag indicating if the player is lifting a tool to the left
    private bool isLiftingToolUp; // Flag indicating if the player is lifting a tool upward
    private bool isLiftingToolDown; // Flag indicating if the player is lifting a tool downward
    private bool isPickingRight; // Flag indicating if the player is picking to the right
    private bool isPickingLeft; // Flag indicating if the player is picking to the left
    private bool isPickingUp; // Flag indicating if the player is picking upward
    private bool isPickingDown; // Flag indicating if the player is picking downward
    private bool isSwingingToolRight; // Flag indicating if the player is swinging a tool to the right
    private bool isSwingingToolLeft; // Flag indicating if the player is swinging a tool to the left
    private bool isSwingingToolUp; // Flag indicating if the player is swinging a tool upward
    private bool isSwingingToolDown; // Flag indicating if the player is swinging a tool downward
    private bool idleUp; // Flag indicating if the player is idle facing up
    private bool idleDown; // Flag indicating if the player is idle facing down
    private bool idleLeft; // Flag indicating if the player is idle facing left
    private bool idleRight; // Flag indicating if the player is idle facing right

    private Camera mainCamera;
    private Rigidbody2D rigidBody2D; // Reference to the RigidBody2D component attached to the player (Typo: should be "RigidBody2D" instead of "RigidBody2D")
#pragma warning disable 414
    private Direction playerDirection; // Current direction the player is facing
#pragma warning restore 414
    private float movementSpeed; // Speed of the player's movement

    private bool _playerInputIsDisabled = false; // Flag indicating if the player input is disabled

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>(); // Get the rigidBbody2D component

        mainCamera = Camera.main;
    }

    private void Update()
    {
        #region Player Input
        if(!PlayerInputIsDisabled)
        {

        ResetAnimationTriggers(); // Reset all animation triggers before processing player input

        PlayerMovementInput(); // Handle player movement input

        PlayerWalkInput(); // if you hold the "shift" key down, the player will walk

        // Send event to any listener for player movement speed

        //By calling EventHandler.CallMovementEvent() with these parameters, the script is notifying all subscribers to the MovementEvent
        // about the player's movement state. The subscribers, in this case, would be other scripts that have subscribed to the MovementEvent
        // and have methods to handle this specific event.
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
    // Calculate the movement vector based on player input and movement speed
    Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

    // Move the player's RigidBody2D to the new position based on the calculated movement vector
    rigidBody2D.MovePosition(rigidBody2D.position + move);
    }


    private void ResetAnimationTriggers()
    {
        // Reset all animation triggers to their default values
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolDown = false;
        isUsingToolUp = false;
        isUsingToolLeft = false;
        isUsingToolRight = false;
        isLiftingToolDown = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolRight = false;
        isSwingingToolDown = false;
        isSwingingToolLeft = false;
        isSwingingToolRight = false;
        isSwingingToolUp = false;
        toolEffect = ToolEffect.none; // Reset the tool effect to none
    }

    private void PlayerMovementInput()
    {
        yInput = Input.GetAxisRaw("Vertical"); // Get vertical input (-1 to 1) from Unity Input system
        xInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (-1 to 1) from Unity Input system

        if (yInput != 0 && xInput != 0)
        {
            // If the player is moving diagonally, slow down the movement by a factor to avoid full speed when moving in two directions at once
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if (xInput != 0 || yInput != 0)
        {
            // If the player is moving (either horizontally or vertically), set the appropriate flags and update movementSpeed
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningspeed;

            // Capture the player's direction for saving the game
            if (xInput < 0)
            {
                playerDirection = Direction.left;
            }
            else if (xInput > 0)
            {
                playerDirection = Direction.right;
            }
            else if (yInput < 0)
            {
                playerDirection = Direction.down;
            }
            else
            {
                playerDirection = Direction.up;
            }
        }
        else if (xInput == 0 && yInput == 0)
        {
            // If the player is not moving, set the appropriate flags and stop the player's movement
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }

    public void PlayerWalkInput()
{
    // Check if the player is holding the left shift or right shift key
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
        // If shift key is held, set the player to walking mode and adjust movement speed accordingly
        isRunning = false;
        isWalking = true;
        isIdle = false;
        movementSpeed = Settings.walkingspeed; // Set movement speed to the walking speed defined in the "Settings" class
    }
    else
    {
        // If shift key is not held, set the player to running mode and adjust movement speed accordingly
        isRunning = true;
        isWalking = false;
        isIdle = false;
        movementSpeed = Settings.runningspeed; // Set movement speed to the running speed defined in the "Settings" class
    }
}
private void ResetMovement()
{
    // Reset movement
    xInput = 0f;
    yInput = 0f;
    isRunning = false;
    isWalking = false;
    isIdle = false;
}
public void DisablePlayerInputAndResetMovement()
{
    DisablePlayerInput();
    ResetMovement();


    EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false);
}
public void DisablePlayerInput()
{
    PlayerInputIsDisabled = true;
}
public void EnablePlayerInput()
{
    PlayerInputIsDisabled = false;
}
public Vector3 GetPlayerViewportPosition()
{
    // Vector3 viewport position for player ((0,0) viewport bottom left, (1,1) viewport top right
    return mainCamera.WorldToViewportPoint(transform.position);
}

}

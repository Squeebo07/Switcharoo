using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PhysicsController))]
public class CharacterController : MonoBehaviour
{

    public float dashDistance;
    public float dashTime;
    float dashVelocity;
    bool dashingThroughTheSnow;
    bool canDash;

    float edgeBufferCount;
    public float edgeBufferLimit;


    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float jumpVelocity;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float moveSpeed = 6;
    float gravity = -20;
    Vector3 velocity;

    float velocityXSmoothing;
    public float accelerationTime;

    public float bufferLimit;
    float bufferCount;

    PhysicsController physicsController;
    private void Start()
    {
        physicsController = GetComponent<PhysicsController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (physicsController.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);

        if (physicsController.collisions.above || physicsController.collisions.below)
        {
            edgeBufferCount = 0;
            velocity.y = 0;
            canDash = true;
        }

        if (!physicsController.collisions.below)
        {
            edgeBufferCount += 1;
        }

        if (Input.GetButtonDown("Dash") && !dashingThroughTheSnow && canDash)
        {
            StartCoroutine(Dash());
        }

        bool wallSliding = false;
        if ((physicsController.collisions.left || physicsController.collisions.right) && !physicsController.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
                
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (physicsController.collisions.below && bufferCount > 0)
        {
            velocity.y = jumpVelocity;
        }
        
        
        if (!physicsController.collisions.below)
        {
            bufferCount -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            bufferCount = bufferLimit;

            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            else if (edgeBufferLimit > edgeBufferCount)
            {
                velocity.y = jumpVelocity;
            }
        }

            velocity.y += gravity * Time.deltaTime;
        
        physicsController.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        dashingThroughTheSnow = true;
        float time = 0;
        dashVelocity = dashDistance / dashTime;
        while (time < dashTime)
        {
            time += Time.deltaTime;
            velocity.x = dashVelocity * physicsController.collisions.faceDir;
            yield return null;
        }
        dashingThroughTheSnow = false;
        canDash = false;
    }
}


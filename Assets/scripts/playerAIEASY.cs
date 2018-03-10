using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIAction
{
    AI_DO_NOTHING,
    STOPPED_CAN_JUMP,
    STOPPED_CANT_JUMP,
    OTHER_HAZARD_STOPPED,
    AI_MOVE_RIGHT,
    AI_MOVE_LEFT,
    AI_JUMP
}

abstract class AIObjective
{
    public AIObjective(GameObject playerObject, GameObject AIObject, AIEnvironmentManager environmentManager)
    {
        this.playerObject = playerObject;
        this.AIObject = AIObject;
        this.environmentManager = environmentManager;
    }

    public abstract AIAction getMovement();
    public abstract AIAction getJump();

    protected GameObject playerObject;
    protected GameObject AIObject;
    protected AIEnvironmentManager environmentManager;
}

abstract class AIEnvironmentManager
{
    public AIEnvironmentManager(GameObject AIObject) { this.AIObject = AIObject; }

    abstract public AIAction canPerform(AIAction action);
    abstract public bool canJump();

    protected GameObject AIObject;
}

class AIEnvironmentManagerAvoidHoles : AIEnvironmentManager
{
    public AIEnvironmentManagerAvoidHoles(GameObject AIObject)
        : base(AIObject)
    {

    }
    
    //determines if the gap is small enough to jump over
    public override bool canJump()
    {
        //determine if the player can jump and not die
        return false;
    }

    public override AIAction canPerform(AIAction action)
    {
        float maxDistance = 3;

        if (action == AIAction.AI_DO_NOTHING)
            return AIAction.AI_DO_NOTHING;
        else if (action == AIAction.AI_MOVE_LEFT)
        {
            RaycastHit2D hit = Physics2D.Raycast(AIObject.transform.position, new Vector2(-1, 0));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Hazard" || hit.collider.gameObject.tag == "JumpHazard")
                {
                    float distance = (AIObject.transform.position - hit.collider.transform.position).magnitude;
                    if (distance < maxDistance)
                    {
                        if (hit.collider.gameObject.tag == "JumpHazard")
                        {
                            return AIAction.STOPPED_CAN_JUMP;
                        }
                        else
                        {
                            return AIAction.STOPPED_CANT_JUMP;
                        }
                    }
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(AIObject.transform.position, new Vector2(1, 0));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Hazard" || hit.collider.gameObject.tag == "JumpHazard")
                {
                    float distance = (AIObject.transform.position - hit.collider.transform.position).magnitude;
                    if (distance < maxDistance)
                    {
                        if (hit.collider.gameObject.tag == "JumpHazard")
                        {
                            return AIAction.STOPPED_CAN_JUMP;
                        }
                        else
                        {
                            return AIAction.STOPPED_CANT_JUMP;
                        }
                    }
                }
            }
        }

        return action;
    }
}

class AIGetClose : AIObjective
{
    private float tooClose;

    public AIGetClose(GameObject playerObject, GameObject AIObject, AIEnvironmentManager environmentManager, float tooClose) 
        :base(playerObject, AIObject, environmentManager)
    {
        this.tooClose = tooClose;
    }

    public override AIAction getMovement()
    {
        AIAction ret = AIAction.AI_DO_NOTHING;

        if (Mathf.Abs(playerObject.transform.position.x - AIObject.transform.position.x) > tooClose)
        {
            if (playerObject.transform.position.x > AIObject.transform.position.x)
            {
                ret = AIAction.AI_MOVE_RIGHT;
            }
            else
            {
                ret = AIAction.AI_MOVE_LEFT;
            }
        }
        else
        {
            ret = AIAction.AI_DO_NOTHING;
        }

        return environmentManager.canPerform(ret);
    }

    public override AIAction getJump()
    {
        if (getMovement() == AIAction.STOPPED_CAN_JUMP)
        {
            return AIAction.AI_JUMP;
        }

        return AIAction.AI_DO_NOTHING;
    }
}

class AIRun : AIObjective
{
    private float tooClose;

    public AIRun(GameObject playerObject, GameObject AIObject, AIEnvironmentManager environmentManager, float tooClose)
        : base(playerObject, AIObject, environmentManager)
    {
        this.tooClose = tooClose;
    }

    public override AIAction getMovement()
    {
        AIAction ret = AIAction.AI_DO_NOTHING;

        if (Mathf.Abs(playerObject.transform.position.x - AIObject.transform.position.x) < tooClose)
        {
            if (playerObject.transform.position.x < AIObject.transform.position.x)
            {
                ret = AIAction.AI_MOVE_RIGHT;
            }
            else
            {
                ret = AIAction.AI_MOVE_LEFT;
            }
        }
        else
        {
            ret = AIAction.AI_DO_NOTHING;
        }

        return environmentManager.canPerform(ret);
    }

    public override AIAction getJump()
    {
        if (getMovement() == AIAction.STOPPED_CAN_JUMP)
        {
            return AIAction.AI_JUMP;
        }

        return AIAction.AI_DO_NOTHING;
    }
}

struct AIActions {
    public AIAction movementAction;
    public AIAction jumpAction;
}

public class playerAIEASY : MonoBehaviour
{
    private Rigidbody2D body;
    private bool jumping;

    [SerializeField] GameObject playerObject;
    [SerializeField] float tooCloseRadius;

    [SerializeField] float inAirMovementAcceleration;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float movementAcceleration;
    [SerializeField] float jumpVelocity;
    [SerializeField] float staticDecel;
    [SerializeField] float staticInAirDecel;

    Animator anim;
    float staticDecelCpy;
    float movementAccelerationCpy;

    private Vector3 spawnPoint;

    AIObjective charge;
    AIObjective run;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        staticDecelCpy = staticDecel;
        movementAccelerationCpy = movementAcceleration;
        spawnPoint = transform.position;

        AIEnvironmentManager environmentManager = new AIEnvironmentManagerAvoidHoles(this.gameObject);
        charge = new AIGetClose(playerObject, this.gameObject, environmentManager, tooCloseRadius);
        run = new AIRun(playerObject, this.gameObject, environmentManager, tooCloseRadius);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        AIActions actions;
        AIEnvironmentManager environmentManager = new AIEnvironmentManagerAvoidHoles(this.gameObject);
        AIObjective objective = charge;

        if(Mathf.Abs(playerObject.transform.position.x - transform.position.x) < tooCloseRadius)
            objective = run;

        actions.movementAction = objective.getMovement();
        actions.jumpAction = objective.getJump();

        if (jumping)
        {
            movementAcceleration = inAirMovementAcceleration;
            staticDecel = staticInAirDecel;
        }
        else
        {
            movementAcceleration = movementAccelerationCpy;
            staticDecel = staticDecelCpy;
        }

        if (actions.movementAction == AIAction.AI_MOVE_LEFT)
        {
            if ((body.velocity.x * -1) < maxMoveSpeed)
            {
                anim.SetBool("RunningLeft", true);
                anim.SetBool("RunningRight", false);
                body.velocity -= new Vector2(movementAcceleration * Time.deltaTime, 0);
            }
        }
        else if (actions.movementAction == AIAction.AI_MOVE_RIGHT)
        {
            if (body.velocity.x < maxMoveSpeed)
            {
                anim.SetBool("RunningRight", true);
                anim.SetBool("RunningLeft", false);
                body.velocity += new Vector2(movementAcceleration * Time.deltaTime, 0);
            }
        }
        else
        {
            anim.SetBool("RunningRight", false);
            anim.SetBool("RunningLeft", false);

            if (body.velocity.x > 0)
            {
                body.velocity -= new Vector2(staticDecel * Time.deltaTime, 0);
            }
            else
            {
                body.velocity += new Vector2(staticDecel * Time.deltaTime, 0);
            }
        }

        if (actions.jumpAction == AIAction.AI_JUMP)
        {
            if (!jumping)
                body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            jumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            jumping = false;
        }
    }
}

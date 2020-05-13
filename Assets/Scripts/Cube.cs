using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube : MonoBehaviour
{
    public bool isBottomOne; //is this cube a bottom one
    public bool isCloneChild;

    public enum Direction { forward, backward, left, right} //global direction
    public Direction direction;
    

    public LayerMask groundLayer;
    public LayerMask blockLayer;

    public LayerMask frontWallLayer;
    public LayerMask backWallLayer;
    public LayerMask leftWallLayer;
    public LayerMask rightWallLayer;

   

    public BlockController parentController;
    public Clone c_parentController;

    private readonly float rayDistance = 1f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.GetMask("Floor");
        blockLayer = LayerMask.GetMask("Block");
        frontWallLayer = LayerMask.GetMask("FrontWall");
        backWallLayer = LayerMask.GetMask("BackWall");
        leftWallLayer = LayerMask.GetMask("LeftWall");
        rightWallLayer = LayerMask.GetMask("RightWall");

        if(!isCloneChild)
        {
            parentController = transform.parent.transform.gameObject.GetComponent<BlockController>();
        }
        else c_parentController = transform.parent.transform.gameObject.GetComponent<Clone>();

        gameObject.layer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBottomOne)
        {
            if (IsTouchingGround())
            {
                if (isCloneChild)
                {
                    if(c_parentController.isVisible) c_parentController.isTouchingGround = true;
                }
                else
                {
                    if(parentController.isVisible) parentController.isTouchingGround = true;
                }
            }
        }
        
        
        HandleWallCollisions();
        HandleBlocksCollision();
    }
    public bool IsTouchingGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, .6f, groundLayer) ||
           Physics.Raycast(transform.position, Vector3.down, .6f, blockLayer))
        {
            return true;
        }
        return false;
    }
    #region Rays
    //Walls-------------
    Ray rayForward;
    Ray rayBack;
    Ray rayLeft;
    Ray rayRight;
    //Blocks-----------
    Ray forward1;
    Ray forward2;
    Ray forward3;
    Ray forward4;

    Ray back1;
    Ray back2;
    Ray back3;
    Ray back4;

    Ray left1;
    Ray left2;
    Ray left3;
    Ray left4;

    Ray right1;
    Ray right2;
    Ray right3;
    Ray right4;

    //Additional stopper booleans:
    bool st_forward;
    bool st_back;
    bool st_left;
    bool st_right;
    
    //Walls_end-------
    //Other Cubes raypoints-----
    [Header("Raypoints_blocks_collisions")]
    public Transform upper_topright;
    public Transform upper_topleft;
    public Transform upper_botright;
    public Transform upper_botleft;
    [Space]
    public Transform bot_topright;
    public Transform bot_topleft;
    public Transform bot_botright;
    public Transform bot_botleft;

    //Other cubes end-------




    private void HandleWallCollisions()
    {
        rayForward = new Ray(transform.position, Vector3.forward);
        rayBack = new Ray(transform.position, Vector3.back);
        rayLeft = new Ray(transform.position, Vector3.left);
        rayRight = new Ray(transform.position, Vector3.right);


        if (Physics.Raycast(rayForward, rayDistance, frontWallLayer))
        {
            if (!st_forward)
            {
                if (!isCloneChild) parentController.isTouchingFrontWall = true;
                else c_parentController.isTouchingFrontWall = true;

                st_forward = true;
            }
        }
        else
        {
            if(st_forward)
            {
                if (!isCloneChild) parentController.isTouchingFrontWall = false;
                else c_parentController.isTouchingFrontWall = false;
                st_forward = false;
            }
        }

        if (Physics.Raycast(rayBack, rayDistance, backWallLayer))
        {
            if (!st_back)
            {
                if (!isCloneChild) parentController.isTouchingBackWall = true;
                else c_parentController.isTouchingBackWall = true;
                st_back = true;
            }
        }
        else
        {
            if (st_back) {
                if (!isCloneChild) parentController.isTouchingBackWall = false;
                else c_parentController.isTouchingBackWall = false;
                st_back = false;
            }           
        }

        if (Physics.Raycast(rayLeft, rayDistance, leftWallLayer)) {
            if (!st_left) {
                if (!isCloneChild) parentController.isTouchingLeftWall = true;
                else c_parentController.isTouchingLeftWall = true;
                st_left = true;
            }
        }
        else {
            if (st_left) {
                if (!isCloneChild) parentController.isTouchingLeftWall = false;
                else c_parentController.isTouchingLeftWall = false;
                st_left = false;
            }      
        }

        if (Physics.Raycast(rayRight, rayDistance, rightWallLayer)) {
            if (!st_right) {
                if (!isCloneChild) parentController.isTouchingRightWall = true;
                else c_parentController.isTouchingRightWall = true;
                st_right = true;
            }
        }
        else {
            if (st_right) {
                if (!isCloneChild) parentController.isTouchingRightWall = false;
                else c_parentController.isTouchingRightWall = false;
                st_right = false;
            }        
        }
    }//collision with walls
    void HandleBlocksCollision()//prevents from going into other blocks in the game
    {
        float blockDistance = .2f;
         //Rays creation
        forward1 = new Ray(upper_topleft.position, transform.forward);
        forward2 = new Ray(upper_topright.position, transform.forward);
        forward3 = new Ray(bot_topleft.position, transform.forward);
        forward4 = new Ray(bot_topright.position, transform.forward);
        
        back1 = new Ray(upper_botleft.position, -transform.forward);
        back2 = new Ray(upper_botright.position, -transform.forward);
        back3 = new Ray(bot_botleft.position, -transform.forward);
        back4 = new Ray(bot_botright.position, -transform.forward);
        
        left1 = new Ray(upper_topleft.position, -transform.right);
        left2 = new Ray(upper_botleft.position, -transform.right);
        left3 = new Ray(bot_topleft.position, -transform.right);
        left4 = new Ray(bot_botleft.position, -transform.right);
        
        right1 = new Ray(upper_topright.position, transform.right);
        right2 = new Ray(upper_botright.position, transform.right);
        right3 = new Ray(bot_topright.position, transform.right);
        right4 = new Ray(bot_botright.position, transform.right);

        bool st_forward_block = false;
        bool st_back_block = false;
        bool st_left_block = false;
        bool st_right_block = false;

        //Cast Rays
        //FORWARD:
        Ray[] forwardRays = new Ray[] { forward1, forward2, forward3, forward4 };
        for (int i = 0; i < 4; i++)
        {           
            RaycastHit[] forwardHits = Physics.RaycastAll(forwardRays[i], blockDistance, blockLayer, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in forwardHits)
            {
                if (hit.collider != gameObject.GetComponent<BoxCollider>())//if we hit other block
                {
                    if (!st_forward_block)
                    {
                        FixBlockMovingWithDirectionFacing(true, "forward");
                        ControlRotation(true);
                        st_forward_block = true;
                    }

                }
                else //if we didnt hit anything
                {
                    if (st_forward_block)
                    {
                        FixBlockMovingWithDirectionFacing(false, "forward");
                        ControlRotation(false);
                        st_forward_block = false;
                    }
                }
            }
        }
        //BACK
        Ray[] backRays = new Ray[] { back1, back2, back3, back4 };
        for (int i = 0; i < 4; i++)
        {
            RaycastHit[] backHits = Physics.RaycastAll(backRays[i], blockDistance, blockLayer, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in backHits)
            {
                if (hit.collider != gameObject.GetComponent<BoxCollider>())//if we hit other block
                {
                    if (!st_back_block)
                    {
                        FixBlockMovingWithDirectionFacing(true, "backward");
                        ControlRotation(true);
                        st_back_block = true;
                    }

                }
                else //if we didnt hit anything
                {
                    if (st_back_block)
                    {
                        FixBlockMovingWithDirectionFacing(false, "backward");
                        ControlRotation(false);
                        st_back_block = false;
                    }
                }
            }
        }
        //LEFT
        Ray[] leftRays = new Ray[] { left1, left2, left3, left4 };
        for (int i = 0; i < 4; i++)
        {
            RaycastHit[] leftHits = Physics.RaycastAll(leftRays[i], blockDistance, blockLayer, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in leftHits)
            {
                if (hit.collider != gameObject.GetComponent<BoxCollider>())//if we hit other block
                {
                    if (!st_left_block)
                    {
                        FixBlockMovingWithDirectionFacing(true, "left");
                        ControlRotation(true);
                        st_left_block = true;
                    }

                }
                else //if we didnt hit anything
                {
                    if (st_left_block)
                    {
                        FixBlockMovingWithDirectionFacing(false, "left");
                        ControlRotation(false);
                        st_left_block = false;
                    }
                }
            }
        }
        //RIGHT
        Ray[] rightRays = new Ray[] { right1, right2, right3, right4 };
        for (int i = 0; i < 4; i++)
        {
            RaycastHit[] rightHits = Physics.RaycastAll(rightRays[i], blockDistance, blockLayer, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in rightHits)
            {
                if (hit.collider != gameObject.GetComponent<BoxCollider>())//if we hit other block
                {
                    if (!st_right_block)
                    {
                        FixBlockMovingWithDirectionFacing(true, "right");
                        ControlRotation(true);
                        st_right_block = true;
                    }

                }
                else //if we didnt hit anything
                {
                    if (st_right_block)
                    {
                        FixBlockMovingWithDirectionFacing(false, "right");
                        ControlRotation(false);
                        st_right_block = false;
                    }
                }
            }
        }
    }

   
    #endregion
    //--------------------------------------------------------------

    void FixBlockMovingWithDirectionFacing(bool value, string ray_dir)
    {
        switch(direction)
        {
            case Direction.forward: //FORWARD
                switch(ray_dir)
                {
                    case "forward":
                        if(value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = true;
                            else c_parentController.isTouchingFrontWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = false;
                            else c_parentController.isTouchingFrontWall = false;
                        }
                        break;
                    case "backward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = true;
                            else c_parentController.isTouchingBackWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = false;
                            else c_parentController.isTouchingBackWall = false;
                        }
                        break;
                    case "left":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = true;
                            else c_parentController.isTouchingLeftWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = false;
                            else c_parentController.isTouchingLeftWall = false;
                        }
                        break;
                    case "right":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = true;
                            else c_parentController.isTouchingRightWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = false;
                            else c_parentController.isTouchingRightWall = false;
                        }
                        break;
                }
                break;
            case Direction.backward://BACKWARDS
                switch (ray_dir)
                {
                    case "forward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = true;
                            else c_parentController.isTouchingBackWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = false;
                            else c_parentController.isTouchingBackWall = false;
                        }
                        break;
                    case "backward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = true;
                            else c_parentController.isTouchingFrontWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = false;
                            else c_parentController.isTouchingFrontWall = false;
                        }
                        break;
                    case "left":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = true;
                            else c_parentController.isTouchingRightWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = false;
                            else c_parentController.isTouchingRightWall = false;
                        }
                        break;
                    case "right":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = true;
                            else c_parentController.isTouchingLeftWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = false;
                            else c_parentController.isTouchingLeftWall = false;
                        }
                        break;
                }
                break;
            case Direction.left://LEFT
                switch (ray_dir)
                {
                    case "forward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = true;
                            else c_parentController.isTouchingLeftWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = false;
                            else c_parentController.isTouchingLeftWall = false;
                        }
                        break;
                    case "backward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = true;
                            else c_parentController.isTouchingRightWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = false;
                            else c_parentController.isTouchingRightWall = false;
                        }
                        break;
                    case "left":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = true;
                            else c_parentController.isTouchingBackWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = false;
                            else c_parentController.isTouchingBackWall = false;
                        }
                        break;
                    case "right":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = true;
                            else c_parentController.isTouchingFrontWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = false;
                            else c_parentController.isTouchingFrontWall = false;
                        }
                        break;
                }
                break;
            case Direction.right://RIGHT
                switch (ray_dir)
                {
                    case "forward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = true;
                            else c_parentController.isTouchingRightWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingRightWall = false;
                            else c_parentController.isTouchingRightWall = false;
                        }
                        break;
                    case "backward":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = true;
                            else c_parentController.isTouchingLeftWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingLeftWall = false;
                            else c_parentController.isTouchingLeftWall = false;
                        }
                        break;
                    case "left":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = true;
                            else c_parentController.isTouchingFrontWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingFrontWall = false;
                            else c_parentController.isTouchingFrontWall = false;
                        }
                        break;
                    case "right":
                        if (value == true)
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = true;
                            else c_parentController.isTouchingBackWall = true;
                        }
                        else
                        {
                            if (!isCloneChild) parentController.isTouchingBackWall = false;
                            else c_parentController.isTouchingBackWall = false;
                        }
                        break;
                }
                break;
        }       


    }
    void ControlRotation(bool block)
    {
        if(block)
        {
            if(isCloneChild)
            {
                if(c_parentController.parent != null)
                {
                    c_parentController.parent.GetComponent<BlockController>().canRotate = false;
                }
            }
            else
            {
                parentController.GetComponent<BlockController>().canRotate = false;
            }
        }
        else
        {
            if (isCloneChild)
            {
                c_parentController.parent.GetComponent<BlockController>().canRotate = true;
            }
            else
            {
                parentController.GetComponent<BlockController>().canRotate = true;
            }
        }
    }

    //Gizmos help rays:
    

    private void OnDrawGizmosSelected()
    {
        Ray groundRay1 = new Ray(transform.position,Vector3.down);
        //Side walls
        Gizmos.color = Color.red;

        //Gizmos.DrawRay(rayForward);
        //Gizmos.DrawRay(rayBack);
        //Gizmos.DrawRay(rayLeft);
        //Gizmos.DrawRay(rayRight);

        ////Ground
        //Gizmos.DrawRay(groundRay1);

        Gizmos.DrawRay(forward3);
        Gizmos.DrawRay(forward1);
        Gizmos.DrawRay(forward2);
        Gizmos.DrawRay(forward4);
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    public bool isVisible;
    public BlockController parent;
    public int fallSpeed = 2;
    [Header("Check wall touch:")]
    public bool isTouchingGround;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    public bool isTouchingBackWall;
    public bool isTouchingFrontWall;

    private Spawner spawner;
    private bool isAccelerated;
    [Space]
    private float moveAmount = 1.1f;
    private bool canTouchGround;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        isActive = true;
        StartCoroutine(LooseControl());       
    }

    // Update is called once per frame
    void Update()
    {        
        if(isActive) HandleMovement();
        if(isActive) transform.Translate(Vector3.down * fallSpeed * Time.deltaTime); //if active - fall down constantly
    }
    private void HandleMovement()//Movement input:
    {
        if (!isTouchingFrontWall)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.position += new Vector3(0, 0, moveAmount);
            }
        }

        if (!isTouchingBackWall)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.position -= new Vector3(0, 0, moveAmount);
            }
        }

        if (!isTouchingRightWall)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(moveAmount, 0, 0);
            }
        }

        if (!isTouchingLeftWall)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position -= new Vector3(moveAmount, 0, 0);
            }

        }

        //---SPACEBAR SPEED UP FALLING SPEED---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpeedUp();
            parent.SpeedUp();
        }
        //--------
    }

    [HideInInspector] public bool isActive;
    private IEnumerator LooseControl()// After hitting ground stop being active and stop moving
    {        
        yield return new WaitUntil(() => isTouchingGround == true);
        MasterController.Instance.PlaySound(MasterController.Instance.hitSound);
        parent.isActive = false;
        if (isVisible) DestroyOtherClones();

        isActive = false;       

        yield return new WaitForSeconds(.5f);

        ChangeLayerOfCubesToBlockLayer();

        spawner.SpawnNewBlock();

        TurnIntoPieces();

        yield return null;
    }
    public void Show()
    {
        canTouchGround = true;
        isVisible = true;
        foreach(Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public void Hide()
    {
        canTouchGround = false;
        isVisible = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }

    }
    
    public void SpeedUp()
    {
        if(!isAccelerated)
        {
            fallSpeed *= 3;
            isAccelerated = true;
        }   
    }
    void DestroyOtherClones()
    {
        foreach(GameObject clone in parent.GetComponent<BlockController>().myClones)
        {
            if(clone == gameObject)
            {
                continue;
            }
            else
            {
                Destroy(clone);
            }           
        }
        Destroy(parent.gameObject);
    }
    void ChangeLayerOfCubesToBlockLayer()
    {
        foreach (Transform child in transform)
        {
            child.transform.gameObject.layer = 8;
        }
    }
    private void TurnIntoPieces() // Stop it being one object and let the pieces fall down
    {
        enabled = false;
    }

}

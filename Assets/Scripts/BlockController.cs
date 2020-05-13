using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: niewidzalna wesja siada na klocku i zatrzymuje wszystko
public class BlockController : MonoBehaviour
{   
    public bool isActive;
    [Header("If not clone:")]
    public GameObject[] myClones;
    private int amountOfClones;
    

    private readonly float moveAmount = 1.1f;
    public int fallSpeed = 2;

    [Header("Check wall touch:")]
    public bool isTouchingGround;
    public bool isTouchingLeftWall;
    public bool isTouchingRightWall;
    public bool isTouchingBackWall;
    public bool isTouchingFrontWall;
    [Space]
    private Spawner spawner;
    public bool isVisible;
    public bool canRotate;
    private bool isAccelerated;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LooseControl());
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        amountOfClones = myClones.Length;
        isVisible = true;
        canRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive) HandleMovement();       
    }
    private void FixedUpdate()
    {
        if(isActive)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime); //if active - fall down constantly
        }  
    }
    private void HandleMovement()//Movement input:
    {     
        if(!isTouchingFrontWall)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.position += new Vector3(0, 0, moveAmount);
            }
        }
       
        if(!isTouchingBackWall)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.position -= new Vector3(0, 0, moveAmount);
            }
        }
       
        if(!isTouchingRightWall)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(moveAmount, 0, 0);
            }
        }
       
        if(!isTouchingLeftWall)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position -= new Vector3(moveAmount, 0, 0);
            }

        }

        //---ROTATIONS---
        if(amountOfClones >= 1 && canRotate)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PrevShape();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextShape();
            }
        }  
        //---SPACEBAR SPEED UP FALLING SPEED---
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpeedUp();

            foreach(GameObject clone in myClones)
            {
                clone.GetComponent<Clone>().SpeedUp();
            }
        }
        //-------------------------------------
    }
    private IEnumerator LooseControl()// After hitting ground stop being active and stop moving
    {
        yield return new WaitUntil(() => isTouchingGround == true);
        MasterController.Instance.PlaySound(MasterController.Instance.hitSound);
        isActive = false;

        if (isVisible) DestroyMyClones();

        

        yield return new WaitForSeconds(.5f);

        ChangeLayerOfCubesToBlockLayer();
        spawner.SpawnNewBlock();

        TurnIntoPieces();
        yield return null;
    }
    private void TurnIntoPieces() // Stop it being one object and let the pieces fall down
    {
        enabled = false;   
    }
    public void SpeedUp()
    {
        if (!isAccelerated)
        {
            fallSpeed *= 3;
            isAccelerated = true;
        }
    }
    #region CloneRegion
    //Has 1 clone---------------------------------------------------
    bool isOneCloneActive;
    
    /// <summary>
    /// Index: 0-3
    /// </summary>
    void GoToClone(int cloneIndex)
    {
        if(cloneIndex == -1)
        {
            GoToMainBlock();
            return;
        }

        HideMe();
        myClones[cloneIndex].GetComponent<Clone>().Show();
        isOneCloneActive = true;
        isVisible = false;
    }   
    void GoToMainBlock()
    {
        ShowMe();
        myClones[0].GetComponent<Clone>().Hide();
        isOneCloneActive = false;
        isVisible = true;
    }
   

    //-------------------------------------------------------------
    //Has 3 Clones--------------------------------------------------
    
    
    void NextShape(bool clockwise)
    {
        if(clockwise)
        {
            currentShape++;
            if (currentShape == 4) currentShape = 0;
        }
        else
        {
            currentShape--;
            if (currentShape == -1) currentShape = 3;
        }
    }





    int currentShape = -1; // -1 is the main block. 0, 1 and 2 are clones.
    void NextShape()//E
    {
        if(amountOfClones == 3)
        {
            if (currentShape == -1)
            {
                HideMe();
            }
            else
            {
                myClones[currentShape].GetComponent<Clone>().Hide();
            }

            if (currentShape == 2)
            {
                currentShape = -2;
            }
        }
        else if(amountOfClones == 1)
        {
            if (currentShape == -1)
            {
                HideMe();
            }
            else
            {
                myClones[currentShape].GetComponent<Clone>().Hide();
            }

            if(currentShape == 0)
            {
                currentShape = -2;
            }
        }
       
        currentShape++;
        GoToClone(currentShape);
    }
    void PrevShape()//Q
    {
        if (amountOfClones == 3)
        {
            if (currentShape == -1)
            {
                HideMe();
            }
            else
            {
                myClones[currentShape].GetComponent<Clone>().Hide();
            }

            
        }
        else if (amountOfClones == 1)
        {
            if (currentShape == -1)
            {
                HideMe();
            }
            else
            {
                myClones[currentShape].GetComponent<Clone>().Hide();
            }           
        }

        currentShape--;
        if (currentShape == -2 && amountOfClones == 1) currentShape = 0;
        if (currentShape == -2 && amountOfClones == 3) currentShape = 2;

        GoToClone(currentShape);
    }





    //----------------------------------------------------------------
    void DestroyMyClones()
    {
        foreach(GameObject clone in myClones)
        {
            Destroy(clone);
        }
    }

    void ChangeLayerOfCubesToBlockLayer()
    {
        foreach(Transform child in transform)
        {
            child.transform.gameObject.layer = 8;
        }
    }


  
    
    void HideMe()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    void ShowMe()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().enabled = true;
        }
    }
  
    #endregion


}

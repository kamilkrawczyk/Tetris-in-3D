using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField]
    Transform[] RayStartPoints;

    public LayerMask blockLayer;

    private void Update()
    {
        CastRays();
    }
    void CastRays()
    {
        foreach(Transform rayPoint in RayStartPoints)
        {
            Ray ray = new Ray
            {
                origin = rayPoint.position,
                direction = rayPoint.transform.forward
            };

            RaycastHit[] hits = Physics.RaycastAll(ray, 10, blockLayer);
            
            //Count hits, if 5 do stuff:
            if(hits.Length == 5)
            {
                MasterController.Instance.PlaySound(MasterController.Instance.scoreSound);
                Score.AddScore(5);
                RemoveBlocks(hits);
            }
        }
    }
    void RemoveBlocks(RaycastHit[] hits)
    {
        foreach(RaycastHit block_hit in hits)
        {
            if(isBlockAbove(block_hit.transform.position))
            {
                MoveDown(blockAboveInfo.transform.gameObject);
            }
            Destroy(block_hit.transform.gameObject);
        }
    }

    RaycastHit blockAboveInfo;
    bool isBlockAbove(Vector3 pos)
    {
        if (Physics.Raycast(pos, Vector3.up,out blockAboveInfo,2 , blockLayer))
        {
            return true;
        }
        else return false;
    }
    /// <summary>
    /// Moves blocks above one hit down the length of one cube
    /// </summary>
    void MoveDown(GameObject cube)
    {
        if (isBlockAbove(cube.transform.position))
        {
            MoveDown(blockAboveInfo.transform.gameObject);
        }

        cube.transform.Translate(Vector3.down);
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        foreach(Transform raypoint in RayStartPoints)
        {
            Gizmos.DrawRay(raypoint.position, raypoint.transform.forward * 10);
        }
    }
}

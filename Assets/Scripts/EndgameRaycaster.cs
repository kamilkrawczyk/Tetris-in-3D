using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameRaycaster : MonoBehaviour
{
    [SerializeField]
    Transform[] rayPoints;

    [SerializeField]
    LayerMask blocksLayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForEndGame());
    }

    IEnumerator WaitForEndGame()
    {
        yield return new WaitUntil(() => AreBlocksUpToTop() == true);

        MasterController.Instance.EndGame();
        yield break;
    }
    bool AreBlocksUpToTop()
    {
        foreach(Transform point in rayPoints)
        {
            if(Physics.Raycast(point.position, transform.right, 7, blocksLayer))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        foreach(Transform tr in rayPoints)
        {
            Gizmos.DrawLine(tr.position, tr.position + Vector3.right * 7);
        }
    }
}

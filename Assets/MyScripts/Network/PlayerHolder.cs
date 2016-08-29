using UnityEngine;
using System.Collections.Generic;

public class PlayerHolder : MonoBehaviour
{

    public List<GameObject> networkPlayers = new List<GameObject>();
    public BoxCollider bc;

    public void newSpawnLoc(GameObject player)
    {
        Bounds bounds = bc.bounds;

        Vector3 center = bounds.center;
        float x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
        float z = Random.Range(center.z - bounds.extents.z, center.z + bounds.extents.z);

        Vector3 spawnLoc = new Vector3(x, bounds.center.y, z);

        player.transform.position = spawnLoc;
    }
}

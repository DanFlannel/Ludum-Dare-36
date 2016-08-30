using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerHolder : MonoBehaviour
{
    public float maxKills;
    public float newRoundDelay;
    public BoxCollider bc;
    public List<GameObject> networkPlayers = new List<GameObject>();
    public List<playerControls_Networked> networkPlayerControls = new List<playerControls_Networked>();
    private bool isRoundEnd;
    private bool isCoroutine;

    void Start()
    {
        isCoroutine = false;
        isRoundEnd = false;
    }

    public void sortList()
    {

        if (isCoroutine)
        {
            return;
        }

        for(int i = 0; i < networkPlayerControls.Count - 1; i++)
        {
            if(networkPlayerControls[i].kills < networkPlayerControls[i + 1].kills)
            {
                playerControls_Networked tmp = networkPlayerControls[i + 1];
                networkPlayerControls[i + 1] = networkPlayerControls[i];
                networkPlayerControls[i] = tmp;
            }

            if(networkPlayerControls[i].kills >= maxKills &&  !isRoundEnd)
            {
                Debug.LogWarning("Round Ended!");
                isRoundEnd = true;
            }

        }
        if (isRoundEnd)
        {
            for (int i = 0; i < networkPlayerControls.Count; i++)
            {
                GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<MultiPlayerGUI>().addPlayer(i, networkPlayerControls[i].pName, networkPlayerControls[i].kills);
            }
            StartCoroutine(newRound());
        }
    }

    public void newSpawnLoc(GameObject player)
    {
        Bounds bounds = bc.bounds;

        Vector3 center = bounds.center;
        float x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
        float z = Random.Range(center.z - bounds.extents.z, center.z + bounds.extents.z);

        Vector3 spawnLoc = new Vector3(x, bounds.center.y, z);

        RaycastHit hit;
        if(!Physics.Linecast(spawnLoc, new Vector3(spawnLoc.x, spawnLoc.y - 100, spawnLoc.z), out hit)){
            newSpawnLoc(player);
        }

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        player.transform.position = spawnLoc;
    }

    IEnumerator newRound()
    {
        Debug.Log("Starting CoRoutine");
        isCoroutine = true;
        isRoundEnd = false;
        GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<MultiPlayerGUI>().endRound();

        yield return new WaitForSeconds(newRoundDelay);

        for(int i = 0; i < networkPlayerControls.Count; i++)
        {
            if (!networkPlayerControls[i].isDead)
            {
                networkPlayerControls[i].applyDamage(1);
            }
            networkPlayerControls[i].kills = 0;
        }
        GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<MultiPlayerGUI>().endRoundPanel.SetActive(false);
        isCoroutine = false;
    }
}

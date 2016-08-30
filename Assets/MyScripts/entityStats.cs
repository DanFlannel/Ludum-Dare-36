using UnityEngine;
using System.Collections;

public class entityStats : MonoBehaviour {

    public int RPM;
    public float force;
    public bool isDead;

    public float respawnTimer;

    public float curRespawnTimer;

    public void StatsInit()
    {
        isDead = false;
        respawnTimer = 3f;
        curRespawnTimer = respawnTimer;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerStats : NetworkBehaviour {

    public int RPM;
    public float force;
    public bool isDead;

    public float respawnTimer;

    public float curRespawnTimer;

    public bool isLocal;

    public void StatsInit()
    {
        isLocal = isLocalPlayer;

        isDead = false;
        respawnTimer = 3f;
        curRespawnTimer = respawnTimer;
    }
}

using UnityEngine;
using System.Collections;

public class EnemyAI : entityStats {

    public float range;

    private NavMeshAgent agent;
    private GameMaster gm;
    private GameObject target;
    private PlayerHolder players;

    private AudioSource aSource;
    private BlowDartGun blowGun;


    void Start()
    {
        curRespawnTimer = respawnTimer;
        gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
        players = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>();
        agent = this.GetComponent<NavMeshAgent>();

        if(players != null)
        {
            if (!players.networkPlayers.Contains(this.gameObject))
            {
                players.networkPlayers.Add(this.gameObject);
            }
        }

        FindTarget();

        aSource = this.GetComponent<AudioSource>();
        blowGun = this.GetComponent<BlowDartGun>();
        blowGun.setRPM_force(RPM, force);


    }
	
	// Update is called once per frame
	void Update () {

        checkRespawn();

        if (target != null && !target.GetComponent<entityStats>().isDead)
        {
            if (!isDead)
            {
                agent.destination = target.transform.position;
                Rotate();
                checkRange();
            }
        }
        else
        {
            FindTarget();
        }
	}

    private void Rotate()
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) <= agent.stoppingDistance)
        {
            Vector3 relativePos = target.transform.position - this.transform.position;
            relativePos = new Vector3(relativePos.x, 0, relativePos.z);
            if (relativePos != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = rotation;
            }
        }
    }

    private void FindTarget() {

        if(players == null)
        {
            return;
        }

        if(players.networkPlayers.Count <= 1)
        {
            return;
        }

        //for randomness
        for(int i = 0; i < players.networkPlayers.Count; i++)
        {
            int rnd = Random.Range(0, players.networkPlayers.Count);
            if(players.networkPlayers[rnd] != this.gameObject)
            {
                if (!players.networkPlayers[rnd].GetComponent<entityStats>().isDead)
                {
                    target = players.networkPlayers[rnd];
                    return;
                }
            }
        }

        //just in case
        for(int i = 0; i < players.networkPlayers.Count; i++)
        {
            if (players.networkPlayers[i] != this.gameObject)
            {
                if (!players.networkPlayers[i].GetComponent<EnemyAI>().isDead)
                {
                    target = players.networkPlayers[i];
                    return;
                }
            }
        }

        target = null;
    }

    private void checkRange()
    {
        if(Vector3.Distance(this.transform.position, target.transform.position) < range)
        {
            blowGun.Shoot(aSource, target.transform.position);
        }
    }

    private void checkRespawn()
    {
        if (!isDead)
        {
            return;
        }
        curRespawnTimer -= Time.deltaTime;
        if(curRespawnTimer <= 0)
        {
            curRespawnTimer = respawnTimer;
            Respawn();
        }
    }

    public void Death()
    {
        GameObject death = Instantiate(gm.bloodPrefab, this.transform.position, Quaternion.identity) as GameObject;
        death.transform.parent = gm.transform;

        foreach(Transform child in death.transform)
        {
            if (child.GetComponent<Rigidbody>())
            {
                float rnd = Random.Range(0f, 1f);
                child.GetComponent<Rigidbody>().AddExplosionForce(gm.explosionForce * rnd, this.transform.position, gm.explosionRadius * rnd, gm.explosionUpwardMod * rnd, ForceMode.Impulse);
            }
        }

        if (!isDead)
        {
            players.newSpawnLoc(this.gameObject);
            models_boxColliders(false);
            isDead = true;
            agent.Stop();
        }
    }

    public void Respawn()
    {
        //change to find all childs and enable them, as well as enabling box colliders
        agent.Resume();
        models_boxColliders(true);
        isDead = false;
        FindTarget();
    }

    private void models_boxColliders(bool b)
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(b);
        }

        BoxCollider[] bcs = this.GetComponents<BoxCollider>();
        for (int i = 0; i < bcs.Length; i++)
        {
            bcs[i].enabled = b;
        }
    }
}

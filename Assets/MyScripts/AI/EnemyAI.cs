using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public AudioClip shotSound;

    public float range;
    public int RPM;
    public float force;

    private float sec_BetweenShots;
    private float shotTimer;

    public GameObject muzzelPos;
    public GameObject bullet;

    private NavMeshAgent agent;
    private GameMaster gm;
    private GameObject target;
    private PlayerHolder players;

    public bool canShoot;
    public bool isInRange;

    void Start()
    {
        canShoot = false;
        gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
        players = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>();
        agent = this.GetComponent<NavMeshAgent>();

        if(players != null)
        {
            players.networkPlayers.Add(this.gameObject);
        }

        if(gm.player != null) {
            target = gm.player;
        }
        else
        {
            FindTarget();
        }
        
        sec_BetweenShots = 60f / RPM;
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            agent.destination = target.transform.position;

            if (Vector3.Distance(target.transform.position, this.transform.position) <= agent.stoppingDistance)
            {
                Rotate();
            }

            shotTimer -= Time.deltaTime;
            checkRange();
            checkShoot();
        }
        else
        {
            FindTarget();
        }
	}

    private void Rotate()
    {
        Vector3 relativePos = target.transform.position - this.transform.position;
        relativePos = new Vector3(relativePos.x, 0, relativePos.z);
        if (relativePos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
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
                target = players.networkPlayers[rnd];
                return;
            }
        }

        //just in case
        for(int i = 0; i < players.networkPlayers.Count; i++)
        {
            if (players.networkPlayers[i] != this.gameObject)
            {
                target = players.networkPlayers[i];
                return;
            }
        }

        target = null;
    }

    private void Shoot(Vector3 targetPos)
    {
        if (!canShoot)
        {
            return;
        }
        AudioSource aSource = this.GetComponent<AudioSource>();
        aSource.PlayOneShot(shotSound);

        GameObject clone = Instantiate(bullet, muzzelPos.transform.position, Quaternion.identity) as GameObject;
        clone.name = "Lazer";
        clone.transform.parent = GameObject.Find(customStrings.ProjectileParent).transform;

        targetPos = new Vector3(targetPos.x, clone.transform.position.y, targetPos.z);

        Vector3 relativePos = targetPos - clone.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        clone.transform.rotation = rotation;

        Rigidbody rigid = clone.GetComponent<Rigidbody>();
        rigid.AddForce(clone.transform.forward * force);

        shotTimer = sec_BetweenShots;
        canShoot = false;
    }

    private void checkShoot()
    {
        if (shotTimer <= 0)
        {
            canShoot = true;
        }
        if (canShoot && isInRange)
        {
            Shoot(target.transform.position);
        }
    }

    private void checkRange()
    {
        if(Vector3.Distance(this.transform.position, target.transform.position) < range)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
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
        players.networkPlayers.Remove(this.gameObject);

        Destroy(this.gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour {

    public int RPM;
    private float sec_BetweenShots;
    private bool canShoot;


    public AudioClip mgSingleShot;
    public GameObject bullet;
    public GameObject muzzelPos;

    private GameMaster gm;
    private float shotTimer;

    void Start()
    {

        gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
        sec_BetweenShots = 60f/RPM;
        shotTimer = sec_BetweenShots;
    }

    void Update()
    {
        if (shotTimer <= 0)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
        if (!canShoot)
        {
            shotTimer -= Time.deltaTime;
        }
    }

    public void Shoot(AudioSource aSource, Vector3 targetPos)
    {
        if (!canShoot)
        {
            return;
        }

        aSource.PlayOneShot(mgSingleShot);

        GameObject clone = Instantiate(bullet, muzzelPos.transform.position, Quaternion.identity) as GameObject;
        clone.name = "bullet";
        clone.transform.parent = gm.projectileParent.transform;

        targetPos = new Vector3(targetPos.x, clone.transform.position.y, targetPos.z);

        Vector3 relativePos = targetPos - clone.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        clone.transform.rotation = rotation;

        Rigidbody rigid = clone.GetComponent<Rigidbody>();
        rigid.AddForce(clone.transform.forward * gm.projectileForce);

        shotTimer = sec_BetweenShots;
        canShoot = false;
    }
}

using UnityEngine;
using System.Collections;

public class BlowDartGun : MonoBehaviour {

    public int RPM;
    public float force;


    public AudioClip blowSound;
    public GameObject projectilePrefab;
    public GameObject muzzelPos;

    private float shotTimer;
    private float sec_BetweenShots;
    private bool canShoot;

    void Start()
    {
        sec_BetweenShots = 60f / RPM;
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

        aSource.PlayOneShot(blowSound);

        GameObject clone = Instantiate(projectilePrefab, muzzelPos.transform.position, Quaternion.identity) as GameObject;
        clone.name = "dart";

        Transform parent = GameObject.Find(customStrings.ProjectileParent).transform;

        clone.transform.parent = parent;

        targetPos = new Vector3(targetPos.x, clone.transform.position.y, targetPos.z);

        Vector3 relativePos = targetPos - clone.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        clone.transform.rotation = rotation;

        Rigidbody rigid = clone.GetComponent<Rigidbody>();
        rigid.AddForce(clone.transform.forward * force);

        Dart dart = clone.GetComponent<Dart>();
        dart.shooter = this.gameObject;

        shotTimer = sec_BetweenShots;
        canShoot = false;
    }
}

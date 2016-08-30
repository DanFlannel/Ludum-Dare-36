using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DartGun_network : NetworkBehaviour {

    private int RPM;
    private float force;

    public AudioClip blowSound;
    public GameObject projectilePrefab;
    public GameObject muzzelPos;

    private float shotTimer;
    private float sec_BetweenShots;
    private bool canShoot;

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

    [Command]
    public void CmdShoot(Vector3 targetPos)
    {
        if (!canShoot)
        {
            return;
        }
        GameObject clone = Instantiate(projectilePrefab, muzzelPos.transform.position, Quaternion.identity) as GameObject;

        Debug.Log(this.transform.GetComponent<Rigidbody>().velocity);
        Rigidbody rigid = this.GetComponent<Rigidbody>();
        rigid.AddForce(this.transform.forward * force);


        clone.name = "dart";

        Transform parent = GameObject.Find(customStrings.ProjectileParent).transform;

        clone.transform.parent = parent;

        targetPos = new Vector3(targetPos.x, clone.transform.position.y, targetPos.z);

        Vector3 relativePos = targetPos - clone.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        clone.transform.rotation = rotation;

        Debug.Log("Force: " + force);

        Dart dart = clone.GetComponent<Dart>();
        dart.shooter = this.gameObject;

        clone.AddComponent<AudioSource>().PlayOneShot(blowSound);

        NetworkServer.Spawn(clone);

        shotTimer = sec_BetweenShots;
        canShoot = false;
    }

    public void setRPM_force(int rpm, float f)
    {
        RPM = rpm;
        sec_BetweenShots = 60f / RPM;
        force = f;
    }
}

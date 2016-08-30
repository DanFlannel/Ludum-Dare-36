using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

    public GameObject shooter;
    public float force;
    //public GameMaster gm;

    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
        Debug.Log(this.transform.GetComponent<Rigidbody>().velocity);
        Rigidbody rigid = this.GetComponent<Rigidbody>();
        rigid.AddForce(this.transform.forward * force);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.tag);

        if(other.transform.tag == customTags.Player && other.gameObject == shooter)
        {
            return;
        }

        if (other.transform.tag == customTags.Projectile)
        {
            return;
        }

        if(other.transform.tag == customTags.Player)
        {
            if (other.GetComponent<playerControls_Networked>())
            {
                other.GetComponent<playerControls_Networked>().applyDamage(1);
            }
            if (other.GetComponent<playerControls>())
            {
                other.GetComponent<playerControls>().applyDamage(1);
            }
        }

        if (other.transform.tag == customTags.Enemy)
        {
            other.gameObject.GetComponent<EnemyAI>().Death();
        }
        else
        {

        }

        selfDestruct();
    }

    void selfDestruct()
    {
        Destroy(this.gameObject);
    }
}

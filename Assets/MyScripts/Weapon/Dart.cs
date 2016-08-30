using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

    public GameObject shooter;
    //public GameMaster gm;

    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();

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
            //networked players
            if (other.GetComponent<playerControls_Networked>())
            {
                other.GetComponent<playerControls_Networked>().applyDamage(1);
                shooter.GetComponent<playerControls_Networked>().kills++;

                Debug.Log(shooter.GetComponent<playerControls_Networked>().kills);
            }
            //single player
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

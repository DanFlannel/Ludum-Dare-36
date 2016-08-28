using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

    //public GameMaster gm;

    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<GameMaster>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.tag);
        if (other.transform.tag == customTags.Projectile || other.transform.tag == customTags.Player)
        {
            return;
        }
        if (other.transform.tag == customTags.Enemies)
        {

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

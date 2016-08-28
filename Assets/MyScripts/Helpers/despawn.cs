using UnityEngine;
using System.Collections;

public class despawn : MonoBehaviour {

    public float timer;

	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
	    if(timer <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}

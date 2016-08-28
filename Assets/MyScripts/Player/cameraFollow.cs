using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

    private Vector3 targetPosition;
    public GameObject player;
    public float dampening;

    public float distanceX;
    public float distanceY;
    public float distanceZ;
    
    public float rotation;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Follow();
	}

    private void Follow()
    {
        if(player == null)
        {
            return;
        }

        targetPosition = new Vector3((player.transform.position.x - distanceX), distanceY, (player.transform.position.z - distanceZ));
        this.gameObject.transform.position = Vector3.Lerp(this.transform.position, targetPosition, (1 - dampening));
    }
}

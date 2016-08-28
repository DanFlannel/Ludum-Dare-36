using UnityEngine;
using System.Collections.Generic;

public class CameraFollowAI : MonoBehaviour
{

    private Vector3 targetPosition;
    public GameObject target;
    public PlayerHolder players;
    public float dampening;

    public float distanceX;
    public float distanceY;
    public float distanceZ;

    public float rotation;


    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>();
        FindNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Follow();
        }
        else
        {
            FindNewTarget();
        }
    }

    private void Follow()
    {
        if (target == null)
        {
            return;
        }

        targetPosition = new Vector3((target.transform.position.x - distanceX), distanceY, (target.transform.position.z - distanceZ));
        this.gameObject.transform.position = Vector3.Lerp(this.transform.position, targetPosition, (1 - dampening));
    }

    private void FindNewTarget() {

        if (players.networkPlayers.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < players.networkPlayers.Count; i++)
        {
            if (players.networkPlayers[i] != this.gameObject)
            {
                target = players.networkPlayers[i];
                return;
            }
        }

    }

}

using UnityEngine;
using System.Collections;

public class MainMenuAIHandler : MonoBehaviour {

    public float spawnTimer;
    public float maxEnemies;
    public GameObject[] enemyPrefabs;

    private BoxCollider bc;
    private PlayerHolder players;
    private float timer;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>();
        bc = this.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnTimer;
            if (players.networkPlayers.Count <= maxEnemies)
            {
                spawn();
            }
        }
    }

    void spawn()
    {
        Bounds bounds = bc.bounds;

        Vector3 center = bounds.center;
        float x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
        float z = Random.Range(center.z - bounds.extents.z, center.z + bounds.extents.z);

        Vector3 spawnLoc = new Vector3(x, bounds.center.y, z);
        int rnd = Random.Range(0, enemyPrefabs.Length);

        GameObject alien = Instantiate(enemyPrefabs[rnd], spawnLoc, Quaternion.identity) as GameObject;
        alien.transform.localScale = new Vector3(1, 1, 1);
        alien.transform.parent = this.transform;
    }
}

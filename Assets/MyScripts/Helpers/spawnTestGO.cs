using UnityEngine;
using System.Collections;

public class spawnTestGO : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag(customTags.GameMaster).GetComponent<PlayerHolder>().networkPlayers.Add(this.gameObject);
	}
}

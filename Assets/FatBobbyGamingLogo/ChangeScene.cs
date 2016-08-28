using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public float timer;
    public int buildIndex;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if(timer<= 0)
        {
            SceneManager.LoadScene(buildIndex);
        }
	}
}

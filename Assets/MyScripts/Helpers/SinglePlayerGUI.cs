using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SinglePlayerGUI : MonoBehaviour {

    public GameObject tutorial;
    public GameObject gameOver;

	// Use this for initialization
	void Start () {
        if (!tutorial.activeInHierarchy)
        {
            tutorial.SetActive(true);
        }
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (tutorial.activeInHierarchy)
        {
            if (Input.anyKey)
            {
                Time.timeScale = 1;
                tutorial.SetActive(false);
            }
        }
	}

    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}

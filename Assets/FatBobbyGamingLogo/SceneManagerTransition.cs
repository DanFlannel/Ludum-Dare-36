using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Origional:
/// http://forum.unity3d.com/threads/scenemanager-loadsceneasync-and-getting-real-loading-times.403034/
/// </summary>

public class SceneManagerTransition : MonoBehaviour {

    public int buildIndex;
    public Text StatusText;
    public float timer;
    private float TimerA;
    private float TimerB;
    private float LoadTime;
    private AsyncOperation async;
    public bool dispLoadTime;

	// Use this for initialization
	void Start () {
        TimerA = Time.time;
        dispLoadTime = false;
        StartCoroutine(LoadScene());
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (async.progress == .9f && !dispLoadTime)
        {
            dispLoadTime = true;
            TimerB = Time.time;
            LoadTime = TimerB - TimerA;
            Debug.Log(string.Format("Load time: {0} {1} : {2}", TimerA, TimerB, LoadTime));
        }

        if (timer <= 0)
        {
            async.allowSceneActivation = true;
        }
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            var scaledPerc = 0.5f * async.progress / 0.9f;
            //StatusText.text = "Loading World : " + (100f * scaledPerc).ToString("F0");
            StatusText.text = "Gathering Carrots : " + (100f * scaledPerc).ToString("F0");
        }
        float perc = 0.5f;

        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
            perc = Mathf.Lerp(perc, 1f, 0.05f);
            //StatusText.text = "Initalizing World : " + (100f * perc).ToString("F0");
            if ((100f * perc) >= 99f)
            {
                StatusText.text = "Bunny Is Ready!";
            }
            else {
                StatusText.text = "Bunny Warming Up : " + (100f * perc).ToString("F0");
            }
        }

        //StatusText.text = "World Domination Complete";
    }

    
}

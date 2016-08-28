using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour {

	public void nextLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
}

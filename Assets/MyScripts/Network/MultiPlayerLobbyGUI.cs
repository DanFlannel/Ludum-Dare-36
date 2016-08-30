using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MultiPlayerLobbyGUI : MonoBehaviour {

    public int menuBuildIndex;
    public GameObject lobbyPanel;

    public void loadMenu()
    {
        Destroy(lobbyPanel);
        SceneManager.LoadScene(menuBuildIndex);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiPlayerGUI : MonoBehaviour {

    public GameObject endRoundPanel;
    public Text standings;
    private string results;
    
    void Start()
    {
        if (endRoundPanel.activeInHierarchy)
        {
            endRoundPanel.SetActive(false);
        }
    }

    public void addPlayer(int index, string name, int kills)
    {
        string result = string.Format("{0}. {1} {2}", index, name, kills);
        results += result + "\n";
    }

    public void endRound()
    {
        standings.text = results;
        endRoundPanel.SetActive(true);
        results = "";
    }

}

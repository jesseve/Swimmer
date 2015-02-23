using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : GameManager {

    public static LevelManager instance;

    private float startTime;
    public Text timerText;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

	// Use this for initialization
	void Start () {        
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator StartCounter() {
        for (int i = 3; i > 0; i--) {
            timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        timerText.text = "GO!";
        SetState(State.Running);
        yield return new WaitForSeconds(1f);
        timerText.text = "";
    }

    public void StartGame() {
        StartCoroutine("StartCounter");
    }

    public void GameOver() { }
    public void QuitGame() { }
    public void QuitApplication() { }
}

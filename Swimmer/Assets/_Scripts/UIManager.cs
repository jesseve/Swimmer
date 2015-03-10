using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject confirmCanvas;
    public GameObject menuCanvas;
    public GameObject runningCanvas;

    public FacebookHandler fb;

    public Text timerText;
    public Text distanceText;

    private bool timerRunning;

    public Transform player;

    private float timeElapsed;


	// Use this for initialization
	void Start () {
        Reset();

        LevelManager.instance.stateChanged += StateChange;
        LevelManager.instance.gameStarted += StartTimer;
        LevelManager.instance.gameEnded += GameOver;
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() == State.Running) RunningState();
	}

    public void StateChange() {
        switch (LevelManager.instance.GetState()) {
            case State.Running:
                SetPanel(runningCanvas);
                break;
            case State.Menu:
                SetPanel(menuCanvas);
                break;
        }
    }

    public void ShowConfrimBox() {
        SetPanel(confirmCanvas);
    }

    public void ExitConfrimBox(State current) {
        if (current == State.Menu)
            SetPanel(menuCanvas);
        else if (current == State.Running)
            SetPanel(runningCanvas);
    }

    public void Reset() {
        timeElapsed = 0;
        timerRunning = false;
    }

    public void StartTimer() {
        timerRunning = true;
        timeElapsed = 0;
    }

    public void StopTimer() {
        timerRunning = false;
    }

    public void GameOver()
    {
        StopTimer();
        if(FB.IsLoggedIn) fb.SetScore(timeElapsed);
    }

    private void SetPanel(GameObject go) {
        confirmCanvas.SetActive(false);        
        runningCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        go.SetActive(true);
    }

    private void RunningState() {        
        timerText.text = string.Format("{0:0.0}", timeElapsed);
        distanceText.text = string.Format("{0:0.0}", player.position.y);
        if(timerRunning)
            timeElapsed += Time.deltaTime;
    }

}
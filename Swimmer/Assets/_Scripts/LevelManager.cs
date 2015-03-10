using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : GameManager {

    public static LevelManager instance;

    public delegate void GamePlayEvent();
    public event GamePlayEvent gameStarted;
    public event GamePlayEvent gameEnded;

    private UIManager uim;
    private float startTime;
    public Text timerText;

    public override void Awake()
    {
        uim = GetComponent<UIManager>();
        if (instance == null) {
            instance = this;
        }
    }

    private IEnumerator StartCounter() {
        SetState(State.Running);
        for (int i = 3; i > 0; i--) {
            timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        timerText.text = "GO!";
        if(gameStarted != null) gameStarted();
        yield return new WaitForSeconds(1f);
        timerText.text = "";
    }
    

    public void PressBackButton() {
        if (GetState() == State.Running)
        {
            PauseGame();
        }
        else if (GetState() == State.Pause)
        {
            uim.ShowConfrimBox();
        }
        else if (GetState() == State.Menu)
        {
            uim.ShowConfrimBox();
        }
    }

    public void StartGame() {
        StartCoroutine("StartCounter");
    }

    public void ContinueGame() {
        SetState(State.Running);
        Time.timeScale = 1f;
    }
    public void PauseGame() {
        SetState(State.Pause);
        Time.timeScale = 0;
    }
    public void GameOver() {
        SetState(State.GameOver);
        if (gameEnded != null) gameEnded();
    }

    public void ConfirmButtonPress(bool pressYes) {
        if (pressYes) {
            switch (GetState()) {
                case State.Menu:
                    QuitApplication();
                    break;
                case State.Pause:
                    QuitGame();
                    break;
            }
        } else {
            uim.ExitConfrimBox(GetState());
        }
    }

    public void QuitGame() {
        SetState(State.Menu);
    }
    public void QuitApplication() {
        Application.Quit();
    }
}

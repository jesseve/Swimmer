using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Button buttonR;
    public Button buttonL;
    public Text buttonRText;
    public Text buttonLText;
    public Text boxText1;
    public Text timerText;
    public Text distanceText;

    public Transform player;

    private float timeElapsed;


	// Use this for initialization
	void Start () {
        timeElapsed = 0;
        buttonLText = buttonL.GetComponentInChildren<Text>();
        buttonRText = buttonR.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() != State.Running) return;
        timeElapsed += Time.deltaTime;
        timerText.text = string.Format("{0:0.0}", timeElapsed);
        distanceText.text = string.Format("{0:0.0}", player.position.y);
	}

    public void SetBoxTexts(string bRt, string bLt, string title) {
        
    }
}
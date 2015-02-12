using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private float startTime;
    public Text timerText;

	// Use this for initialization
	void Start () {
        StartCoroutine("StartCounter");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator StartCounter() {
        float timer = 4f;        
        int change = 1;
        timerText.text = (4-change).ToString();
        float start = Time.time;
        while (start + timer > Time.time) {
            if (Time.time - start > change) {
                change++;
                ChangeNumber(change);
            }
            yield return null;   
        }
        ChangeNumber(change);
    }

    private void ChangeNumber(int n) {
        timerText.text = (4 - n).ToString();
    }
}

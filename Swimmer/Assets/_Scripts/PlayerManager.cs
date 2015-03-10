using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public Transform goal;

    private Movement movement;

	// Use this for initialization
	void Start () {
        movement = GetComponent<Movement>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.instance.GetState() == State.Running && transform.position.y > goal.position.y) {
            LevelManager.instance.GameOver();
        }
	}

    public void Swipe(float distanceY) {
        movement.Move(distanceY);
    }

    public void PressBackButton()
    {
        LevelManager.instance.PressBackButton();
    }
}

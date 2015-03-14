using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour
{

    private PlayerManager manager;    


    //allow 2 touches at a time.
    private Touch touch1;
    private Touch touch2;
    private int touch1Id;
    private int touch2Id;

    private float amountSwiped; //Amount of screen swiped from top to down.

    // Use this for initialization
    void Start()
    {
        touch1Id = touch2Id = -1;
        manager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ONLY FOR TESTING
        //if(Input.GetMouseButtonDown(0))
        //    manager.Swipe(480f);
        //return;
        //END OF TESTING
        AllStates();
        if (LevelManager.instance.GetState() == State.Running)
            RunningInput();
    }

    private void AllStates() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            manager.PressBackButton();
        }
    }

    private void RunningInput() {        
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Began)
            {
                if (touch1Id < 0)
                {
                    touch1 = Input.touches[i];
                    touch1Id = touch1.fingerId;
                    //Debug.Log("Touch1");
                }
                else if (touch2Id < 0)
                {
                    touch2 = Input.touches[i];
                    touch2Id = touch2.fingerId;
                    //Debug.Log("Touch2");
                }
            }
            else if (Input.touches[i].phase == TouchPhase.Ended)
            {
                if (Input.touches[i].fingerId.Equals(touch1Id))
                {
                    //Debug.Log("Distance: " + Vector2.Distance(touch1.position, Input.touches[i].position));
                    if (touch1.position.y > Input.touches[i].position.y)
                        manager.Swipe(touch1.position.y - Input.touches[i].position.y);
                    touch1Id = -1;
                }
                else if (Input.touches[i].fingerId.Equals(touch2Id))
                {
                    //Debug.Log("Distance: " + Vector2.Distance(touch2.position, Input.touches[i].position));
                    if (touch2.position.y > Input.touches[i].position.y)
                        manager.Swipe(touch2.position.y - Input.touches[i].position.y);
                    touch2Id = -1;
                }
            }
        }
    }
}

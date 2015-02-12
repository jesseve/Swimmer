using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private Rigidbody2D rb;
    private Vector2 friction;
    private float moveAmount;

	// Use this for initialization
	void Start () {
	    rb = GetComponent<Rigidbody2D>();        
        moveAmount = 0.65f * Screen.height;
        friction = Vector2.up * 0.2f;        
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate() {
        if (rb.velocity.y > 0)
            rb.velocity -= friction;
        else
            rb.velocity = Vector2.zero;
    }

    public void Move(float amountSwiped) {        
        float force = amountSwiped / Screen.height * moveAmount;          
        rb.AddForce(Vector2.up * force);        
        SwapSprite();
    }

    private void SwapSprite() {
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}

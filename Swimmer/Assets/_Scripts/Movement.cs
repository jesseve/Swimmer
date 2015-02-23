using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 friction;
    private float moveAmount;
    private bool idle;

    public Sprite idleSprite;
    public Sprite runningSprite;
    

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	    rb = GetComponent<Rigidbody2D>();    
        moveAmount = 0.70f * Screen.height;
        friction = Vector2.up * Screen.height * 0.02f;        
	}
	
	// Update is called once per frame
	void Update () {
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = 1;
            //rb.velocity -= friction * Time.deltaTime;
        }
        else
        {
            rb.gravityScale = 0;
            idle = true;
            SwitchSprite();
            rb.velocity = Vector2.zero;
        }
	}

    

    public void Move(float amountSwiped) {        
        float force = amountSwiped / Screen.height * moveAmount;          
        rb.AddForce(Vector2.up * force);
        idle = false;
        SwitchSprite();
        SwapXScale();
    }

    private void SwitchSprite() {
        sr.sprite = idle ? idleSprite : runningSprite;
    }

    private void SwapXScale() {
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}

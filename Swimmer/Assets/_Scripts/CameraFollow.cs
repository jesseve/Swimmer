using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 p = transform.position;
        p.y = player.position.y;
        transform.position = p + Vector3.up * 2f;
	}
}

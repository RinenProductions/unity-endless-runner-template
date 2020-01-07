using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public int length = 1;
    // Use this for initialization
    void Start () {
        Physics.IgnoreLayerCollision(0, 9);
    }
	
}

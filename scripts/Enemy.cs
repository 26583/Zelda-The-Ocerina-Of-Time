using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int health = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hit()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

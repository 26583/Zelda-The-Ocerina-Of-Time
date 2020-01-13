using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject cam;
    [SerializeField]
    float speed = 5;
    Vector3 look;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 dir = new Vector3(0,0,0);
        if (Input.GetAxis("Vertical") != 0)
        {
            dir += cam.transform.forward * Input.GetAxis("Vertical");
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            dir += cam.transform.right * Input.GetAxis("Horizontal");
        }
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(transform.position + dir);
        if(dir == new Vector3(0, 0, 0))
        {
            timer += 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        if (timer >= 2)
        {
            //cam.transform.rotation = transform.rotation;
            cam.GetComponent<CameraMovement>().ChangeRot(transform.eulerAngles.y);
            Debug.Log(transform.eulerAngles.y);
            timer = 0;
        }
        //cam.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
}

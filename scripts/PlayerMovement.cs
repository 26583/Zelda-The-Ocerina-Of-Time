using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool jump = true;
    private int jumpAmount = 1;
    private Rigidbody rb;
    private bool walkable = true;
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject cam;
    [SerializeField]
    float speed = 5;
    Vector3 look;
    float timer;
    [SerializeField]
    LayerMask layer;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {

        //----------------------------------------------------------animations
        anim.SetBool("climbing", false);
        walkable = gameObject.GetComponent<Ladder>().isClimbing();
        if (Input.GetAxis("Back") > 0.3f)
        {
            walkable = false;
        }
        if (!walkable)
        {
            if (!jump)
            {
                speed = speed *0.4f;
            }
            else
            {
                speed = 5;
            }
            if (jump)
            {
                if (!gameObject.GetComponent<AttackScript>().Locked())
                {
                    Movement();
                }
                else
                {
                    Vector3 dir = new Vector3(0, 0, 0);
                    if (Input.GetAxis("Vertical") != 0)
                    {
                        dir += transform.forward * Input.GetAxis("Vertical");
                    }
                    if (Input.GetAxis("Horizontal") != 0)
                    {
                        dir += transform.right * Input.GetAxis("Horizontal");
                    }
                    dir.Normalize();
                    transform.position += dir * speed * Time.deltaTime;
                }
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    anim.SetBool("isRunning", true);
                }
                else
                {
                    anim.SetBool("isRunning", false);
                    anim.speed = 1;
                }
                
            }
            Jump();
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("climbing", true);
            anim.speed = 0;
            if(Input.GetAxis("Vertical") != 0)
            {
                anim.speed = 1;
            }

        }
    }
    void Movement()
    {
        // ---------------------------------------------------------PLayer Movement

        Vector3 dir = new Vector3(0, 0, 0);
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
        if (dir == new Vector3(0, 0, 0))
        {
            timer += Time.deltaTime;
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
        }
    }

    void Jump()
    {
        Debug.DrawRay(transform.position, -transform.up, Color.red);
        if (Physics.Raycast(transform.position, -transform.up,2.3f, layer))
        {
            Debug.Log("OnGround");
            jump = true;
        }
        else if(jump)
        {
            Debug.Log("Jump");
            jump = false;
            anim.SetBool("isRunning", false);
            rb.AddForce(new Vector3(0, 6f, 0) + transform.forward * 4f, ForceMode.Impulse);
        }
    }
}

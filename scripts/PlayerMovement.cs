using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool jump = false;
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
    [SerializeField]
    float distance = 0.8f;
    [SerializeField]
    Collider col;
    //Vector3 dirm = new Vector3(0, 0, 0);

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!StateMachine.GetCutsene())
        {
            //anim.SetBool("jump", !jump);
            //----------------------------------------------------------animations
            anim.SetBool("climbing", false);
            walkable = gameObject.GetComponent<Ladder>().isClimbing();
            if (Input.GetAxis("Back") > 0.3f)
            {
                walkable = false;
            }
            if (!walkable)
            {
                if (jump)
                {
                    Vector3 dir = new Vector3(0, 0, 0);
                    if (!gameObject.GetComponent<AttackScript>().Locked())
                    {
                        Movement();
                    }
                    else
                    {
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
                //dirm = cam.transform.forward;
                jump = false;
                //dir = transform.forward;
                anim.SetBool("isRunning", false);
                anim.SetBool("climbing", true);
                /*
                anim.speed = 0;

                if (Input.GetAxis("Vertical") != 0)
                {
                    anim.speed = 1;
                }
                if (Input.GetAxis("Vertical") > 0)
                {
                    anim.SetFloat("climbspeed", 1);
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    anim.SetFloat("climbspeed", -1);
                }*/
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
        //dirm = Vector3.Lerp(dirm, dir, Time.deltaTime);

        transform.LookAt(transform.position + dir);
        transform.position += dir * speed * Time.deltaTime;
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
        RaycastHit hit;
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0) + transform.forward * 0.1f, transform.position + new Vector3(0, 1, 0) + transform.forward * 0.1f - transform.up*distance, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0,1,0)+transform.forward*0.06f, -transform.up,out hit, distance, layer))
        {
            Debug.Log("OnGround");
            jump = true;
            anim.SetBool("jump", false);
            rb.interpolation = RigidbodyInterpolation.None;
            
        }
        else if(jump && !walkable)
        {
            
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            Debug.Log("Jump");
            jump = false;
            anim.SetBool("jump", true);
            anim.SetBool("isRunning", false);
            rb.AddForce(new Vector3(0, 6f, 0) + transform.forward * 3f, ForceMode.Impulse);
        }
    }
}

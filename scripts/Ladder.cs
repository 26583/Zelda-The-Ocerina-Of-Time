using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private float climbHeight = 20f;
    private Rigidbody rb;
    private bool insideLadder = false;
    bool climable = true;
    GameObject trap;
    [SerializeField]
    Animator anim;
    Ray checkHead;
    RaycastHit hit;
    bool enterStair = false;
    [SerializeField]
    GameObject cam;
    [SerializeField]
    GameObject child;
    [SerializeField]
    LayerMask layer;
    [SerializeField]
    GameObject hips;
    bool canClimb = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!StateMachine.GetCutsene())
        {
            if (insideLadder && !climable)
            {
                if (Input.GetAxis("Vertical") <= 0)
                {
                    //climable = true;
                    //enterStair = true;
                    StartCoroutine(makeClimb());
                }
            }
            if (Input.GetAxis("Back") > 0.3f)
            {
                insideLadder = false;
                rb.useGravity = true;
                StopCoroutine(TurnToTrap(0.7f));
            }
            if (insideLadder && climable && canClimb)
            {
                transform.position += new Vector3(0, Input.GetAxis("Vertical"), 0) * 3 * Time.deltaTime;
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
                }
            }
            else
            {
                anim.speed = 1;
            }
            if (!insideLadder)
            {
                anim.SetBool("climbing", false);
            }
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out hit, 1f, layer) && insideLadder)
            {

                enterStair = true;
                
            }
            else if (insideLadder && enterStair)
            {
                Debug.Log("lerp");
                climable = false;
                canClimb = false;
                //insideLadder = false;
                enterStair = false;
                StartCoroutine(GoUpTrep());
                anim.SetBool("hanging", true);
            }
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + transform.forward * 1f);
            Debug.Log(insideLadder);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == "Ground")
        {
            insideLadder = false;
        }*/
        if (other.gameObject.tag == "Trap")
        {
            canClimb = true;
            insideLadder = true;
            rb.useGravity = false;
            climable = true;
            enterStair = false;
            Debug.Log("Stair");
            //transform.position = new Vector3(other.gameObject.transform.position.x, transform.position.y, other.gameObject.transform.position.z) - other.gameObject.transform.forward * 0.5f;
            trap = other.gameObject;
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                climable = false;
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.6f,transform.position.z) - other.gameObject.transform.forward * 0.14f;
            }
            else
            {
                enterStair = true;
                transform.position = transform.position + other.gameObject.transform.forward * 0.3f;
            }
            
            StartCoroutine(TurnToTrap(0.7f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Trap")
        {
            insideLadder = false;
            rb.useGravity = true;
            if (transform.position.y > other.gameObject.transform.position.y&&enterStair)
            {
                //rb.AddForce(new Vector3(0, 0.5f, 0) + transform.forward * 0.7f, ForceMode.Impulse);
                StartCoroutine(GoUpTrep());
            }
            else
            {
                //rb.AddForce(new Vector3(0, 0, 0) - transform.forward * 0.7f, ForceMode.Impulse);
            }
           // StopAllCoroutines();
            enterStair = false;
        }
    }
    IEnumerator TurnToTrap(float t)
    {
        bool b = true;
        float f = 0;
        while (b)
        {
            f += t * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, trap.transform.rotation, f);
            cam.GetComponent<CameraMovement>().ChangeRot(transform.eulerAngles.y);
            yield return new WaitForSeconds(0f);
            if (f >= 1)
            {
                b = false;
            }
        }
    }
    IEnumerator makeClimb()
    {
        yield return new WaitForSeconds(0.4f);
        climable = true;
    }
    IEnumerator GoUpTrep()
    {
        /*
        bool b = true;
        float f = 0;
        while (b)
        {
            rb.useGravity = false;
            f += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0,0.17f,0)+transform.forward*0.3f , f*0.3f);
            //rb.constraints = RigidbodyConstraints.FreezeAll;
            //cam.GetComponent<CameraMovement>().ChangeRot(transform.eulerAngles.y);
            yield return new WaitForSeconds(0f);
            if (f >= 1)
            {
                b = false;
                rb.useGravity = true;
            }
        }*/
        /*
        bool b = true;
        float timer = 0;
        
        while (timer < 2)
        {
            //transform.SetParent(child.transform);
            rb.useGravity = false;
            timer +=Time.deltaTime;
                transform.position += transform.up * 0.4f * Time.deltaTime;
                transform.position += transform.forward * 0.3f * Time.deltaTime;
            
        }if(timer > 2)
        {
            rb.useGravity = true;
        }*/
        yield return new WaitForSeconds(1f);
        Debug.Log("Stop");
        anim.SetBool("hanging", false);
        transform.position = hips.transform.position - new Vector3(0,0.2f,0)+transform.forward*0.4f;
        anim.speed = 1;
        rb.useGravity = true;
        insideLadder = false;
        yield return new WaitForSeconds(0f);
        
    }
    
    public bool isClimbing()
    {
        return insideLadder;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    GameObject target;
    bool lockedOn = false;
    [SerializeField]
    GameObject cam1;
    [SerializeField]
    GameObject cam2;
    [SerializeField]
    Animator anim;
    bool attackeble =true;
    // Start is called before the first frame update
    void Start()
    {
        LockOnToTarget(6);
        cam1.SetActive(true);
        cam2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("L2") > 0.3f)
        {
            LockOnToTarget(6);
            lockedOn = true;
            cam1.SetActive(false);
            cam2.SetActive(true);
        }
        if (Input.GetAxis("L2") < 0.3f)
        {
            lockedOn = false;
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
        if(Input.GetAxis("Xbutton") > 0.3f && lockedOn && attackeble)
        {
            Debug.Log("Attack Babie met IONEUS");
            anim.SetBool("attack", true);
            StartCoroutine(AttackTime());
        }

        if (lockedOn) {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
        
    }
    void LockOnToTarget(float radius)
    {
        target = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.tag == "Enemy")
            {
                target = hitColliders[i].gameObject;
            }
        }
    }
    public bool Locked()
    {
        return lockedOn;
    }
    IEnumerator AttackTime()
    {
        attackeble = false;
        yield return new WaitForSeconds(0.6f);
        anim.SetBool("attack", false);
        target.GetComponent<Enemy>().Hit();
        attackeble = true;
    }
}

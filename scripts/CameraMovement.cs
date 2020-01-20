using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    float distance = 5;
    [SerializeField]
    float turnspeed = 0.1f;
    float yRotate;
    float yR;
    float timer = 0;
    [SerializeField]
    LayerMask layer;
    Transform rotat;
    // Start is called before the first frame update
    void Start()
    {
        rotat = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!StateMachine.GetCutsene())
        {
            if (Input.GetAxis("Horizontal") != 0)
                yRotate += Input.GetAxis("Horizontal") * turnspeed;


            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, yRotate, transform.rotation.z));
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotat.rotation, Time.time * 0.1f);
            transform.position = Vector3.Lerp(transform.position,target.transform.position - transform.forward * distance + new Vector3(0, 1f, 0),Time.deltaTime*10);
            RaycastHit hit;

            if (Physics.Raycast(target.transform.position + new Vector3(0, 1f, 0), -transform.forward, out hit, distance, layer))
            {
                transform.position = hit.point;
            }
        }
    }
    public void ChangeRot( float rot)
    {
        Debug.Log("Lerpies");
        yRotate =Mathf.LerpAngle(yRotate, rot, 1.4f * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingScript : MonoBehaviour
{

    private float hitDistance;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = 1 << 8;

        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit,Mathf.Infinity,layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*hit.distance);
            hitDistance = hit.distance;
        }
         

    }

    public float getHitDistance()
    {
        return hitDistance;
    }

    public Vector3 getCameraPosition()
    {
        return transform.position;
    }


}

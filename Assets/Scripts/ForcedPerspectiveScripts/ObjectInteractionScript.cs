using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionScript : MonoBehaviour
{


    public bool isForcedPerspective;

    RayTracingScript rayTracingInfo;
    Rigidbody body;
    Vector3 originalScale;
    float originalDistance;

    // Start is called before the first frame update
    void Start()
    {
        rayTracingInfo = FindObjectOfType<RayTracingScript>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDrag()
    {
        Vector3 mousePosition;

        if (isForcedPerspective)
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, rayTracingInfo.getHitDistance() - 1);
        }
        else
        {
            
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, originalDistance);
            if (rayTracingInfo.getHitDistance() < originalDistance)
            {
                Debug.Log("Distancia: " + originalDistance);
                mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, rayTracingInfo.getHitDistance());
            }
        }

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        body.velocity = new Vector3();

        //La magia
        Debug.Log(rayTracingInfo.getHitDistance());
        if(isForcedPerspective)
            transform.localScale = originalScale * (rayTracingInfo.getHitDistance() / originalDistance);
    }

    private void OnMouseDown()
    {
        //Guardar Distancia original y escala 
        originalScale = transform.localScale;
        originalDistance = Vector3.Distance(rayTracingInfo.getCameraPosition(), transform.position);
        
    }

    private void OnMouseUp()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{

    public Transform player;
    public Transform target;

    private bool playerContact = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
    
        if(playerContact){
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            Debug.Log(dotProduct);

            if (dotProduct < 0f){
                //TODO Arreglar la diferencia de rotación 

                //float rotationDiff = -Quaternion.Angle(player.rotation, target.rotation);
                float rotationDiff = 0;
                player.Rotate(Vector3.up, rotationDiff);
                Debug.Log("Go!");

                Vector3 positionOffset = target.position - player.position + new Vector3(0f, -1);
                player.position += positionOffset;
                 

                playerContact = false;
            }          
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            playerContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerContact = false;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparentWindowScript : MonoBehaviour
{
    public Transform player;
    public ApparentWindowScript targetWindow;
    public MeshRenderer screen;
    public bool windowEnabled = false;

    public static int recursionLimit = 5;
    MeshFilter screenMeshFilter;
    Camera playerCam;
    public Camera portalCam;
    RenderTexture viewTexture;

    Vector3 previousOffsetFromPortal;
    


    private bool playerContact = false;
    private bool hasAlreadyTeleported = false;

    private void Awake()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;
        screen.material.SetInt("displayMask", 1);
        screenMeshFilter = screen.GetComponent<MeshFilter>();
        
        
    }

    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCam.targetTexture = viewTexture;
            targetWindow.screen.material.SetTexture("_MainTex", viewTexture);

        }

        if(windowEnabled){
            targetWindow.screen.enabled = false;
        }


    }

    static bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
    }

    public void Render()
    {

        if (!VisibleFromCamera(targetWindow.screen, playerCam))
        {
            if (windowEnabled)
            {
                targetWindow.screen.enabled = false;
            }

            return;

        }
        Debug.Log("Render!");
        screen.enabled = false;
        CreateViewTexture();        

        if (playerCam == null)
        {
            Debug.Log("Null!");
        }

        var matrix = transform.localToWorldMatrix
        * targetWindow.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);


        SetNearClipPlane();
        portalCam.Render();
        screen.enabled = true;
    }


    // Update is called once per frame
    void LateUpdate()
    {

        if (playerContact && !hasAlreadyTeleported && windowEnabled)
        {
            var m = targetWindow.transform.localToWorldMatrix * transform.worldToLocalMatrix * player.localToWorldMatrix;
            teleportPlayer(transform, targetWindow.transform, m.GetColumn(3), m.rotation);
            targetWindow.OnTravellerEnterPortal();
            targetWindow.hasAlreadyTeleported = true;
            playerContact = false;
        }
    }

    private void teleportPlayer(Transform transform, Transform targetPortalTransform, Vector3 position, Quaternion rotation)
    {
        player.position = position;
        player.rotation = rotation;
    }

    private void OnTravellerEnterPortal()
    {
        previousOffsetFromPortal = player.transform.position - transform.position;
    }

    public void ProtectScreenFromClipping(Vector3 viewPoint)
    {
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, dstToNearClipPlaneCorner);
        screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);

    }

    void SetNearClipPlane()
    {
        Transform clipPlane = transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

        portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerContact = true;
            OnTravellerEnterPortal();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerContact = false;
            hasAlreadyTeleported = false;
        }
    }
}

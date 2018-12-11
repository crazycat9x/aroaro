using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AvatarTransformFollow : MonoBehaviourPunCallbacks
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

	// Use this for initialization
	void Start () {
        if (!photonView.IsMine) return;

        Transform sdkTransform = VRTK_SDKManager.instance.loadedSetup.actualBoundaries.transform;
        sdkTransform.position = transform.position;
        sdkTransform.rotation = transform.rotation;

        leftHand.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = VRTK_DeviceFinder.GetControllerLeftHand();
        rightHand.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = VRTK_DeviceFinder.GetControllerRightHand();


        head.GetComponent<MeshRenderer>().enabled = false;
        leftHand.GetComponent<MeshRenderer>().enabled = false;
        rightHand.GetComponent<MeshRenderer>().enabled = false;
        foreach(MeshRenderer r in head.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
        

	}

    void Update()
    {
        if (!photonView.IsMine) return;

        head.transform.position = VRTK_DeviceFinder.HeadsetTransform().position;
        head.transform.rotation = VRTK_DeviceFinder.HeadsetTransform().rotation;
    }
}

namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="AvatarTransformFollow" />
    /// </summary>
    public class AvatarTransformFollow : MonoBehaviourPun
    {
        /// <summary>
        /// Defines the head
        /// </summary>
        public GameObject head;

        /// <summary>
        /// Defines the leftHand
        /// </summary>
        public GameObject leftHand;

        /// <summary>
        /// Defines the rightHand
        /// </summary>
        public GameObject rightHand;

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        /// <summary>
        /// The OnEnable
        /// </summary>
        internal void OnEnable()
        {
            if (!photonView.IsMine) return;

            GameObject sdk = VRTK_SDKManager.instance.loadedSetup.actualBoundaries;

            leftHand.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = VRTK_DeviceFinder.GetControllerLeftHand();
            rightHand.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = VRTK_DeviceFinder.GetControllerRightHand();
            gameObject.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = sdk;

            head.GetComponent<MeshRenderer>().enabled = false;
            leftHand.GetComponent<MeshRenderer>().enabled = false;
            rightHand.GetComponent<MeshRenderer>().enabled = false;
            foreach (MeshRenderer r in head.GetComponentsInChildren<MeshRenderer>())
            {
                r.enabled = false;
            }
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            if (!photonView.IsMine) return;

            head.transform.position = VRTK_DeviceFinder.HeadsetTransform().position;
            head.transform.rotation = VRTK_DeviceFinder.HeadsetTransform().rotation;
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        internal void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}

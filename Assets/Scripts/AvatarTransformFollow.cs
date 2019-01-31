namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// The <see cref="AvatarTransformFollow" /> map the avatar position/rotation to the player movements and have that synced across the network
    /// </summary>
    public class AvatarTransformFollow : MonoBehaviourPun
    {
        /// <summary>
        /// The gameobject representing the avatar head
        /// </summary>
        public GameObject head;

        /// <summary>
        /// The gameobject representing the avatar left hand
        /// </summary>
        public GameObject leftHand;

        /// <summary>
        /// The gameobject representing the avatar right hand
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

namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;
    using VRTK.GrabAttachMechanics;

    /// <summary>
    /// The <see cref="ControllableObject" /> inherit from <see cref="VRTK_InteractableObject" /> and added the ability for object interaction to be synced across the network
    /// </summary>
    public class ControllableObject : VRTK_InteractableObject
    {
        /// <summary>
        /// Whether or not to display an object menu above the object
        /// </summary>
        [Header("Additional Options")]
        public bool displayObjectMenu = false;

        /// <summary>
        /// Defines the photonView
        /// </summary>
        private PhotonView photonView;

        /// <summary>
        /// Destroy the object this script attached to
        /// </summary>
        public void DestroyObject()
        {
            if (photonView != null)
            {
                photonView.RPC("DestroyNetworkedObject", RpcTarget.All);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Signal the owner to destroy the object
        /// </summary>
        [PunRPC]
        private void DestroyNetworkedObject()
        {
            if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        }

        /// <summary>
        /// Setup default grab mechanic
        /// </summary>
        private void SetupDefaultGrabMechanic()
        {
            VRTK_ChildOfControllerGrabAttach primaryGrabMechanic = gameObject.AddComponent<VRTK_ChildOfControllerGrabAttach>();
            primaryGrabMechanic.precisionGrab = true;
            grabAttachMechanicScript = primaryGrabMechanic;
        }

        /// <summary>
        /// Request ownership of the object on grab
        /// </summary>
        /// <param name="currentGrabbingObject">The currentGrabbingObject<see cref="VRTK_InteractGrab"/></param>
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            base.Grabbed(currentGrabbingObject);
            if (photonView != null)
                photonView.RequestOwnership();
        }

        /// <summary>
        /// Relinquish ownership of the object on ungrab
        /// </summary>
        /// <param name="previousGrabbingObject">The previousGrabbingObject<see cref="VRTK_InteractGrab"/></param>
        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
        {
            base.Ungrabbed(previousGrabbingObject);
            if (photonView != null)
                photonView.TransferOwnership(0);
        }

        /// <summary>
        /// The Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            photonView = gameObject.GetComponent<PhotonView>();
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            if (displayObjectMenu)
            {
                GameObject objectMenu = Resources.Load<GameObject>("ObjectMenu");
                ObjectMenu objectMenuScript = objectMenu.GetComponent<ObjectMenu>();
                objectMenuScript.targetGameObject = transform.gameObject;
                Instantiate(objectMenu, transform.position + new Vector3(0, transform.localScale.z, 0), transform.rotation, transform);
            }

            // Setup default grab mechanic if none is specified
            if (grabAttachMechanicScript == null)
                SetupDefaultGrabMechanic();
        }
    }
}

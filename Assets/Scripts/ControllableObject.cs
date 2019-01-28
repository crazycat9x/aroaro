namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;
    using VRTK.GrabAttachMechanics;

    /// <summary>
    /// Defines the <see cref="ControllableObject" />
    /// </summary>
    public class ControllableObject : VRTK_InteractableObject
    {

        private PhotonView photonView;

        /// <summary>
        /// The DestroyObject
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
        /// The DestroyNetworkedObject
        /// </summary>
        [PunRPC]
        private void DestroyNetworkedObject()
        {
            if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        }

        /// <summary>
        /// The SetupDefaultGrabMechanic
        /// </summary>
        private void SetupDefaultGrabMechanic()
        {
            VRTK_ChildOfControllerGrabAttach primaryGrabMechanic = gameObject.AddComponent<VRTK_ChildOfControllerGrabAttach>();
            primaryGrabMechanic.precisionGrab = true;
            grabAttachMechanicScript = primaryGrabMechanic;
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
            GameObject objectMenu = Resources.Load<GameObject>("ObjectMenu");
            ObjectMenu objectMenuScript = objectMenu.GetComponent<ObjectMenu>();
            objectMenuScript.targetGameObject = transform.gameObject;

            Instantiate(objectMenu, transform.position + new Vector3(0, transform.localScale.z, 0), transform.rotation, transform);

            // Return object ownership to scene so that anyone can take over
            if (photonView != null)
                photonView.TransferOwnership(0);

            // Setup default grab mechanic if none is specified
            if (grabAttachMechanicScript == null)
                SetupDefaultGrabMechanic();

            isGrabbable = false;
        }
    }
}

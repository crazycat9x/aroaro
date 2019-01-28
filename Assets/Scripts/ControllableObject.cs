namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;
    using VRTK.GrabAttachMechanics;

    /// <summary>
    /// Defines the <see cref="ControllableObject" />
    /// </summary>
    [RequireComponent(typeof(VRTK_InteractableObject))]
    public class ControllableObject : MonoBehaviourPun
    {

        /// <summary>
        /// Defines the interactableObject
        /// </summary>
        private VRTK_InteractableObject interactableObject;

        /// <summary>
        /// Defines the isGrabbable
        /// </summary>
        private bool isGrabbable;

        /// <summary>
        /// Gets or sets a value indicating whether IsGrabbable
        /// </summary>
        public bool IsGrabbable
        {
            get { return isGrabbable; }
            set
            {
                isGrabbable = value;
                interactableObject.isGrabbable = isGrabbable;
            }
        }

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
            interactableObject.grabAttachMechanicScript = primaryGrabMechanic;
            IsGrabbable = false;
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            interactableObject = gameObject.GetComponent<VRTK_InteractableObject>();
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

            // Setup default grab mechanic if none is specified
            if (gameObject.GetComponent<VRTK_BaseGrabAttach>() == null)
                SetupDefaultGrabMechanic();
        }
    }
}

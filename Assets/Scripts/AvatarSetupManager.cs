namespace Aroaro
{
    using ExitGames.Client.Photon;
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="AvatarSetupManager" />
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class AvatarSetupManager : MonoBehaviourPunCallbacks
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
        /// Defines the menuScript
        /// </summary>
        public RemotePlayerMenu menuScript;

        /// <summary>
        /// Defines the pointerCount
        /// </summary>
        [HideInInspector]
        public int pointerCount;

        /// <summary>
        /// Defines the instantiationData
        /// </summary>
        private object[] instantiationData;

        /// <summary>
        /// Defines the gazePointer
        /// </summary>
        private VRTK_StraightPointerRenderer gazePointer;

        /// <summary>
        /// The OnPlayerPropertiesUpdate
        /// </summary>
        /// <param name="target">The target<see cref="Player"/></param>
        /// <param name="changedProps">The changedProps<see cref="Hashtable"/></param>
        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
            if (target.ActorNumber == photonView.OwnerActorNr && !photonView.IsMine)
                ToggleGazePointer((bool)changedProps[CustomPlayerProperties.GazePointerState]);
        }

        /// <summary>
        /// The ToggleGazePointer
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void ToggleGazePointer(bool state)
        {
            VRTK_BasePointerRenderer.VisibilityStates visibilitySate = state ? VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn : VRTK_BasePointerRenderer.VisibilityStates.AlwaysOff;
            gazePointer.tracerVisibility = visibilitySate;
            gazePointer.cursorVisibility = visibilitySate;
        }

        /// <summary>
        /// The SetupAvatarColor
        /// </summary>
        private void SetupAvatarColor()
        {
            Color avatarColor = new Color((float)instantiationData[0], (float)instantiationData[1], (float)instantiationData[2], (float)instantiationData[3]);
            head.GetComponent<Renderer>().material.color = avatarColor;
            leftHand.GetComponent<Renderer>().material.color = avatarColor;
            rightHand.GetComponent<Renderer>().material.color = avatarColor;
        }

        /// <summary>
        /// The OnTriggerEnter
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerEnter(Collider other)
        {
            if (PointerUtilities.IsLocalPointer(other.gameObject))
            {
                pointerCount++;
                if (pointerCount > 0) StartCoroutine(menuScript.ToggleMenu(true));
            }
        }

        /// <summary>
        /// The OnTriggerExit
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerExit(Collider other)
        {
            if (PointerUtilities.IsLocalPointer(other.gameObject))
            {
                pointerCount--;
                if (pointerCount == 0) StartCoroutine(menuScript.ToggleMenu(false, 1f));
            }
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            instantiationData = gameObject.GetComponent<PhotonView>().InstantiationData;
            gazePointer = head.GetComponent<VRTK_StraightPointerRenderer>();
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            SetupAvatarColor();
            if (photonView.IsMine)
            {
                // Hide gaze pointer for local avatar by default
                ToggleGazePointer(false);

                // Make avatar invisible as we don't need to see our own avatar
                head.GetComponent<MeshRenderer>().enabled = false;
                leftHand.GetComponent<MeshRenderer>().enabled = false;
                rightHand.GetComponent<MeshRenderer>().enabled = false;
                foreach (MeshRenderer r in head.GetComponentsInChildren<MeshRenderer>())
                {
                    r.enabled = false;
                }

                // Disable menu & menu activation collider as it's only used for remote player
                menuScript.gameObject.SetActive(false);
                gameObject.GetComponent<Collider>().enabled = false;
            }
            else
            {
                // Get current gaze pointer state of remote user
                ToggleGazePointer((bool)photonView.Owner.CustomProperties[CustomPlayerProperties.GazePointerState]);
            }
        }
    }
}

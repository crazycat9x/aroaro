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
            if (target.ActorNumber == photonView.OwnerActorNr && !photonView.IsMine) ToggleGazePointer((bool)changedProps["gazePointerState"]);
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
                // Hide gaze line for local avatar by default
                ToggleGazePointer(false);
            }
            else
            {
                ToggleGazePointer((bool)photonView.Owner.CustomProperties["gazePointerState"]);
            }
        }
    }
}

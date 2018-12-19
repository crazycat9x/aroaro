namespace Aroaro
{
    using Photon.Pun;
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
        /// The ToggleGazePointer
        /// </summary>
        private void ToggleGazePointer()
        {
            if (photonView.IsMine) return;
            VRTK_StraightPointerRenderer gazePointer = head.GetComponent<VRTK_StraightPointerRenderer>();
            gazePointer.tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
            gazePointer.cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            instantiationData = gameObject.GetComponent<PhotonView>().InstantiationData;
            SetupAvatarColor();
            ToggleGazePointer();
        }
    }
}

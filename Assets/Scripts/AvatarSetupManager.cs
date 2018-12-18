namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="AvatarSetupManager" />
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class AvatarSetupManager : MonoBehaviour
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
        /// The Start
        /// </summary>
        internal void Start()
        {
            object[] instantiationData = gameObject.GetComponent<PhotonView>().InstantiationData;
            Color avatarColor = new Color((float)instantiationData[0], (float)instantiationData[1], (float)instantiationData[2], (float)instantiationData[3]);
            head.GetComponent<Renderer>().material.color = avatarColor;
            leftHand.GetComponent<Renderer>().material.color = avatarColor;
            rightHand.GetComponent<Renderer>().material.color = avatarColor;
        }
    }
}

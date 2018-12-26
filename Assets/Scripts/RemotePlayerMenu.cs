namespace Aroaro
{
    using Photon.Pun;
    using Photon.Voice.Unity;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="RemotePlayerMenu" />
    /// </summary>
    public class RemotePlayerMenu : MonoBehaviour
    {
        /// <summary>
        /// Defines the avatar
        /// </summary>
        public GameObject avatar;

        /// <summary>
        /// The ToggleSpeaker
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void ToggleSpeaker(bool state)
        {
            avatar.GetComponent<Speaker>().enabled = state;
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            if (avatar.GetComponent<PhotonView>().IsMine) gameObject.SetActive(false);
        }
    }
}

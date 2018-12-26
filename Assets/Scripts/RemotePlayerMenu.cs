namespace Aroaro
{
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

        public bool AvatarIsHovered { get; set; } = false;

        /// <summary>
        /// The ToggleSpeaker
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void ToggleSpeaker(bool state)
        {
            avatar.GetComponent<Speaker>().enabled = state;
        }
    }
}

namespace Aroaro
{
    using Photon.Voice.Unity;
    using System.Collections;
    using UnityEngine;
    using VRTK;

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
        /// Defines the avatarSetup
        /// </summary>
        public AvatarSetupManager avatarSetup;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Defines the pointerCount
        /// </summary>
        private int pointerCount = 0;

        /// <summary>
        /// The ToggleMenu
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        /// <param name="delay">The delay<see cref="float"/></param>
        /// <returns>The <see cref="IEnumerator"/></returns>
        public IEnumerator ToggleMenu(bool state, float delay = 0f)
        {
            while (delay > 0f)
            {
                if (!state && pointerCount == 0 && avatarSetup.pointerCount == 0) yield break;
                delay -= Time.deltaTime;
                yield return null;
            }
            if (state || pointerCount == 0 && avatarSetup.pointerCount == 0)
            {
                canvas.enabled = state;
                gameObject.GetComponent<Collider>().enabled = state;
            }
        }

        /// <summary>
        /// The ToggleSpeaker
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void ToggleSpeaker(bool state)
        {
            avatar.GetComponent<Speaker>().enabled = state;
        }

        /// <summary>
        /// The OnTriggerEnter
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<VRTK_PlayerObject>()?.objectType == VRTK_PlayerObject.ObjectTypes.Pointer)
                pointerCount++;
        }

        /// <summary>
        /// The OnTriggerExit
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<VRTK_PlayerObject>()?.objectType == VRTK_PlayerObject.ObjectTypes.Pointer)
            {
                pointerCount--;
                StartCoroutine(ToggleMenu(false));
            }
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            avatarSetup = avatar.GetComponent<AvatarSetupManager>();
            canvas = gameObject.GetComponent<Canvas>();
        }
    }
}

namespace Aroaro
{
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
        /// Defines the audioSource
        /// </summary>
        private AudioSource audioSource;

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
                if (!state && pointerCount + avatarSetup.pointerCount != 0) yield break;
                delay -= Time.deltaTime;
                yield return null;
            }
            if (state || pointerCount + avatarSetup.pointerCount == 0)
            {
                canvas.enabled = state;
                gameObject.GetComponent<Collider>().enabled = state;
            }
        }

        /// <summary>
        /// The Mute
        /// </summary>
        /// <param name="muted">The muted<see cref="bool"/></param>
        public void Mute(bool muted)
        {
            audioSource.volume = muted ? 0f : 1f;
        }

        /// <summary>
        /// The OnTriggerEnter
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerEnter(Collider other)
        {
            if (PointerUtilities.IsLocalPointer(other.gameObject))
                pointerCount++;
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
                StartCoroutine(ToggleMenu(false));
            }
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            canvas = gameObject.GetComponent<Canvas>();
            audioSource = avatar.GetComponent<AudioSource>();
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            if (canvas.enabled)
                transform.rotation = Quaternion.LookRotation(transform.position - VRTK_DeviceFinder.HeadsetTransform().position);
        }
    }
}

namespace Aroaro
{
    using Photon.Pun;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="Portal" />
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Portal : MonoBehaviour
    {
        /// <summary>
        /// Defines the sceneName
        /// </summary>
        public string sceneName;

        /// <summary>
        /// The LoadPortalScene
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/></returns>
        private IEnumerator LoadPortalScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            // Don't let the scene activate right away, we still need to leave the current room
            asyncLoad.allowSceneActivation = false;
            // Wait for scene to fully load
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    // Leave current room in preparation to enter new room
                    PhotonNetwork.LeaveRoom();
                }
                yield return null;
            }
        }

        /// <summary>
        /// The OnCollisionEnter
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerEnter(Collider other)
        {
            VRTK_PlayerObject playerObject = other.gameObject.GetComponent<VRTK_PlayerObject>();
            if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
            {
                StartCoroutine(LoadPortalScene());
            }
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }
}

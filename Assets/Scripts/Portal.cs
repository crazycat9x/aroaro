namespace Aroaro
{
    using Photon.Pun;
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
        /// The OnCollisionEnter
        /// </summary>
        /// <param name="other">The other<see cref="Collider"/></param>
        internal void OnTriggerEnter(Collider other)
        {
            VRTK_PlayerObject playerObject = other.gameObject.GetComponent<VRTK_PlayerObject>();
            if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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

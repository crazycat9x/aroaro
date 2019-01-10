namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="ControllableObject" />
    /// </summary>
    public class ControllableObject : MonoBehaviourPun
    {
        /// <summary>
        /// The DestroyObject
        /// </summary>
        public void DestroyObject()
        {
            if (photonView != null)
            {
                photonView.RPC("DestroyNetworkedObject", RpcTarget.All);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// The DestroyNetworkedObject
        /// </summary>
        [PunRPC]
        private void DestroyNetworkedObject()
        {
            if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            GameObject objectMenu = Resources.Load<GameObject>("ObjectMenu");
            ObjectMenu objectMenuScript = objectMenu.GetComponent<ObjectMenu>();
            objectMenuScript.targetGameObject = transform.gameObject;

            Instantiate(objectMenu, transform.position + new Vector3(0, transform.localScale.z, 0), transform.rotation, transform);
        }
    }
}

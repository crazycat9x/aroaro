namespace Aroaro
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Defines the <see cref="PUNNetworkManager" />
    /// </summary>
    public class PUNNetworkManager : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// Defines the gameVersion
        /// </summary>
        private string gameVersion = "1.0";

        /// <summary>
        /// Defines the avatar
        /// </summary>
        public GameObject avatar;

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions(), TypedLobby.Default);
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        /// <summary>
        /// The OnConnectedToMaster
        /// </summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name, new RoomOptions(), TypedLobby.Default);
        }

        /// <summary>
        /// The OnJoinRandomFailed
        /// </summary>
        /// <param name="returnCode">The returnCode<see cref="short"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(SceneManager.GetActiveScene().name, new RoomOptions());
        }

        /// <summary>
        /// The OnJoinedRoom
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            PhotonNetwork.Instantiate(avatar.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
}

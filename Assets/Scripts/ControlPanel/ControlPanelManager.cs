namespace Aroaro
{
    using ExitGames.Client.Photon;
    using Photon.Pun;
    using Photon.Voice.Unity;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="ControlPanelManager" />
    /// </summary>
    public class ControlPanelManager : MonoBehaviour
    {
        /// <summary>
        /// Defines the snapJoint
        /// </summary>
        public VRTK_TransformFollow snapJoint;

        /// <summary>
        /// Defines the recorder
        /// </summary>
        public Recorder recorder;

        /// <summary>
        /// Defines the whiteboard
        /// </summary>
        public GameObject whiteboard;

        /// <summary>
        /// Defines the leftController
        /// </summary>
        private GameObject leftController;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Defines the gazePointerState
        /// </summary>
        private bool gazePointerState;

        /// <summary>
        /// Defines the transmitEnabled
        /// </summary>
        private bool transmitEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether GazePointerState
        /// </summary>
        public bool GazePointerState
        {
            get { return gazePointerState; }
            set
            {
                gazePointerState = value;
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { CustomPlayerProperties.GazePointerState, gazePointerState } });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether TransmitEnabled
        /// </summary>
        public bool TransmitEnabled
        {
            get { return transmitEnabled; }
            set
            {
                transmitEnabled = value;
                recorder.TransmitEnabled = transmitEnabled;
            }
        }

        /// <summary>
        /// The toggleControlPanel
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void ToggleControlPanel(bool state)
        {

            canvas.enabled = state;
            gameObject.GetComponent<Collider>().enabled = state;
        }

        /// <summary>
        /// The CreateWhiteboard
        /// </summary>
        public void CreateWhiteboard()
        {
            Transform headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            Vector3 whiteboardPosition = headsetTransform.position + new Vector3(headsetTransform.forward.x, 0, headsetTransform.forward.z) * 2;
            Vector3 lookPosition = whiteboardPosition - headsetTransform.position;
            lookPosition.y = 0;
            PhotonNetwork.Instantiate(whiteboard.name, whiteboardPosition, Quaternion.LookRotation(lookPosition));
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
            canvas = gameObject.GetComponent<Canvas>();
        }

        /// <summary>
        /// The OnEnable
        /// </summary>
        internal void OnEnable()
        {
            leftController = VRTK_DeviceFinder.GetControllerLeftHand();
            snapJoint.gameObjectToFollow = leftController;
            ToggleControlPanel(true);
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            GazePointerState = true;
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        internal void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}

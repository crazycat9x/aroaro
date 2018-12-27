namespace Aroaro
{
    using ExitGames.Client.Photon;
    using Photon.Pun;
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
        /// Defines the gazePointerState
        /// </summary>
        private bool gazePointerState;

        /// <summary>
        /// Defines the leftController
        /// </summary>
        private GameObject leftController;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The toggleControlPanel
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void toggleControlPanel(bool state)
        {

            canvas.enabled = state;
            gameObject.GetComponent<Collider>().enabled = state;
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
            toggleControlPanel(true);
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

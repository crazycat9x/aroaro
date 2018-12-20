namespace Aroaro
{
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
        /// Defines the leftController
        /// </summary>
        private GameObject leftController;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Defines the vrtkCanvas
        /// </summary>
        private VRTK_UICanvas vrtkCanvas;

        /// <summary>
        /// The toggleControlPanel
        /// </summary>
        /// <param name="state">The state<see cref="bool"/></param>
        public void toggleControlPanel(bool state)
        {
            canvas.enabled = state;
            vrtkCanvas.enabled = state;
        }

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
            canvas = gameObject.GetComponent<Canvas>();
            vrtkCanvas = gameObject.GetComponent<VRTK_UICanvas>();
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
        /// The OnDestroy
        /// </summary>
        internal void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}

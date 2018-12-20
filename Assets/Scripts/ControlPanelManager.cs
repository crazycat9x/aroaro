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
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        /// <summary>
        /// The OnEnable
        /// </summary>
        internal void OnEnable()
        {
            leftController = VRTK_DeviceFinder.GetControllerLeftHand();
            snapJoint.gameObjectToFollow = leftController;
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

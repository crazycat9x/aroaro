namespace Aroaro
{
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="ControllersInputManager" />
    /// </summary>
    public class ControllersInputManager : MonoBehaviour
    {
        /// <summary>
        /// Defines the controllerEvents
        /// </summary>
        public VRTK_ControllerEvents controllerEvents;

        /// <summary>
        /// Defines the boundary
        /// </summary>
        private Transform boundary;

        /// <summary>
        /// The Awake
        /// </summary>
        private void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        /// <summary>
        /// The OnEnable
        /// </summary>
        private void OnEnable()
        {
            boundary = VRTK_DeviceFinder.PlayAreaTransform();
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {

            controllerEvents.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TouchpadPress, true, (object sender, ControllerInteractionEventArgs args) =>
            {
                Debug.Log(boundary);
                if (boundary == null) return;
                if (args.touchpadAxis.x > 0)
                {
                    boundary.rotation *= Quaternion.Euler(0, 30, 0);
                }
                else if (args.touchpadAxis.x < 0)
                {
                    boundary.rotation *= Quaternion.Euler(0, -30, 0);
                }
            });
        }
    }
}

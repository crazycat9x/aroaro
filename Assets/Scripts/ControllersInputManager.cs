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
        /// Defines the teleportPointer
        /// </summary>
        public VRTK_Pointer teleportPointer;

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
            teleportPointer.SelectionButtonPressed += (object _, ControllerInteractionEventArgs args) =>
            {
                // Wait for SDKSetup to load
                if (boundary == null) return;
                // Must be called twice to be able to turn off again (weird bug)
                teleportPointer.Toggle(true);
                teleportPointer.Toggle(true);
                if (args.touchpadAxis.x > 0.5)
                {
                    boundary.rotation *= Quaternion.Euler(0, 30, 0);
                }
                else if (args.touchpadAxis.x < -0.5)
                {
                    boundary.rotation *= Quaternion.Euler(0, -30, 0);
                }
            };
            teleportPointer.SelectionButtonReleased += (object _, ControllerInteractionEventArgs args) =>
            {
                if (args.touchpadAxis.x < -0.5 || args.touchpadAxis.x > 0.5 || args.touchpadAxis.y < 0) return;
                if(teleportPointer.IsStateValid())
                {
                    teleportPointer.DestinationMarkerSet += (object __, DestinationMarkerEventArgs ___) =>
                    {
                        teleportPointer.Toggle(false);
                    };
                }
                else
                {
                    teleportPointer.Toggle(false);
                }
            };
        }
    }
}

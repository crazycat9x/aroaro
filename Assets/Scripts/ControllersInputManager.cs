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
        /// Defines the dashTime
        /// </summary>
        public float dashTime;

        /// <summary>
        /// Defines the boundary
        /// </summary>
        private Transform boundary;

        /// <summary>
        /// The SelectionButtonPressedHandler
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="ControllerInteractionEventArgs"/></param>
        private void SelectionButtonPressedHandler(object sender, ControllerInteractionEventArgs args)
        {
            // Wait for SDKSetup to load
            if (boundary == null) return;

            // Touchpad right pressed
            if (args.touchpadAxis.x > 0.5)
            {
                boundary.rotation *= Quaternion.Euler(0, 30, 0);
            }
            // Touchpad left pressed
            else if (args.touchpadAxis.x < -0.5)
            {
                boundary.rotation *= Quaternion.Euler(0, -30, 0);
            }
            // Touchpad top pressed
            else if (args.touchpadAxis.y >= 0)
            {
                // Must be called twice to be able to turn off again (weird bug)
                teleportPointer.Toggle(true);
                teleportPointer.Toggle(true);
            }
            // Touchpad bottom pressed
            else
            {
                // TODO: Implement backward dash
            }
        }

        /// <summary>
        /// The SelectionButtonReleasedHandler
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="ControllerInteractionEventArgs"/></param>
        private void SelectionButtonReleasedHandler(object sender, ControllerInteractionEventArgs args)
        {
            if (!teleportPointer.IsStateValid()) teleportPointer.Toggle(false);
        }

        /// <summary>
        /// The DestinationMarkerSetHandler
        /// </summary>
        /// <param name="__">The __<see cref="object"/></param>
        /// <param name="___">The ___<see cref="DestinationMarkerEventArgs"/></param>
        private void DestinationMarkerSetHandler(object __, DestinationMarkerEventArgs ___)
        {
            teleportPointer.Toggle(false);
        }

        /// <summary>
        /// The TearDownEventHandlers
        /// </summary>
        private void TearDownEventHandlers()
        {
            teleportPointer.SelectionButtonPressed -= SelectionButtonPressedHandler;
            teleportPointer.SelectionButtonReleased -= SelectionButtonReleasedHandler;
            teleportPointer.DestinationMarkerSet -= DestinationMarkerSetHandler;
        }

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
            boundary = VRTK_DeviceFinder.PlayAreaTransform();
            teleportPointer.SelectionButtonPressed += SelectionButtonPressedHandler;
            teleportPointer.SelectionButtonReleased += SelectionButtonReleasedHandler;
            teleportPointer.DestinationMarkerSet += DestinationMarkerSetHandler;
        }

        /// <summary>
        /// The OnDisable
        /// </summary>
        internal void OnDisable()
        {
            TearDownEventHandlers();
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        internal void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
            TearDownEventHandlers();
        }
    }
}

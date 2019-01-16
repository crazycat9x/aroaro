namespace Aroaro
{
    using UnityEngine;
    using VRTK;
    using static VRTK.VRTK_BasePointerRenderer;

    /// <summary>
    /// Defines the <see cref="ControllersInputManager" />
    /// </summary>
    public class ControllersInputManager : MonoBehaviour
    {
        private enum PointerRenderers
        {
            StraightPointer,
            BezierPointer
        }

        public VRTK_Pointer pointer;

        public VRTK_StraightPointerRenderer straightPointerRenderer;

        public VRTK_BezierPointerRenderer bezierPointerRenderer;

        /// <summary>
        /// Defines the boundary
        /// </summary>
        private Transform boundary;

        private void TogglePointerRenderer(PointerRenderers pointerRenderer)
        {
            pointer.enabled = false;
            pointer.pointerRenderer.enabled = false;
            switch (pointerRenderer)
            {
                case PointerRenderers.StraightPointer:
                    pointer.enableTeleport = false;
                    pointer.pointerRenderer = straightPointerRenderer;
                    pointer.selectionButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
                    break;
                case PointerRenderers.BezierPointer:
                    pointer.interactWithObjects = false;
                    pointer.grabToPointerTip = false;
                    pointer.enableTeleport = true;
                    pointer.pointerRenderer = bezierPointerRenderer;
                    pointer.selectionButton = VRTK_ControllerEvents.ButtonAlias.TouchpadPress;
                    break;
            }
            pointer.enabled = true;
            pointer.pointerRenderer.enabled = true;
        }

        /// <summary>
        /// The TearDownEventHandlers
        /// </summary>
        private void TearDownEventHandlers()
        {
            pointer.controllerEvents.TouchpadPressed -= ControllerEvents_TouchpadPressed;
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
            pointer.controllerEvents.TouchpadPressed += ControllerEvents_TouchpadPressed;
            pointer.SelectionButtonReleased += Pointer_SelectionButtonReleased;
            pointer.DestinationMarkerSet += Pointer_DestinationMarkerSet;
        }

        private void Pointer_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        private void Pointer_SelectionButtonReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (pointer.enableTeleport && !pointer.IsStateValid())
                TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        internal void Start()
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        private void ControllerEvents_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            // Wait for SDKSetup to load
            if (boundary == null) return;

            // Touchpad right pressed
            if (e.touchpadAxis.x > 0.5)
                boundary.rotation *= Quaternion.Euler(0, 30, 0);
            // Touchpad left pressed
            else if (e.touchpadAxis.x < -0.5)
                boundary.rotation *= Quaternion.Euler(0, -30, 0);
            // Touchpad top pressed
            else if (e.touchpadAxis.y >= 0)
                TogglePointerRenderer(PointerRenderers.BezierPointer);
            // Touchpad bottom pressed
            else
            {
                // TODO: Implement backward dash
            }
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

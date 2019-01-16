﻿namespace Aroaro
{
    using Aroaro.Utilities;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="ControllersInputManager" />
    /// </summary>
    public class ControllersInputManager : MonoBehaviour
    {
        /// <summary>
        /// Defines the PointerRenderers
        /// </summary>
        private enum PointerRenderers
        {
            /// <summary>
            /// Defines the StraightPointer
            /// </summary>
            StraightPointer,

            /// <summary>
            /// Defines the BezierPointer
            /// </summary>
            BezierPointer
        }

        /// <summary>
        /// Defines the pointer
        /// </summary>
        public VRTK_Pointer pointer;

        /// <summary>
        /// Defines the straightPointerRenderer
        /// </summary>
        public VRTK_StraightPointerRenderer straightPointerRenderer;

        /// <summary>
        /// Defines the bezierPointerRenderer
        /// </summary>
        public VRTK_BezierPointerRenderer bezierPointerRenderer;

        /// <summary>
        /// Defines the interactTouch
        /// </summary>
        public VRTK_InteractTouch interactTouch;

        /// <summary>
        /// Defines the interactGrab
        /// </summary>
        public VRTK_InteractGrab interactGrab;

        /// <summary>
        /// Defines the boundary
        /// </summary>
        private Transform boundary;

        /// <summary>
        /// Defines the interactableObjectCount
        /// </summary>
        private int interactableObjectCount = 0;

        /// <summary>
        /// The ControllerEvents_TouchpadPressed
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ControllerInteractionEventArgs"/></param>
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
        /// The Pointer_DestinationMarkerSet
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DestinationMarkerEventArgs"/></param>
        private void Pointer_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        /// <summary>
        /// The Pointer_SelectionButtonReleased
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ControllerInteractionEventArgs"/></param>
        private void Pointer_SelectionButtonReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (pointer.enableTeleport && !pointer.IsStateValid())
                TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        /// <summary>
        /// The SetupControllerBehaviours
        /// </summary>
        /// <param name="controller">The controller<see cref="GameObject"/></param>
        private void SetupControllerBehaviours(GameObject controller)
        {
            BehavioursInjection childControllerBehaviours = controller.AddComponent<BehavioursInjection>();
            childControllerBehaviours.onTriggerEnter = (Collider other) =>
            {
                if (interactTouch.IsObjectInteractable(other.gameObject))
                    interactableObjectCount++;

                Debug.Log(interactableObjectCount);
                Debug.Log(interactGrab.GetGrabbedObject());
                if (interactableObjectCount > 0 && interactGrab.GetGrabbedObject() == null)
                {
                    pointer.enabled = false;
                    pointer.pointerRenderer.enabled = false;
                    pointer.interactWithObjects = false;
                    pointer.grabToPointerTip = false;
                    pointer.enabled = true;
                    pointer.pointerRenderer.enabled = true;
                }
            };

            childControllerBehaviours.onTriggerExit = (Collider other) =>
            {
                if (interactTouch.IsObjectInteractable(other.gameObject))
                    interactableObjectCount--;
                Debug.Log(interactableObjectCount);
                if (interactableObjectCount == 0)
                {
                    pointer.enabled = false;
                    pointer.pointerRenderer.enabled = false;
                    pointer.interactWithObjects = true;
                    pointer.grabToPointerTip = true;
                    pointer.enabled = true;
                    pointer.pointerRenderer.enabled = true;
                }
            };
        }

        /// <summary>
        /// The TogglePointerRenderer
        /// </summary>
        /// <param name="pointerRenderer">The pointerRenderer<see cref="PointerRenderers"/></param>
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
            pointer.SelectionButtonReleased -= Pointer_SelectionButtonReleased;
            pointer.DestinationMarkerSet -= Pointer_DestinationMarkerSet;
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

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            GameObject controller = transform.Find("[VRTK][AUTOGEN][Controller][CollidersContainer]").gameObject;
            if (controller.GetComponent<BehavioursInjection>() == null)
                SetupControllerBehaviours(controller);
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

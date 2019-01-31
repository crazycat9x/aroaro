namespace Aroaro
{
    using Aroaro.Utilities;
    using System;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// The <see cref="ControllersInputManager" /> map user input to an action
    /// </summary>
    public class ControllersInputManager : MonoBehaviour
    {
        private enum PointerRenderers
        {
            StraightPointer,
            BezierPointer
        }

        public CustomPointer pointer;
        public VRTK_StraightPointerRenderer straightPointerRenderer;
        public VRTK_BezierPointerRenderer bezierPointerRenderer;
        public VRTK_InteractTouch interactTouch;
        public VRTK_InteractGrab interactGrab;
        // The count of interactable object this controller is currently touching
        private int interactableObjectCount = 0;
        private bool pointerInteractWithObject;
        private PointerRenderers currentPointerRenderer = PointerRenderers.StraightPointer;

        /// <summary>
        /// Gets or sets a value indicating whether PointerInteractWithObject
        /// </summary>
        private bool PointerInteractWithObject
        {
            get { return pointerInteractWithObject; }
            set
            {
                // Check if we need to set as this is an expensive operation
                if (pointerInteractWithObject != value)
                {
                    pointerInteractWithObject = value;
                    pointer.enabled = false;
                    pointer.pointerRenderer.enabled = false;
                    pointer.grabToPointerTip = pointerInteractWithObject;
                    pointer.enabled = true;
                    pointer.pointerRenderer.enabled = true;
                }
            }
        }

        private void TogglePointerRenderer(PointerRenderers pointerRenderer)
        {
            if (currentPointerRenderer == pointerRenderer) return;
            currentPointerRenderer = pointerRenderer;
            pointer.enabled = false;
            pointer.pointerRenderer.enabled = false;
            switch (pointerRenderer)
            {
                case PointerRenderers.StraightPointer:
                    pointer.pointerRenderer = straightPointerRenderer;
                    break;
                case PointerRenderers.BezierPointer:
                    pointer.pointerRenderer = bezierPointerRenderer;
                    break;
            }
            pointer.enabled = true;
            pointer.pointerRenderer.enabled = true;
        }

        /// <summary>
        /// Handle touchpad pressed event depending on the pressed axis (up - teleport, left - rotate left, right - rotate right, down - dash backward)
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ControllerInteractionEventArgs"/></param>
        private void ControllerEvents_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            VRTK_SDKSetup loadedSetup = VRTK_SDKManager.instance.loadedSetup;
            if (loadedSetup == null) return;

            // Touchpad right pressed
            if (e.touchpadAxis.x > 0.5)
            {
                loadedSetup.actualBoundaries.transform.rotation *= Quaternion.Euler(0, 30, 0);
            }
            // Touchpad left pressed
            else if (e.touchpadAxis.x < -0.5)
            {
                loadedSetup.actualBoundaries.transform.rotation *= Quaternion.Euler(0, -30, 0);
            }
            // Touchpad top pressed
            else if (e.touchpadAxis.y >= 0)
            {
                TogglePointerRenderer(PointerRenderers.BezierPointer);
                pointer.enableTeleport = true;
            }
            // Touchpad bottom pressed
            else
            {
                // TODO: Implement backward dash
            }
        }

        /// <summary>
        /// Change the pointer renderer accordingly when touchpad is released
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="ControllerInteractionEventArgs"/></param>
        private void ControllerEvents_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (pointer.IsStateValid() && pointer.enableTeleport)
                pointer.Teleport();
            else
                TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        /// <summary>
        /// Change the pointer renderer accordingly after teleported
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DestinationMarkerEventArgs"/></param>
        private void Pointer_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
            pointer.enableTeleport = false;
        }

        /// <summary>
        /// The SetupControllerBehaviours setup the controller and its children to disable grab to pointer when controller is touching an interactable object and re-enable it otherwise
        /// </summary>
        /// <param name="controller">The controller<see cref="GameObject"/></param>
        private void SetupControllerBehaviours(GameObject controller)
        {
            Action<Collider> onTriggerEnter = (Collider other) =>
            {

                if (!interactTouch.IsObjectInteractable(other.gameObject)) return;
                interactableObjectCount++;
                if (interactableObjectCount > 0 && interactGrab.GetGrabbedObject() == null)
                    PointerInteractWithObject = false;
            };

            Action<Collider> onTriggerExit = (Collider other) =>
            {
                if (!interactTouch.IsObjectInteractable(other.gameObject)) return;
                interactableObjectCount--;
                if (interactableObjectCount == 0 && interactGrab.GetGrabbedObject() == null)
                    PointerInteractWithObject = true;
            };

            BehavioursInjection controllerBehaviours = controller.AddComponent<BehavioursInjection>();
            controllerBehaviours.onTriggerEnter = onTriggerEnter;
            controllerBehaviours.onTriggerExit = onTriggerExit;

            // Some controllers have childrens as colliders
            foreach (Transform childOfController in controller.transform)
            {
                if (childOfController.GetComponent<Collider>() != null && childOfController.GetComponent<BehavioursInjection>() == null)
                {
                    BehavioursInjection childOfControllerBehaviours = childOfController.gameObject.AddComponent<BehavioursInjection>();
                    childOfControllerBehaviours.onTriggerEnter = onTriggerEnter;
                    childOfControllerBehaviours.onTriggerExit = onTriggerExit;
                }
            }
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            pointer.controllerEvents.TouchpadPressed += ControllerEvents_TouchpadPressed;
            pointer.controllerEvents.TouchpadReleased += ControllerEvents_TouchpadReleased;
            pointer.DestinationMarkerSet += Pointer_DestinationMarkerSet;
            TogglePointerRenderer(PointerRenderers.StraightPointer);
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            GameObject controller = transform.Find("[VRTK][AUTOGEN][Controller][CollidersContainer]")?.gameObject;
            if (controller != null && controller.GetComponent<BehavioursInjection>() == null)
                SetupControllerBehaviours(controller);
        }
    }
}

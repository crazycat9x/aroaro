namespace Aroaro
{
    using Aroaro.Utilities;
    using System;
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
        public CustomPointer pointer;

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
        /// Defines the interactableObjectCount
        /// </summary>
        private int interactableObjectCount = 0;

        /// <summary>
        /// Defines the pointerInteractWithObject
        /// </summary>
        private bool pointerInteractWithObject;

        /// <summary>
        /// Defines the currentPointerRenderer
        /// </summary>
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
                    pointer.interactWithObjects = pointerInteractWithObject;
                    pointer.grabToPointerTip = pointerInteractWithObject;
                    pointer.enabled = true;
                    pointer.pointerRenderer.enabled = true;
                }
            }
        }

        /// <summary>
        /// The TogglePointerRenderer
        /// </summary>
        /// <param name="pointerRenderer">The pointerRenderer<see cref="PointerRenderers"/></param>
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
        /// The ControllerEvents_TouchpadPressed
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
        /// The ControllerEvents_TouchpadReleased
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
        /// The Pointer_DestinationMarkerSet
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DestinationMarkerEventArgs"/></param>
        private void Pointer_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            TogglePointerRenderer(PointerRenderers.StraightPointer);
            pointer.enableTeleport = false;
        }

        /// <summary>
        /// The SetupControllerBehaviours
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

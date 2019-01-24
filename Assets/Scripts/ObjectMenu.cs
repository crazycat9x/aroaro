namespace Aroaro
{
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="ObjectMenu" />
    /// </summary>
    public class ObjectMenu : MonoBehaviour
    {
        /// <summary>
        /// Defines the targetGameObject
        /// </summary>
        public GameObject targetGameObject;

        /// <summary>
        /// Defines the controllableObject
        /// </summary>
        [HideInInspector]
        public ControllableObject controllableObject;

        /// <summary>
        /// Gets or sets a value indicating whether IsGrabable
        /// </summary>
        public bool IsGrabable
        {
            get { return controllableObject.IsGrabbable; }
            set { controllableObject.IsGrabbable = value; }
        }

        /// <summary>
        /// The DestroyObject
        /// </summary>
        public void DestroyObject()
        {
            controllableObject.DestroyObject();
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Awake()
        {
            controllableObject = targetGameObject.GetComponent<ControllableObject>();
            if (controllableObject == null)
                controllableObject = targetGameObject.AddComponent<ControllableObject>();
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            Transform headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            if (headsetTransform != null)
                transform.rotation = Quaternion.LookRotation(transform.position - VRTK_DeviceFinder.HeadsetTransform().position);
        }
    }
}

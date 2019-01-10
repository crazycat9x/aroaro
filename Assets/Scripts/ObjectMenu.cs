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
        /// The Update
        /// </summary>
        internal void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - VRTK_DeviceFinder.HeadsetTransform().position);
        }
    }
}

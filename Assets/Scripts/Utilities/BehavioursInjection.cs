namespace Aroaro
{
    namespace Utilities
    {
        using System;
        using UnityEngine;

        /// <summary>
        /// Defines the <see cref="BehavioursInjection" />
        /// </summary>
        public class BehavioursInjection : MonoBehaviour
        {
            /// <summary>
            /// Defines the onTriggerEnter
            /// </summary>
            public Action<Collider> onTriggerEnter = (Collider c) => { };

            /// <summary>
            /// Defines the onTriggerExit
            /// </summary>
            public Action<Collider> onTriggerExit = (Collider c) => { };

            /// <summary>
            /// Defines the onCollisionEnter
            /// </summary>
            public Action<Collision> onCollisionEnter = (Collision c) => { };

            /// <summary>
            /// Defines the onCollisionExit
            /// </summary>
            public Action<Collision> onCollisionExit = (Collision c) => { };

            /// <summary>
            /// The OnTriggerEnter
            /// </summary>
            /// <param name="other">The other<see cref="Collider"/></param>
            internal void OnTriggerEnter(Collider other)
            {
                onTriggerEnter(other);
            }

            /// <summary>
            /// The OnTriggerExit
            /// </summary>
            /// <param name="other">The other<see cref="Collider"/></param>
            internal void OnTriggerExit(Collider other)
            {
                onTriggerExit(other);
            }

            /// <summary>
            /// The OnCollisionEnter
            /// </summary>
            /// <param name="collision">The collision<see cref="Collision"/></param>
            internal void OnCollisionEnter(Collision collision)
            {
                onCollisionEnter(collision);
            }

            /// <summary>
            /// The OnCollisionExit
            /// </summary>
            /// <param name="collision">The collision<see cref="Collision"/></param>
            internal void OnCollisionExit(Collision collision)
            {
                onCollisionExit(collision);
            }
        }
    }
}

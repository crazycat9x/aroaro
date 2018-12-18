namespace Aroaro
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Pen" />
    /// </summary>
    public class Pen : MonoBehaviour
    {
        /// <summary>
        /// Defines the penTipTransform
        /// </summary>
        private Transform penTipTransform;

        /// <summary>
        /// Defines the hit
        /// </summary>
        private RaycastHit hit;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Drawable canvas;

        /// <summary>
        /// The OnCollisionEnter
        /// </summary>
        /// <param name="collision">The collision<see cref="Collision"/></param>
        internal void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Drawable>() != null)
            {
                canvas = collision.gameObject.GetComponent<Drawable>();
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        /// <summary>
        /// The OnCollisionExit
        /// </summary>
        /// <param name="collision">The collision<see cref="Collision"/></param>
        internal void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<Drawable>() != null)
            {
                canvas = null;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            penTipTransform = transform.Find("Tip");
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            if (canvas != null && Physics.Raycast(penTipTransform.position, transform.up, out hit))
            {
                canvas.Draw(gameObject.GetInstanceID(), new Vector2(hit.textureCoord.x, hit.textureCoord.y), 10, Color.blue);
            }
        }
    }
}

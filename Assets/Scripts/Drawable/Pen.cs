namespace Aroaro
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Pen" />
    /// </summary>
    public class Pen : MonoBehaviour
    {
        /// <summary>
        /// Defines the penColor
        /// </summary>
        [SerializeField]
        private Color penColor;

        /// <summary>
        /// Defines the penSize
        /// </summary>
        [Range(1, 10)]
        public int penSize;

        /// <summary>
        /// Defines the penTipTransform
        /// </summary>
        private Transform penTipTransform;

        /// <summary>
        /// Defines the penEndTransform
        /// </summary>
        private Transform penEndTransform;

        /// <summary>
        /// Defines the hit
        /// </summary>
        private RaycastHit hit;

        /// <summary>
        /// Defines the canvas
        /// </summary>
        private Drawable canvas;

        /// <summary>
        /// Gets or sets the PenColor
        /// </summary>
        public Color PenColor
        {
            get { return penColor; }
            set
            {
                penColor = value;
                penTipTransform.gameObject.GetComponent<Renderer>().material.color = penColor;
                penEndTransform.gameObject.GetComponent<Renderer>().material.color = penColor;
            }
        }

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
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            penTipTransform = transform.Find("Tip");
            penEndTransform = transform.Find("End");
            PenColor = penColor;
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            if (Application.isPlaying && canvas != null && Physics.Raycast(penTipTransform.position, transform.up, out hit))
            {
                canvas.Draw(gameObject.GetInstanceID(), new Vector2(hit.textureCoord.x, hit.textureCoord.y), penSize, PenColor);
            }
        }
    }
}

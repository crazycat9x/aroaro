namespace Aroaro
{
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="Pen" />
    /// </summary>
    public class Pen : VRTK_InteractableObject
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
        /// Defines the resetTransformOnDrop
        /// </summary>
        public bool resetTransformOnDrop;

        /// <summary>
        /// The parent GameObject or "holder" of this pen
        /// </summary>
        [Tooltip("Default to the GameObject this script is attached to")]
        public Transform originalTransform;

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
        /// Defines the originalPosition
        /// </summary>
        private Vector3 originalPosition;

        /// <summary>
        /// Defines the originalRotation
        /// </summary>
        private Quaternion originalRotation;

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
        /// The Grabbed
        /// </summary>
        /// <param name="currentGrabbingObject">The currentGrabbingObject<see cref="VRTK_InteractGrab"/></param>
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            base.Grabbed(currentGrabbingObject);
            transform.parent = null;
        }

        /// <summary>
        /// The Ungrabbed
        /// </summary>
        /// <param name="previousGrabbingObject">The previousGrabbingObject<see cref="VRTK_InteractGrab"/></param>
        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
        {
            base.Ungrabbed(previousGrabbingObject);
            if (!resetTransformOnDrop) return;
            if (originalTransform != null)
            {
                transform.position = originalTransform.position;
                transform.rotation = originalTransform.rotation;
                transform.parent = originalTransform;
            }
            else
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
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
        protected override void Awake()
        {
            base.Awake();
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            penTipTransform = transform.Find("Tip");
            penEndTransform = transform.Find("End");
            PenColor = penColor;
        }

        /// <summary>
        /// The Update
        /// </summary>
        protected override void Update()
        {
            base.Update();
            if (IsUsing() && Application.isPlaying && canvas != null && Physics.Raycast(penTipTransform.position, transform.up, out hit))
            {
                canvas.Draw(gameObject.GetInstanceID(), new Vector2(hit.textureCoord.x, hit.textureCoord.y), penSize, PenColor);
            }
        }
    }
}

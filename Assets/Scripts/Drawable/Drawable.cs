namespace Aroaro
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Drawable" />
    /// </summary>
    public class Drawable : MonoBehaviour
    {
        /// <summary>
        /// Defines the textureWidth
        /// </summary>
        public int textureWidth = 2048;

        /// <summary>
        /// Defines the textureHeight
        /// </summary>
        private int textureHeight;

        /// <summary>
        /// Defines the texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Defines the previousPositions
        /// </summary>
        private Dictionary<int, Vector2> previousPositions = new Dictionary<int, Vector2>();

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="penId">The penId<see cref="int"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="penSize">The penSize<see cref="int"/></param>
        /// <param name="penColor">The penColor<see cref="Color"/></param>
        public void Draw(int penId, Vector2 position, int penSize, Color penColor)
        {
            int x = Mathf.RoundToInt(position.x * texture.height - (penSize / 2));
            int y = Mathf.RoundToInt(position.y * texture.width - (penSize / 2));
            Color[] colors = Enumerable.Repeat<Color>(penColor, penSize * penSize).ToArray<Color>();

            texture.SetPixels(x, y, penSize, penSize, colors);

            Vector2 previousPosition;
            if (previousPositions.TryGetValue(penId, out previousPosition))
            {
                for (float t = 0.01f; t < 1.00f; t += 0.01f)
                {
                    int lerpX = Mathf.RoundToInt(Mathf.Lerp(previousPosition.x, (float)x, t));
                    int lerpY = Mathf.RoundToInt(Mathf.Lerp(previousPosition.y, (float)y, t));
                    texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                }
            }
            previousPositions[penId] = new Vector2(x, y);
        }

        /// <summary>
        /// The OnCollisionExit
        /// </summary>
        /// <param name="collision">The collision<see cref="Collision"/></param>
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<Pen>() != null) previousPositions.Remove(collision.gameObject.GetInstanceID());
        }

        /// <summary>
        /// The Start
        /// </summary>
        private void Start()
        {
            float localScaleY = transform.localScale.y;
            float localScaleZ = transform.localScale.z;
            textureHeight = Mathf.RoundToInt(textureWidth * localScaleY / localScaleZ);
            texture = new Texture2D(textureWidth, textureHeight);
            GetComponent<Renderer>().material.mainTexture = (Texture)texture;
        }

        /// <summary>
        /// The Update
        /// </summary>
        private void Update()
        {
            if (previousPositions.Count() > 0) texture.Apply();
        }
    }
}

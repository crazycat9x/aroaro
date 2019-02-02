namespace Aroaro
{
    using Paroxe.PdfRenderer;
    using Paroxe.PdfRenderer.WebGL;
    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Drawable" />
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class Drawable : MonoBehaviour
    {
        public enum SourceFileType
        {
            PDF
        }

        public string textureSource;
        public SourceFileType textureSourceType;

        private int textureWidth;
        private int textureHeight;
        private Texture2D texture;

        /// <summary>
        /// Previous pen position
        /// </summary>
        private Dictionary<int, Vector2> previousPositions = new Dictionary<int, Vector2>();

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="penId">The penId<see cref="int"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="penSize">The penSize<see cref="int"/></param>
        /// <param name="penColor">The penColor<see cref="Color"/></param>
        public void Draw(int penId, Vector2 position, int penSize, Color32 penColor)
        {
            gameObject.GetComponent<PhotonView>().RPC("DrawAndBroadCast", RpcTarget.AllBufferedViaServer, penId, position, penSize, penColor.r, penColor.g, penColor.b, penColor.a);
        }

        /// <summary>
        /// The DrawAndBroadCast
        /// </summary>
        /// <param name="penId">The penId<see cref="int"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="penSize">The penSize<see cref="int"/></param>
        /// <param name="r">The r<see cref="byte"/></param>
        /// <param name="g">The g<see cref="byte"/></param>
        /// <param name="b">The b<see cref="byte"/></param>
        /// <param name="a">The a<see cref="byte"/></param>
        [PunRPC]
        private void DrawAndBroadCast(int penId, Vector2 position, int penSize, byte r, byte g, byte b, byte a)
        {
            Color32 penColor = new Color32(r, g, b, a);
            int x = Mathf.RoundToInt(position.x * texture.width - (penSize / 2));
            int y = Mathf.RoundToInt(position.y * texture.height - (penSize / 2));
            Color32[] colors = Enumerable.Repeat<Color32>(penColor, penSize * penSize).ToArray<Color32>();

            texture.SetPixels32(x, y, penSize, penSize, colors);

            Vector2 previousPosition;
            if (previousPositions.TryGetValue(penId, out previousPosition))
            {
                for (float t = 0.01f; t < 1.00f; t += 0.01f)
                {
                    int lerpX = Mathf.RoundToInt(Mathf.Lerp(previousPosition.x, (float)x, t));
                    int lerpY = Mathf.RoundToInt(Mathf.Lerp(previousPosition.y, (float)y, t));
                    texture.SetPixels32(lerpX, lerpY, penSize, penSize, colors);
                }
            }
            previousPositions[penId] = new Vector2(x, y);
        }

        /// <summary>
        /// The EndStroke
        /// </summary>
        /// <param name="penId">The penId<see cref="int"/></param>
        [PunRPC]
        private void EndStroke(int penId)
        {
            previousPositions.Remove(penId);
        }

        /// <summary>
        /// The OnCollisionExit
        /// </summary>
        /// <param name="collision">The collision<see cref="Collision"/></param>
        internal void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<Pen>() != null) gameObject.GetComponent<PhotonView>().RPC("EndStroke", RpcTarget.AllBufferedViaServer, collision.gameObject.GetInstanceID());
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal IEnumerator Start()
        {
            float localScaleX = transform.localScale.x;
            float localScaleY = transform.localScale.y;
            if (localScaleY > localScaleX)
            {
                textureWidth = 1024;
                textureHeight = Mathf.RoundToInt(textureWidth * localScaleY / localScaleX);
            }
            else
            {
                textureHeight = 1024;
                textureWidth = Mathf.RoundToInt(textureWidth * localScaleX / localScaleY);
            }
            texture = new Texture2D(textureWidth, textureHeight);
            
            // Load pdf as texture if source is given
            if (textureSource != null && textureSourceType == SourceFileType.PDF)
            {
                PDFJS_Promise<PDFDocument> documentPromise = PDFDocument.LoadDocumentFromUrlAsync(textureSource);
                while (!documentPromise.HasFinished)
                    yield return null;
                PDFDocument document = documentPromise.Result;
                if (document.IsValid)
                {
                    PDFRenderer renderer = new PDFRenderer();
                    // Display first page for testing
                    texture = renderer.RenderPageToTexture(document.GetPage(0), 1024, 1024);
                }
            }
            GetComponent<Renderer>().material.mainTexture = (Texture)texture;
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
            if (previousPositions.Count() != 0) texture.Apply();
        }
    }
}

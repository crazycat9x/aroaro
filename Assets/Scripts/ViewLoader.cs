namespace Aroaro
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Defines the <see cref="ViewLoader" />
    /// </summary>
    public class ViewLoader : MonoBehaviour
    {
        /// <summary>
        /// Defines the coreSystemScene
        /// </summary>
        private string coreSystemScene = "CoreSystem";

        // Use this for initialization
        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            SceneManager.LoadScene(coreSystemScene, LoadSceneMode.Additive);
        }

        // Update is called once per frame
        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
        }
    }
}

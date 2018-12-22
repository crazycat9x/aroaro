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
        /// Defines the coreSystem
        /// </summary>
        private string coreSystem = "CoreSystem";

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            if (GameObject.Find(coreSystem)) SceneManager.LoadScene(coreSystem, LoadSceneMode.Additive);
        }

        /// <summary>
        /// The Update
        /// </summary>
        internal void Update()
        {
        }
    }
}

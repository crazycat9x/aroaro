namespace Aroaro
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the <see cref="GazePointerToggle" />
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class GazePointerToggle : MonoBehaviour
    {
        /// <summary>
        /// Defines the controlPanelManager
        /// </summary>
        public ControlPanelManager controlPanelManager;

        /// <summary>
        /// Defines the toggle
        /// </summary>
        private Toggle toggle;

        /// <summary>
        /// The Awake
        /// </summary>
        internal void Awake()
        {
            toggle = gameObject.GetComponent<Toggle>();
        }

        /// <summary>
        /// The Start
        /// </summary>
        internal void Start()
        {
            toggle.onValueChanged.AddListener((value) => { controlPanelManager.GazePointerState = value; });
        }
    }
}

namespace Aroaro
{
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="SpawnManager" />
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Defines the defaultSpawnPosition
        /// </summary>
        public Vector3 defaultSpawnPosition = new Vector3(0, 0, 0);

        /// <summary>
        /// Defines the defaultSpawnRotation
        /// </summary>
        public Quaternion defaultSpawnRotation = Quaternion.Euler(0, 0, 0);

        /// <summary>
        /// Defines the boundary
        /// </summary>
        private Transform boundary;

        /// <summary>
        /// Defines the hasSpawned
        /// </summary>
        private bool hasSpawned = false;

        /// <summary>
        /// The Respawn
        /// </summary>
        public void Respawn()
        {
            boundary = VRTK_DeviceFinder.PlayAreaTransform();
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            if (spawnPoints.Length > 0)
            {
                GameObject chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                boundary.transform.position = chosenSpawnPoint.transform.position;
                boundary.transform.rotation = chosenSpawnPoint.transform.rotation;
            }
            else
            {
                boundary.transform.position = defaultSpawnPosition;
                boundary.transform.rotation = defaultSpawnRotation;
            }
            hasSpawned = true;
        }

        /// <summary>
        /// The Awake
        /// </summary>
        private void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        /// <summary>
        /// The OnEnable
        /// </summary>
        private void OnEnable()
        {
            if (!hasSpawned) Respawn();
        }

        /// <summary>
        /// The OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}

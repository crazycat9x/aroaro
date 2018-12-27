namespace Aroaro
{
    using Photon.Pun;
    using UnityEngine;
    using VRTK;

    /// <summary>
    /// Defines the <see cref="PointerUtilities" />
    /// </summary>
    public static class PointerUtilities
    {
        /// <summary>
        /// The IsPointer
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsPointer(GameObject gameObject)
        {
            if (gameObject.gameObject.GetComponent<VRTK_PlayerObject>()?.objectType == VRTK_PlayerObject.ObjectTypes.Pointer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The IsLocalPointer
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsLocalPointer(GameObject gameObject)
        {
            if (!IsPointer(gameObject)) return false;
            if (gameObject.name.StartsWith("[VRTK][AUTOGEN][Head]"))
            {
                if (gameObject.GetComponentInParent<VRTK_TransformFollow>()?.gameObjectToFollow.GetComponent<PhotonView>()?.IsMine ?? false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}

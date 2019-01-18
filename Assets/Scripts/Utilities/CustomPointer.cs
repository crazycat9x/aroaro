namespace Aroaro
{
    using VRTK;

    /// <summary>
    /// Defines the <see cref="CustomPointer" />
    /// </summary>
    public class CustomPointer : VRTK_Pointer
    {
        /// <summary>
        /// The Teleport
        /// </summary>
        public void Teleport()
        {
            enableTeleport = true;
            ExecuteSelectionButtonAction();
        }
    }

}
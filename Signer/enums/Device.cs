namespace com.mkysoft.gib.signer.enums
{
    /// <summary>
    /// Device to use for signing
    /// </summary>
    public enum Device
    {
        /// <summary>
        /// File, mostly used for test
        /// </summary>
        pfx = 0,
        /// <summary>
        /// Device in usb port
        /// </summary>
        usb = 1,
        /// <summary>
        /// Use hsm
        /// </summary>
        hsm = 2
    }
}

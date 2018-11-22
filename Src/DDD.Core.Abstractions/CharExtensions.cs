namespace DDD
{
    public static class CharExtensions
    {
        #region Methods

        /// <summary>
        /// Indicates whether the specified Unicode character is categorized as a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>
        ///   <c>true</c> if c is a decimal digit; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDigit(this char c) => char.IsDigit(c);

        /// <summary>
        /// Indicates whether the specified Unicode character is categorized as a Unicode letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>
        ///   <c>true</c> if c is a letter; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLetter(this char c) => char.IsLetter(c);

        /// <summary>
        /// Indicates whether the specified Unicode character is categorized as a letter or a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>
        ///   <c>true</c> if c is a letter or a decimal digit; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLetterOrDigit(this char c) => char.IsLetterOrDigit(c);

        #endregion Methods
    }
}

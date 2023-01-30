using EnsureThat;

namespace DDD
{
    public static class StringParamExtensions
    {

        #region Methods

        public static void IsAllLetters(this in StringParam param)
            => Ensure.String.IsAllLetters(param.Value, param.Name, param.OptsFn);

        public static void IsAllDigits(this in StringParam param)
            => Ensure.String.IsAllDigits(param.Value, param.Name, param.OptsFn);

        public static void HasMinLength(this in StringParam param, int minLength)
            => Ensure.String.HasMinLength(param.Value, minLength, param.Name, param.OptsFn);

        #endregion Methods

    }
}

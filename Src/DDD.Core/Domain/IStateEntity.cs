namespace DDD.Core.Domain
{
    /// <summary>
    /// An entity of the State Model.
    /// </summary>
    public interface IStateEntity
    {
        #region Properties

        EntityState EntityState { get; set; }

        #endregion Properties
    }
}
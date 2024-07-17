namespace Framework.Core.Interfaces
{
    public interface IDateTimeSignature : ICreationTimeSignature
    {
        #region Properties
        DateTime? FirstModificationDate { get; set; }
        DateTime? LastModificationDate { get; set; }
        #endregion
    }
}

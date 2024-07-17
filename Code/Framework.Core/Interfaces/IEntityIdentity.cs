namespace Framework.Core.Interfaces
{
    public interface IEntityIdentity<TPrimeryKey>
    {
        #region Properties
        public TPrimeryKey Id { get; set; }
        #endregion
    }
}

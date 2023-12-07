namespace ConduitAPI.EntityCommon
{

    public interface IAuditInfo

    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}

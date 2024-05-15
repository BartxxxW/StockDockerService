namespace StockAPI.DtoModels
{
    public class DatabaseProperties
    {
        public string DatabaseName { get; set; }
        public string SnapshotName { get; set; }

        public string MasterContext { get; set; }
        public string DbContext { get; set; }
    }
}

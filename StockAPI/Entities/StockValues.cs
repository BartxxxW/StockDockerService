namespace StockAPI.Entities
{
    public class StockValues
    {
        public int Id { get;set; }
        public string Name { get; set; }
        public decimal Price { get; set; }   
        public decimal Volume { get; set; }   
        public DateTime date { get; set; }   
    }
}

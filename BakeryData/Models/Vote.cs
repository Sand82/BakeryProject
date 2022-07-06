namespace BakeryData.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int ProductId { get; set; }       

        public Product Product { get; set; }

        public string UsreId { get; set; }

        public byte Value { get; set; }
    }
}

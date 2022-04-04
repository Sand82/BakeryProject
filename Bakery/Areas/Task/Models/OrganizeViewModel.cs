namespace Bakery.Areas.Task.Models
{
    public class OrganizeViewModel
    {
        public decimal TottalPrice { get; set; }

        public DateTime DayOfOrder { get; set; }

        public string ColapsValue { get; set; }

        public Dictionary<string, Dictionary<decimal, int>> Items { get; set; } =
            new Dictionary<string, Dictionary<decimal, int>>();
    }
}

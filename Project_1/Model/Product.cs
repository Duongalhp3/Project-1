public class Product
{
    public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
    public int quantity { get; set; } // this have 2 mean: 1-is inventory quantity, 2-is order quantity
    public string status { get; set; }
}

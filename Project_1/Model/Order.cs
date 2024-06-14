public class Order
{
    public int id { get; set; }
    public User user { get; set; }
    public DateTime dateTime { get; set; }
    public string paymentMethod {  get; set; }
    public int totalPrice { get; set; }
    public string status { get; set; }
    public List<Product> orderProductList { get; set; }

    public Order()
    {
        orderProductList = new List<Product>();
    }
}

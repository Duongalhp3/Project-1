using MySqlConnector;

public abstract class OrderDAL
{
    private static List<string> statusList = new List<string> { "Pending", "Confirmed", "Cancelled" };

    public static Order GetOrderById(int orderId)
    {
        Order order = null;
        int userId = -1;
        string query = @"SELECT * FROM orders 
                         INNER JOIN orderDetails ON orders.order_id = orderDetails.order_id 
                         WHERE orders.order_id = @orderId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@orderId", orderId);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    if (order == null)
                    {
                        order = new Order
                        {
                            id = reader.GetInt32("order_id"),
                            dateTime = reader.GetDateTime("order_dateTime"),
                            paymentMethod = reader.GetString("order_paymentMethod"),
                            totalPrice = reader.GetInt32("order_totalPrice"),
                            status = reader.GetString("order_status")
                        };
                        userId = reader.GetInt32("user_id");
                    }
                    Product product = new Product
                    {
                        id = reader.GetInt32("product_id"),
                        name = reader.GetString("product_name"),
                        price = reader.GetInt32("product_price"),
                        quantity = reader.GetInt32("orderDetail_quantity")
                    };
                    order.orderProductList.Add(product);
                }
            }
        }
        order.user = UserDAL.GetUserById(userId);
        return order;
    }

    public static List<Order> GetAllOrderByUser(User user)
    {
        List<Order> orderList = new List<Order>();
        string query = @"SELECT * FROM orders WHERE user_id = @userId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userId", user.id);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    Order order = new Order
                    {
                        id = reader.GetInt32("order_id")
                    };
                    orderList.Add(order);
                }
            }
        }
        for (int i = 0; i < orderList.Count; i++)
        {
            orderList[i] = OrderDAL.GetOrderById(orderList[i].id);
        }
        return Utility.SortByStatus<Order>(orderList, order => order.status, statusList);
    }

    public static List<Order> GetAllOrder()
    {
        List<Order> orderList = new List<Order>();
        string query = "SELECT * FROM orders;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    Order order = new Order
                    {
                        id = reader.GetInt32("order_id")
                    };
                    orderList.Add(order);
                }
            }
        }
        for (int i = 0; i < orderList.Count; i++)
        {
            orderList[i] = OrderDAL.GetOrderById(orderList[i].id);
        }
        return Utility.SortByStatus<Order>(orderList, order => order.status, statusList);
    }

    public static int SaveOrder(Order order)
    {
        bool canPayment = true;
        foreach (Product orderProduct in order.orderProductList)
        {
            if (!ProductDAL.CheckQuantity(orderProduct))
            {
                Console.WriteLine("Mat hang co id = " + orderProduct.id + "trong gio hang cua ban khong du so luong hoac het hang");
                canPayment = false;
            }
        }
        if (!canPayment) return 0;
        foreach (var product in order.orderProductList)
        {
            order.totalPrice += product.quantity * product.price;
        }
        return DataBase.ExecuteOrderTransaction(order);
    }

    public static int UpdateOrderAttribute(int orderId, string attributeName, object newValue)
    {
        string query = $@"UPDATE orders SET {attributeName} = @newValue WHERE order_id = @orderId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@orderId", orderId);
            return DataBase.ExecuteNonQuery(command);
        }
    }

    public static (int revenue, int orderCount) GetReVenue(int option) // 1 is current day, 2 is current month, 3 is current year
    {
        int revenue = 0;
        int orderCount = 0;
        string query = "";
        if (option == 1)
        {
            query = @"SELECT SUM(order_totalPrice) AS total_price, COUNT(*) AS order_count
                  FROM orders 
                  WHERE DAY(order_dateTime) = DAY(CURDATE()) 
                  AND MONTH(order_dateTime) = MONTH(CURDATE()) 
                  AND YEAR(order_dateTime) = YEAR(CURDATE()) 
                  AND order_status = @orderStatus";
        }
        else if (option == 2)
        {
            query = @"SELECT SUM(order_totalPrice) AS total_price, COUNT(*) AS order_count
                  FROM orders 
                  WHERE MONTH(order_dateTime) = MONTH(CURDATE()) 
                  AND YEAR(order_dateTime) = YEAR(CURDATE()) 
                  AND order_status = @orderStatus";
        }
        else if (option == 3)
        {
            query = @"SELECT SUM(order_totalPrice) AS total_price, COUNT(*) AS order_count
                  FROM orders 
                  WHERE YEAR(order_dateTime) = YEAR(CURDATE()) 
                  AND order_status = @orderStatus";
        }
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@orderStatus", "Confirmed");
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("total_price")))
                    {
                        revenue = reader.GetInt32("total_price");
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal("order_count")))
                    {
                        orderCount = reader.GetInt32("order_count");
                    }
                }
            }
        }
        return (revenue, orderCount);
    }
}

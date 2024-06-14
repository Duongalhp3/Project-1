using MySqlConnector;
using System.Data;

public class DataBase
{
    private static MySqlConnection connection = new MySqlConnection(@"server=localhost;user id=root;password=123456789;port=3306;database=storemanager;");

    public static MySqlDataReader ExecuteQuery(MySqlCommand command)
    {
        connection.Open();
        command.Connection = connection;
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public static int ExecuteNonQuery(MySqlCommand command)
    {
        connection.Open();
        command.Connection = connection;
        int rowsAffected = command.ExecuteNonQuery();
        connection.Close();
        return rowsAffected;
    }

    public static object ExecuteScalar(MySqlCommand command)
    {
        connection.Open();
        command.Connection = connection;
        object value = command.ExecuteScalar();
        connection.Close();
        return value;
    }

    public static int ExecuteOrderTransaction(Order order)
    {
        connection.Open();
        var transaction = connection.BeginTransaction();
        try
        {
            using (var lockTableCommand = new MySqlCommand("LOCK TABLES products WRITE, orders WRITE, orderDetails WRITE", connection, transaction))
            {
                lockTableCommand.ExecuteNonQuery();
            }
            int orderId;
            using (var orderCommand = new MySqlCommand(@"INSERT INTO orders (user_id, order_paymentMethod, order_totalPrice) VALUES (@userId, @paymentMethod, @totalPrice); SELECT LAST_INSERT_ID();", connection, transaction))
            {
                orderCommand.Parameters.AddWithValue("@userId", order.user.id);
                orderCommand.Parameters.AddWithValue("@paymentMethod", order.paymentMethod);
                orderCommand.Parameters.AddWithValue("@totalPrice", order.totalPrice);
                orderId = Convert.ToInt32(orderCommand.ExecuteScalar());
            }
            foreach (Product product in order.orderProductList)
            {
                using (var orderDetailCommand = new MySqlCommand(@"INSERT INTO orderDetails (order_id, product_id, product_name, product_price, orderdetail_quantity) VALUES (@orderId, @productId, @productName, @productPrice, @quantity);", connection, transaction))
                {
                    orderDetailCommand.Parameters.AddWithValue("@orderId", orderId);
                    orderDetailCommand.Parameters.AddWithValue("@productId", product.id);
                    orderDetailCommand.Parameters.AddWithValue("@productName", product.name);
                    orderDetailCommand.Parameters.AddWithValue("@productPrice", product.price);
                    orderDetailCommand.Parameters.AddWithValue("@quantity", product.quantity);
                    orderDetailCommand.ExecuteNonQuery();
                }
                using (var productCommand = new MySqlCommand(@"UPDATE products SET product_quantity = product_quantity - @newQuantity WHERE product_id = @productId;", connection, transaction))
                {
                    productCommand.Parameters.AddWithValue("@productId", product.id);
                    productCommand.Parameters.AddWithValue("@newQuantity", product.quantity);
                    productCommand.ExecuteNonQuery();
                }
            }
            transaction.Commit();
            return orderId;
        }
        catch (Exception ex)
        {
            /*Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine("Stack Trace: " + ex.StackTrace);*/
            transaction.Rollback();
            return 0;
        }
        finally
        {
            using (var unlockTableCommand = new MySqlCommand("UNLOCK TABLES;", connection))
            {
                unlockTableCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}

using MySqlConnector;
using System.ComponentModel;

public abstract class ProductDAL
{
    public static Product GetProductById(int id)
    {
        string query = @"SELECT * FROM products WHERE product_id = @productId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@productId", id);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                reader.Read();
                Product product = new Product
                {
                    id = reader.GetInt32("product_id"),
                    name = reader.GetString("product_name"),
                    price = reader.GetInt32("product_price"),
                    quantity = reader.GetInt32("product_quantity"),
                    status = reader.GetString("product_status")
                };
                return product;
            }
        }
    }

    public static List<Product> GetProductByName(string name)
    {
        List<Product> productList = new List<Product>();
        string query = @"SELECT * FROM products WHERE product_name LIKE @productName;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@productName", "%" + name + "%");
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        id = reader.GetInt32("product_id"),
                        name = reader.GetString("product_name"),
                        price = reader.GetInt32("product_price"),
                        quantity = reader.GetInt32("product_quantity"),
                        status = reader.GetString("product_status")
                    };
                    productList.Add(product);
                }
            }
        }
        return productList;
    }

    public static List<Product> GetAllProduct()
    {
        List<Product> productList = new List<Product>();
        string query = @"SELECT * FROM products;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        id = reader.GetInt32("product_id"),
                        name = reader.GetString("product_name"),
                        price = reader.GetInt32("product_price"),
                        quantity = reader.GetInt32("product_quantity"),
                        status = reader.GetString("product_status")
                    };
                    productList.Add(product);
                }
            }
        }
        return productList;
    }

    public static int SaveProduct(Product product)
    {
        string query = @"INSERT INTO products (product_name, product_price, product_quantity) VALUES (@name, @price, @quantity);";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@name", product.name);
            command.Parameters.AddWithValue("@price", product.price);
            command.Parameters.AddWithValue("@quantity", product.quantity);
            return DataBase.ExecuteNonQuery(command);
        }
    }

    public static bool CheckQuantity(Product orderProduct)
    {
        Product product = ProductDAL.GetProductById(orderProduct.id);
        return product.quantity >= orderProduct.quantity;
    }

    public static int UpdateProductAttribute(int productId, string attributeName, object newValue)
    {
        List<MySqlCommand> commands = new List<MySqlCommand>();
        string query = $@"UPDATE products SET {attributeName} = @newValue WHERE product_id = @productId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@productId", productId);
            return DataBase.ExecuteNonQuery(command);
        }
    }
}

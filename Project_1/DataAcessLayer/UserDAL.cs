using MySqlConnector;

public abstract class UserDAL
{
    private static List<string> statusList = new List<string> { "Active", "Lock"};

    public static User GetUserById(int id)
    {
        string query = @"SELECT * FROM users WHERE user_id = @userId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userId", id);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                reader.Read();
                User user = new User
                {
                    id = reader.GetInt32("user_id"),
                    userName = reader.GetString("user_name"),
                    password = reader.GetString("user_password"),
                    address = reader.GetString("user_address"),
                    phoneNumber = reader.GetString("user_phoneNumber"),
                    role = reader.GetString("user_role"),
                    status = reader.GetString("user_status")
                };
                return user;
            }
        }
    }

    public static List<User> GetUserByName(string name)
    {
        List<User> userList = new List<User>();
        string query = @"SELECT * FROM users WHERE user_name LIKE @userName;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userName", "%" + name + "%");
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    User user = new User
                    {
                        id = reader.GetInt32("user_id"),
                        userName = reader.GetString("user_name"),
                        password = reader.GetString("user_password"),
                        address = reader.GetString("user_address"),
                        phoneNumber = reader.GetString("user_phoneNumber"),
                        role = reader.GetString("user_role"),
                        status = reader.GetString("user_status")
                    };
                    userList.Add(user);
                }
            }
        }
        return Utility.SortByStatus<User>(userList, user => user.status, statusList);
    }

    public static List<User> GetAllUser()
    {
        List<User> userList = new List<User>();
        string query = @"SELECT * FROM users;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    User user = new User
                    {
                        id = reader.GetInt32("user_id"),
                        userName = reader.GetString("user_name"),
                        password = reader.GetString("user_password"),
                        address = reader.GetString("user_address"),
                        phoneNumber = reader.GetString("user_phoneNumber"),
                        role = reader.GetString("user_role"),
                        status = reader.GetString("user_status")
                    };
                    userList.Add(user);
                }
            }
        }
        return Utility.SortByStatus<User>(userList, user => user.status, statusList);
    }

    public static int SaveUser(User user)
    {
        string query = @"INSERT INTO users (user_name, user_password, user_address, user_phoneNumber, user_role) VALUES (@userName, @password, @address, @phoneNumber, @role);";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userName", user.userName);
            command.Parameters.AddWithValue("@password", user.password);
            command.Parameters.AddWithValue("@address", user.address);
            command.Parameters.AddWithValue("@phoneNumber", user.phoneNumber);
            command.Parameters.AddWithValue("@role", user.role);
            return DataBase.ExecuteNonQuery(command);
        }
    }

    public static bool CheckUserName(string userName)
    {
        string query = @"SELECT * FROM users WHERE user_name = @userName;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userName", userName);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                return reader.HasRows ? false : true;
            }
        }
    }

    public static User VerifyUser(string userName, string password)
    {
        string query = @"SELECT * FROM users WHERE user_name = @userName AND user_password = @password;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@password", password);
            using (MySqlDataReader reader = DataBase.ExecuteQuery(command))
            {
                if (!reader.HasRows) return null;
                reader.Read();
                User user = new User
                {
                    id = reader.GetInt32("user_id"),
                    userName = reader.GetString("user_name"),
                    password = reader.GetString("user_password"),
                    address = reader.GetString("user_address"),
                    phoneNumber = reader.GetString("user_phoneNumber"),
                    role = reader.GetString("user_role"),
                    status = reader.GetString("user_status")
                };
                return user;
            }
        }
    }

    public static int UpdateUserAttribute(int userId, string attributeName, object newValue)
    {
        string query = $@"UPDATE users SET {attributeName} = @newValue WHERE user_id = @userId;";
        using (MySqlCommand command = new MySqlCommand(query))
        {
            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@userId", userId);
            return DataBase.ExecuteNonQuery(command);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("                Cong Nghe - Chuyen Nghiep - Tien Loi");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Press 1 to login");
            Console.WriteLine("Press 2 to register");
            Console.WriteLine("Press 0 to exit program");
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                if (pressedKey.KeyChar == '0')
                {
                    return;
                }
                else if (pressedKey.KeyChar == '1')
                {
                    User user = AuthBL.Login();
                    if (user != null && user.role == "Customer")
                    {
                        Order order = new Order
                        {
                            user = user,
                        };
                        CustomerBL.IsLogin(order);
                    }
                    else if (user != null && user.role == "Admin")
                    {
                        AdminBL.IsLogin(user);
                    }
                    else if(user != null && user.role == "Employee")
                    {
                        EmployeeBL.IsLogin(user);
                    }
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    Console.Clear();
                    Utility.PrintTitle("Caffe Store");
                    Console.WriteLine("----------------------------------Register---------------------------------");
                    AuthBL.Register("Customer");
                    break;
                }
            }
        }
    }
}
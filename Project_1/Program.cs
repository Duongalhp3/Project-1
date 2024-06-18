using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            AnsiConsole.MarkupLine("----------------------------------------------------------------------------");
            Console.WriteLine("Press 1 to Login");
            Console.WriteLine("Press 2 to Register");
            Console.WriteLine("Press 0 to Exit Program");
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
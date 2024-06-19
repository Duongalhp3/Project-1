using Spectre.Console;

public abstract class AuthBL
{
    public static User Login()
    {
        User user;
        do
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("-----------------------------------Login-----------------------------------");
            Console.Write("-> Enter User Name: ");
            string userName = Utility.CheckInput((Console.ReadLine()));
            Console.Write("-> Enter Password: ");
            string password = Utility.ReadPassword();
            user = UserDAL.VerifyUser(userName, password);
            if(user == null)
            {
                AnsiConsole.MarkupLine("[Red]Incorrect username or password ![/]");
                Console.Write("Do you want to login again. Press [y/n] ");
                while (true)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                    if (pressedKey.KeyChar == 'n' || pressedKey.KeyChar == 'N')
                    {
                        return null;
                    }
                    else if (pressedKey.KeyChar == 'y' || pressedKey.KeyChar == 'Y')
                    {
                        break;
                    }
                }
            }
            else if(user.status == "Lock")
            {
                AnsiConsole.MarkupLine("[Red]Your account has been locked ![/]");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                return null;
            }
        } while (user == null);
        Console.WriteLine("System is loging...");
        Thread.Sleep(1000);
        return user;
    }

    public static void Register(string role)
    {
        string userName;
        bool canRegister;
        do
        {
            Console.Write("-> Enter User Name: ");
            userName = Utility.CheckInput(Console.ReadLine());
            canRegister = UserDAL.CheckUserName(userName);
            if(!canRegister)
            {
                AnsiConsole.MarkupLine("[Red]Username already exists ![/]");
                Console.Write("Do you want to enter again? Press [y/n] ");
                while (true)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                    if (pressedKey.KeyChar == 'n' || pressedKey.KeyChar == 'N')
                    {
                        return;
                    }
                    else if (pressedKey.KeyChar == 'y' || pressedKey.KeyChar == 'Y')
                    {
                        break;
                    }
                }
            }
        } while (!canRegister);
        Console.Write("-> Enter Password: ");
        string password = Utility.CheckInput(Console.ReadLine());
        Console.Write("-> Enter Address: ");
        string address = Utility.CheckInput(Console.ReadLine());
        Console.Write("-> Enter Phone Number: ");
        string phoneNumber = Utility.CheckInput(Console.ReadLine());
        User user = new User
        {
            userName = userName,
            password = password,
            address = address,
            phoneNumber = phoneNumber,
            role = role
        };
        if (UserDAL.SaveUser(user) != 0)
        {
            Console.WriteLine("Add successfully ");
        }
        else AnsiConsole.MarkupLine("[Red]Add failed ![/]");
        Console.Write("-> Press any key to go back");
        Console.ReadLine();
    }
}

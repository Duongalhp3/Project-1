using Spectre.Console;

public abstract class AdminBL
{
    public static void IsLogin(User user)
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("User Management");
            List<User> userList = UserDAL.GetAllUser();
            userList.RemoveAll(x => x.id == 1);
            int pageCount = (int)Math.Ceiling((double)userList.Count / pageSize);
            List<User> users = userList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (users.Count == pageSize)
            {
                Utility.PrintUserTable(users);
            }
            else
            {
                Utility.PrintUserTable(users);
                for (int i = 0; i < pageSize - users.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Add Employee Account");
            Console.WriteLine("Press 2 to Search User");
            Console.WriteLine("Press 3 to Edit Account");
            Console.WriteLine("Press 4 to Change Password");
            Console.WriteLine("Press 0 to Logout");
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                if (pressedKey.Key == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage -= 1;
                    break;
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow && currentPage < pageCount)
                {
                    currentPage += 1;
                    break;
                }
                else if (pressedKey.KeyChar == '0')
                {
                    return;
                }
                else if (pressedKey.KeyChar == '1')
                {
                    AddEmployeeAccount();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    SearchUser();
                    break;
                }
                else if (pressedKey.KeyChar == '3')
                {
                    EditAccount();
                    break;
                }
                else if (pressedKey.KeyChar == '4')
                {
                    Utility.ChangePassword(user);
                    break;
                }
            }
        }
    }

    public static void AddEmployeeAccount()
    {
        Console.Clear();
        Utility.PrintTitle("User Management");
        Console.WriteLine("----------------------------Add Employee Account---------------------------");
        AuthBL.Register("Employee");
    }

    public static void SearchUser()
    {
        Console.Clear();
        Utility.PrintTitle("User Management");
        Console.WriteLine("--------------------------------Search User--------------------------------");
        Console.Write("-> Enter user name to search: ");
        string input = Utility.CheckInput(Console.ReadLine());
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            List<User> searchList = UserDAL.GetUserByName((input));
            searchList.RemoveAll(x => x.id == 1);
            if (searchList == null || searchList.Count == 0)
            {
                AnsiConsole.MarkupLine("[Red]The user not found ![/]");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                return;
            }
            int pageCount = (int)Math.Ceiling((double)searchList.Count / pageSize);
            List<User> users = searchList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (users.Count == pageSize)
            {
                Utility.PrintUserTable(users);
            }
            else
            {
                Utility.PrintUserTable(users);
                for (int i = 0; i < pageSize - users.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Edit Account");
            Console.WriteLine("Press 0 to Exit");
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                if (pressedKey.Key == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage -= 1;
                    break;
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow && currentPage < pageCount)
                {
                    currentPage += 1;
                    break;
                }
                else if (pressedKey.KeyChar == '0')
                {
                    return;
                }
                else if (pressedKey.KeyChar == '1')
                {
                    EditAccount();
                    return;
                }
            }
            Console.SetCursorPosition(0, Console.CursorTop - 19);
            for (int i = 0; i < 20; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop - 19);
        }
    }

    public static void EditAccount()
    {
        Console.Clear();
        Utility.PrintTitle("User Management");
        Console.WriteLine("--------------------------------Edit Account-------------------------------");
        Console.Write("-> Enter user id to edit: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        User user = UserDAL.GetUserById(id);
        if (user == null || user.id == 1)
        {
            AnsiConsole.MarkupLine("[Red]Invalid id ![/]");
            Console.Write("Press any key to go back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Press 1 to Lock Account");
        Console.WriteLine("Press 2 to Unlock Account");
        Console.WriteLine("Press 0 to Exit");
        Console.WriteLine("---------------------------------------");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                return;
            }
            else if (pressedKey.KeyChar == '1')
            {
                if (user.status == "Lock")
                {
                    AnsiConsole.MarkupLine("[Red]This account has been locked ![/]");
                }
                else if (UserDAL.UpdateUserAttribute(id, "user_status", "Lock") != 0)
                {
                    Console.WriteLine("Lock successful");
                }
                else AnsiConsole.MarkupLine("[Red]Lock failed ![/]");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                return;
            }
            else if (pressedKey.KeyChar == '2')
            {
                if (user.status == "Active")
                {
                    AnsiConsole.MarkupLine("[Red]This account has been unlocked ![/]");
                }
                else if (UserDAL.UpdateUserAttribute(user.id, "user_status", "Active") != 0)
                {
                    Console.WriteLine("Unlocked successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Unlocking failed ![/]");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                return;
            }
        }
    }
}

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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
            Console.WriteLine("Nhan 1 de them tai khoan nhan vien");
            Console.WriteLine("Nhan 2 de tim kiem nguoi dung");
            Console.WriteLine("Nhan 3 de edit tai khoan");
            Console.WriteLine("Nhan 4 de thay doi mat khau");
            Console.WriteLine("Nhan 0 de dang xuat");
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
        Console.Write("Nhap Ten San Pham Can Tim: ");
        string input = Utility.CheckInput(Console.ReadLine());
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            List<User> searchList = UserDAL.GetUserByName((input));
            searchList.RemoveAll(x => x.id == 1);
            if (searchList == null || searchList.Count == 0)
            {
                Console.WriteLine("Khong tim thay nguoi dung can tim");
                Console.Write("Press any button to back");
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
            Console.WriteLine("Nhan 1 de edit account");
            Console.WriteLine("Nhan 0 de tro lai");
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
        Console.Write("Nhap id user muon edit: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        User user = UserDAL.GetUserById(id);
        if (user == null || user.id == 1)
        {
            Console.Write("Id khong hop le");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Nhan 1 de khoa tai khoan");
        Console.WriteLine("Nhan 2 de mo tai khoan");
        Console.WriteLine("Nhan 0 de Thoat");
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
                    Console.WriteLine("Tai khoan nay da bi khoa");
                }
                else if (UserDAL.UpdateUserAttribute(id, "user_status", "Lock") != 0)
                {
                    Console.WriteLine("Khoa thanh cong");
                }
                else Console.WriteLine("Khoa that bai");
                Console.Write("Press any button to back");
                Console.ReadLine();
                return;
            }
            else if (pressedKey.KeyChar == '2')
            {
                if (user.status == "Active")
                {
                    Console.WriteLine("Tai khoan nay dang ative");
                }
                else if (UserDAL.UpdateUserAttribute(user.id, "user_status", "Active") != 0)
                {
                    Console.WriteLine("Mo tai khoan thanh cong");
                }
                else Console.WriteLine("Mo tai khoan that bai");
                Console.Write("Press any button to back");
                Console.ReadLine();
                return;
            }
        }
    }
}

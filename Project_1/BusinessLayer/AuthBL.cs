public abstract class AuthBL
{
    public static User Login()
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("-----------------------------------Login-----------------------------------");
        User user;
        do
        {
            Console.Write("-> Enter User Name: ");
            string userName = Utility.CheckInput((Console.ReadLine()));
            Console.Write("-> Enter Password: ");
            string password = Utility.ReadPassword();
            user = UserDAL.VerifyUser(userName, password);
            if(user == null)
            {
                Console.WriteLine("Ten dang nhap hoac mat khau sai !");
            }
            else if(user.status == "Lock")
            {
                Console.WriteLine("Tai khoan cua ban da bi khoa");
                Console.Write("-> Press any key to come back");
                Console.ReadLine();
                return null;
            }
        } while (user == null);
        Console.WriteLine("System is loging...");
        Thread.Sleep(2000);
        return user;
    }

    public static void Register(string role)
    {
        Console.Write("-> Enter User Name: ");
        string userName;
        bool canRegister;
        do
        {
            userName = Utility.CheckInput(Console.ReadLine());
            canRegister = UserDAL.CheckUserName(userName);
            if(!canRegister)
            {
                Console.Write("Ten Dang Nhap Da Ton Tai, Xin Moi Nhap Lai: ");
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
            Console.WriteLine("Them Thanh cong ");
        }
        else Console.WriteLine("Them that bai");
        Console.Write("-> Press any key to come back");
        Console.ReadLine();
    }
}

using Spectre.Console;
using System.Globalization;
using System.Text;

public abstract class Utility
{
    public static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(intercept: true);
            if (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password.ToString();
    }

    public static string CheckInput(string input)
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.Write("Khong duoc de trong, vui long nhap lai: ");
            input = Console.ReadLine();
        }
        return input;
    }

    public static int CheckDigitalInput(string input)
    {
        int result;
        while (true)
        {
            if (Int32.TryParse(input, out result) && result >= 0)
            {
                result = Int32.Parse(input);
                break;
            }
            else
            {
                Console.Write("Invalid input please re_input: ");
                input = Console.ReadLine();
            }
        }
        return result;
    }

    public static void PrintOrderTable(List<Order> ordertList, int option)// 1: table for customer, 2: table for admin
    {
        if (option == 1)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[blue]Id[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Date Time[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Total Price[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Status[/]").LeftAligned());
            foreach (Order order in ordertList)
            {
                table.AddRow(order.id.ToString(), order.dateTime.ToString(), ConvertToCurrency(order.totalPrice), order.status);
            }
            table.Border(TableBorder.Rounded);
            table.Title = new TableTitle("[bold yellow]Order Purchased List[/]");
            AnsiConsole.Write(table);
        }
        else if (option == 2)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[blue]Id[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Customer Name[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Date Time[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Total Price[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Status[/]").LeftAligned());
            foreach (Order order in ordertList)
            {
                table.AddRow(order.id.ToString(), order.user.userName, order.dateTime.ToString(), ConvertToCurrency(order.totalPrice), order.status);
            }
            table.Border(TableBorder.Rounded);
            table.Title = new TableTitle("[bold yellow]Order List[/]");
            AnsiConsole.Write(table);
        }
    }

    public static void PrintProductTable(List<Product> productList,int option)// 1: table for product in menu, 2: table for product in cart
    {
        if(option == 1)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[blue]Id[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Name[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Price[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Quantity[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Status[/]").LeftAligned());
            foreach (Product product in productList)
            {
                table.AddRow(product.id.ToString(), product.name, ConvertToCurrency(product.price), product.quantity.ToString(), product.status);
            }
            table.Border(TableBorder.Rounded);
            table.Title = new TableTitle("[bold yellow]Product List[/]");
            AnsiConsole.Write(table);
        }
        else if (option == 2)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[blue]Id[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Name[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Quantity[/]").LeftAligned());
            table.AddColumn(new TableColumn("[blue]Total Price[/]").LeftAligned());
            foreach (Product product in productList)
            {
                table.AddRow(product.id.ToString(), product.name, product.quantity.ToString(), ConvertToCurrency(product.quantity * product.price));
            }
            table.Border(TableBorder.Rounded);
            table.Title = new TableTitle("[bold yellow]Product List In Cart[/]");
            AnsiConsole.Write(table);
        }
    }

    public static void PrintUserTable(List<User> userList)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("[blue]Id[/]").Centered());
        table.AddColumn(new TableColumn("[blue]User Name[/]").LeftAligned());
        table.AddColumn(new TableColumn("[blue]Phone Number[/]").LeftAligned());
        table.AddColumn(new TableColumn("[blue]Address[/]").LeftAligned());
        table.AddColumn(new TableColumn("[blue]Role[/]").LeftAligned());
        table.AddColumn(new TableColumn("[blue]Status[/]").LeftAligned());
        foreach (User user in userList)
        {
            table.AddRow(user.id.ToString(), user.userName, user.phoneNumber, user.address, user.role, user.status);
        }
        table.Border(TableBorder.Rounded);
        table.Title = new TableTitle("[bold yellow]User List[/]");
        AnsiConsole.Write(table);
    }

    public static void PrintTitle(string title)
    {
        AnsiConsole.Write(new FigletText(title).LeftJustified().Color(Color.SteelBlue));
    }

    public static void PrintOrderDetailTable(Order order)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("[yellow]Order Detail[/]").Centered());
        var leftAlignedTable = new Table().HideHeaders().NoBorder().AddColumn(new TableColumn(""));
        leftAlignedTable.AddRow("User Name: [blue]" + order.user.userName + "[/]");
        leftAlignedTable.AddRow("Date Time: [blue]" + order.dateTime + "[/]");
        leftAlignedTable.AddRow("Phone Number: [blue]" + order.user.phoneNumber + "[/]");
        leftAlignedTable.AddRow("Address : [blue]" + order.user.address + "[/]");
        leftAlignedTable.AddRow("Payment Method: [blue]" + order.paymentMethod + "[/]");
        leftAlignedTable.AddRow("Status: [blue]" + order.status + "[/]");
        table.AddRow(leftAlignedTable);
        var detailTable = new Table().Title("Product List");
        detailTable.AddColumn(new TableColumn("Product Name"));
        detailTable.AddColumn(new TableColumn("Product Price"));
        detailTable.AddColumn(new TableColumn("Order Quantity"));
        detailTable.AddColumn(new TableColumn("Total Price"));
        foreach (Product orderProduct in order.orderProductList)
        {
            detailTable.AddRow(orderProduct.name, ConvertToCurrency(orderProduct.price), orderProduct.quantity.ToString(), ConvertToCurrency(orderProduct.price * orderProduct.quantity));
        }
        table.AddRow(detailTable);
        var rightAlignedTable = new Table().HideHeaders().NoBorder().AddColumn(new TableColumn(""));
        rightAlignedTable.AddRow("[bold red]Total Price: "+ ConvertToCurrency(order.totalPrice) +"[/]").RightAligned();
        table.AddRow(rightAlignedTable);
        AnsiConsole.Write(table);
    }

    public static string ConvertToCurrency(int input)
    {
        CultureInfo vietnamCulture = new CultureInfo("vi-VN");
        return string.Format(vietnamCulture, "{0:N0} VND", input);
    }

    public static void ChangePassword(User user)
    {
        string oldPassword, newPassword;
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("------------------------------Change Password------------------------------");
        Console.Write("Nhap mat khau cu: ");
        do
        {
            oldPassword = Utility.CheckInput(Utility.ReadPassword());
            if (user.password != oldPassword)
            {
                Console.Write("Mat khau khong dung, xin moi nhap lai: ");
            }
        } while (user.password != oldPassword);
        Console.Write("Nhap mat khau moi: ");
        newPassword = Utility.CheckInput(Utility.ReadPassword());
        if (UserDAL.UpdateUserAttribute(user.id, "user_password", newPassword) != 0)
        {
            Console.WriteLine("Thay doi mat khau thanh cong");
        }
        else Console.WriteLine(" Thay doi mat khau that bai");
        Console.Write("Press any button to back");
        Console.ReadLine();
    }

    public static void PrintRevenueTable(int revenueDay, int orderCountDay, int revenueMonth, int orderCountMonth, int revenueYear, int orderCountYear)
    {
        var table = new Table();
        table.Title("Bang thong ke doanh so");
        table.AddColumn("Thong Ke").LeftAligned();
        table.AddColumn("So Tien/So Luong").Centered();
        table.AddRow("So Luong Don Trong Ngay: \nDoanh Thu Trong Ngay: ",orderCountDay + "\n" + Utility.ConvertToCurrency(revenueDay));
        table.AddEmptyRow();
        table.AddRow("So Luong Don Trong Thang: \nDoanh Thu Trong thang: ", orderCountMonth + "\n" + Utility.ConvertToCurrency(revenueMonth));
        table.AddEmptyRow();
        table.AddRow("So Luong Don Trong nam: \nDoanh Thu Trong nam: ", orderCountYear + "\n" + Utility.ConvertToCurrency(revenueYear));
        AnsiConsole.Write(table);
    }
}
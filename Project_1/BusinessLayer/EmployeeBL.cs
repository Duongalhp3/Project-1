using Spectre.Console;

public abstract class EmployeeBL
{
    public static void IsLogin(User user)
    {
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Management");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Press 1 to Product Management");
            Console.WriteLine("Press 2 to Order Management");
            Console.WriteLine("Press 3 to View Revenue");
            Console.WriteLine("Press 4 to Change Password");
            Console.WriteLine("Press 0 to Logout");
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                if (pressedKey.KeyChar == '0')
                {
                    return;
                }
                else if (pressedKey.KeyChar == '1')
                {
                    ProductManagement();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    OrderManagement();
                    break;
                }
                else if (pressedKey.KeyChar == '3')
                {
                    ViewRevenue();
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

    public static void ProductManagement()
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Product Management");
            List<Product> producList = ProductDAL.GetAllProduct();
            int pageCount = (int)Math.Ceiling((double)producList.Count / pageSize);
            List<Product> products = producList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (products.Count == pageSize)
            {
                Utility.PrintProductTable(products, 1);
            }
            else
            {
                Utility.PrintProductTable(products, 1);
                for (int i = 0; i < pageSize - products.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press 1 to Add New Product");
            Console.WriteLine("Press 2 to Edit Product");
            Console.WriteLine("Press 3 to Search Product");
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
                    AddProduct();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    EditProduct();
                    break;
                }
                else if (pressedKey.KeyChar == '3')
                {
                    SearchProduct();
                    break;
                }
            }
        }
    }

    public static void OrderManagement()
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Order Management");
            List<Order> orderList = OrderDAL.GetAllOrder();
            int pageCount = (int)Math.Ceiling((double)orderList.Count / pageSize);
            List<Order> orders = orderList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (orderList.Count == pageSize)
            {
                Utility.PrintOrderTable(orderList, 2);
            }
            else
            {
                Utility.PrintOrderTable(orderList, 2);
                for (int i = 0; i < pageSize - orderList.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Processing Order");
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
                    ProcessingOrder();
                    break;
                }
            }
        }
    }


    private static void SearchProduct()
    {
        Console.Clear();
        Utility.PrintTitle("Product Management");
        Console.WriteLine("------------------------------Search Product-------------------------------");
        Console.Write("-> Enter product name to search: ");
        string input = Utility.CheckInput(Console.ReadLine());
        List<Product> searchList = ProductDAL.GetProductByName(input);
        if (searchList == null)
        {
            AnsiConsole.MarkupLine("[Red]The product not found ![/]");
            Console.Write("-> Press any key to come back");
            Console.ReadLine();
            return;
        }
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            int pageCount = (int)Math.Ceiling((double)searchList.Count / pageSize);
            List<Product> products = searchList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (products.Count == pageSize)
            {
                Utility.PrintProductTable(products, 1);
            }
            else
            {
                Utility.PrintProductTable(products, 1);
                for (int i = 0; i < pageSize - products.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Edit Product");
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
                    EditProduct();
                    return;
                }
            }
            Console.SetCursorPosition(0, Console.CursorTop - 20);
            for (int i = 0; i < 21; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop - 20);
        }
    }

    public static void AddProduct()
    {
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Product Management");
            Console.WriteLine("------------------------------Add Product-------------------------------");
            Product product = new Product();
            Console.Write("-> Enter product name: ");
            product.name = Utility.CheckInput(Console.ReadLine());
            Console.Write("-> Enter product price: ");
            string priceInput = Utility.CheckInput(Console.ReadLine());
            product.price = Utility.CheckDigitalInput(priceInput);
            Console.Write("-> Enter product quantity: ");
            string quantityInput = Utility.CheckInput(Console.ReadLine());
            product.quantity = Utility.CheckDigitalInput(quantityInput);
            if (ProductDAL.SaveProduct(product) != 0)
            {
                Console.WriteLine("Add successfully");
            }
            else AnsiConsole.MarkupLine("[Red]Add failed ![/]");
            Console.Write("-> Do you want to add more products. Press [y/n] ");
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
    }

    public static void EditProduct()
    {
        Console.Clear();
        Utility.PrintTitle("Product Management");
        Console.WriteLine("------------------------------Edit Product-------------------------------");
        Console.Write("-> Enter product id to edit: ");
        string input_ID = Console.ReadLine();
        Product product = ProductDAL.GetProductById(Utility.CheckDigitalInput(input_ID));
        if (product == null)
        {
            AnsiConsole.MarkupLine("[Red]The product not found ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Press 1 to Edit Name");
        Console.WriteLine("Press 2 to Edit Price");
        Console.WriteLine("Press 3 to Edit Quantity");
        Console.WriteLine("Press 4 to Stop Selling");
        Console.WriteLine("Press 5 to Continue Selling");
        Console.WriteLine("Press 0 to Exit");
        Console.WriteLine("-------------------------------");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                break;
            }
            else if (pressedKey.KeyChar == '1')
            {
                Console.Write("-> Enter new product name: ");
                product.name = Utility.CheckInput(Console.ReadLine());
                if (ProductDAL.UpdateProductAttribute(product.id, "product_name", product.name) != 0)
                {
                    Console.WriteLine("Edit successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Edit failed ![/]");
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                Console.Write("-> Enter new price: ");
                string priceInput = Utility.CheckInput(Console.ReadLine());
                product.price = Utility.CheckDigitalInput(priceInput);
                if (ProductDAL.UpdateProductAttribute(product.id, "product_price", product.price) != 0)
                {
                    Console.WriteLine("Edit successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Edit failed ![/]");
                break;
            }
            else if (pressedKey.KeyChar == '3')
            {
                Console.Write("-> Enter new quantity: ");
                string quantityInput = Utility.CheckInput(Console.ReadLine());
                product.quantity = Utility.CheckDigitalInput(quantityInput);
                if (ProductDAL.UpdateProductAttribute(product.id, "product_quantity", product.quantity) != 0)
                {
                    Console.WriteLine("Edit successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Edit failed ![/]");
                break;
            }
            else if (pressedKey.KeyChar == '4')
            {
                if (ProductDAL.UpdateProductAttribute(product.id, "product_status", "Stop Selling") != 0)
                {
                    Console.WriteLine("Edit successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Edit failed ![/]");
                break;
            }
            else if (pressedKey.KeyChar == '5')
            {
                if (product.quantity == 0)
                {
                    product.status = "Out Of Stock";
                }
                else product.status = "In Stock";
                if (ProductDAL.UpdateProductAttribute(product.id, "product_status", product.status) != 0)
                {
                    Console.WriteLine("Edit successfully");
                }
                else AnsiConsole.MarkupLine("[Red]Edit failed ![/]");
                break;
            }
        }
        Console.Write("-> Press any key to go back");
        Console.ReadKey();
    }

    public static void ProcessingOrder()
    {
        Console.Clear();
        Utility.PrintTitle("Order Management");
        Console.WriteLine("------------------------------Processing Order-------------------------------");
        Console.Write("-> Enter order id to process: ");
        string inputID = Utility.CheckInput(Console.ReadLine());
        Order order = OrderDAL.GetOrderById(Utility.CheckDigitalInput(inputID));
        if (order == null)
        {
            AnsiConsole.MarkupLine("[Red]The order not found ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return;
        }
        Utility.PrintOrderDetailTable(order);
        Console.WriteLine("Press 1 to Confirm Order");
        Console.WriteLine("Press 2 to Cancel Order");
        Console.WriteLine("Press 0 to Exit");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                return;
            }
            else if (pressedKey.KeyChar == '1')
            {
                if (order.status == "Cancelled")
                {
                    AnsiConsole.MarkupLine("[Red]This order has been cancelled, can not confirm ![/]");
                    Console.Write("-> Press any key to go back");
                    Console.ReadKey();
                    return;
                }
                else if (order.status == "Confirmed")
                {
                    AnsiConsole.MarkupLine("[Red]This order has been confirmed ![/]");
                    Console.Write("-> Press any key to go back");
                    Console.ReadKey();
                    return;
                }
                order.status = "Confirmed";
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                if (order.status == "Cancelled")
                {
                    AnsiConsole.MarkupLine("[Red]This order has been cancelled ![/]");
                    Console.Write("-> Press any key to come back");
                    Console.ReadKey();
                    return;
                }
                else if (order.status == "Confirmed")
                {
                    AnsiConsole.MarkupLine("[Red]This order has been confirmed, can not cancel ![/]");
                    Console.Write("-> Press any key to come back");
                    Console.ReadKey();
                    return;
                }
                order.status = "Cancelled";
                break;
            }
        }
        if (OrderDAL.UpdateOrderAttribute(order.id, "order_status", order.status) != 0)
        {
            Console.WriteLine("Processing successfully");
        }
        else AnsiConsole.MarkupLine("[Red]Processing failed ![/]");
        Console.Write("-> Press any key to go back");
        Console.ReadKey();
    }

    public static void ViewRevenue()
    {
        Console.Clear();
        Utility.PrintTitle("View Revenue");
        Console.WriteLine("-------------------------------View Revenue--------------------------------");
        var (revenueDay, orderCountDay) = OrderDAL.GetReVenue(1);
        var (revenueMonth, orderCountMonth) = OrderDAL.GetReVenue(2);
        var (revenueYear, orderCountYear) = OrderDAL.GetReVenue(3);
        Utility.PrintRevenueTable(revenueDay, orderCountDay, revenueMonth, orderCountMonth, revenueYear, orderCountYear);
        Console.Write("-> Press any key to go back");
        Console.ReadKey();
        Console.Clear();
    }
}
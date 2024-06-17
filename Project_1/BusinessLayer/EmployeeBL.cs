﻿public abstract class EmployeeBL
{
    public static void IsLogin(User user)
    {
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Management System");
            Console.WriteLine("Nhan 1 de Product Management");
            Console.WriteLine("Nhan 2 de Order Management");
            Console.WriteLine("nhan 3 de View Revenue");
            Console.WriteLine("Nhan 4 de thay doi mat khau");
            Console.WriteLine("nhan 0 de Logout");
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("nhan 1 de Add product");
            Console.WriteLine("Nhan 2 de Update Product");
            Console.WriteLine("Nhan 3 de Remove product");
            Console.WriteLine("Nhan 4 de Search product");
            Console.WriteLine("Nhan 0 de Exit");
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
                    RemoveProduct();
                    break;
                }
                else if (pressedKey.KeyChar == '4')
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
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
        Console.Write("-> Enter product name want search: ");
        string input = Utility.CheckInput(Console.ReadLine());
        List<Product> searchList = ProductDAL.GetProductByName(input);
        if (searchList == null)
        {
            Console.WriteLine("Khong tim thay san pham can tim");
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
            Console.WriteLine("Press 1 to Update Product");
            Console.WriteLine("Press 2 to Remove product");
            Console.WriteLine("Press 0 to Back");
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
                else if (pressedKey.KeyChar == '2')
                {
                    RemoveProduct();
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
                Console.WriteLine("Adding Successfully");
            }
            else Console.WriteLine("can not add");
            Console.Write("-> Do you want to add more products? Press [y/n] ");
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
        Console.Write("-> Enter ID product want edit: ");
        string input_ID = Console.ReadLine();
        Product product = ProductDAL.GetProductById(Utility.CheckDigitalInput(input_ID));
        if (product == null)
        {
            Console.WriteLine("cannot found");
            Console.Write("-> Press any key to come back");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Press 1 to Edit Name");
        Console.WriteLine("Press 2 to Edit Price");
        Console.WriteLine("Press 3 to Edit Quantity");
        Console.WriteLine("Press 0 to Back");
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
                Console.Write("-> Enter New Product Name: ");
                product.name = Utility.CheckInput(Console.ReadLine());
                if (ProductDAL.UpdateProductAttribute(product.id, "product_name", product.name) != 0)
                {
                    Console.WriteLine("Cap nhat thanh cong");
                }
                else Console.WriteLine("Cap nhat that bai");
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                Console.Write("-> Enter New Price: ");
                string priceInput = Utility.CheckInput(Console.ReadLine());
                product.price = Utility.CheckDigitalInput(priceInput);
                if (ProductDAL.UpdateProductAttribute(product.id, "product_price", product.price) != 0)
                {
                    Console.WriteLine("Cap nhat thanh cong");
                }
                else Console.WriteLine("Cap nhat that bai");
                break;
            }
            else if (pressedKey.KeyChar == '3')
            {
                Console.Write("-> Enter New Quantity: ");
                string quantityInput = Utility.CheckInput(Console.ReadLine());
                product.quantity = Utility.CheckDigitalInput(quantityInput);
                if (ProductDAL.UpdateProductAttribute(product.id, "product_quantity", product.quantity) != 0)
                {
                    Console.WriteLine("Cap nhat thanh cong");
                }
                else Console.WriteLine("Cap nhat that bai");
                break;
            }
        }
        Console.Write("-> Press any key to come back");
        Console.ReadKey();
        return;
    }

    public static void RemoveProduct()
    {
        Console.Clear();
        Utility.PrintTitle("Product Management");
        Console.WriteLine("------------------------------Remove Product-------------------------------");
        Console.Write("-> Enter id product want remove: ");
        string input_ID = Console.ReadLine();
        Product product = ProductDAL.GetProductById(Utility.CheckDigitalInput(input_ID));
        if (product == null)
        {
            Console.WriteLine("cannot found");
            Console.Write("-> Press any key to come back");
            Console.ReadKey();
            return;
        }
        if (ProductDAL.UpdateProductAttribute(product.id, "product_status", "Removed") != 0)
        {
            Console.WriteLine("Cap hat thanh cong");
        }
        else Console.WriteLine("Cap nhat that bai");
        Console.Write("-> Press any key to come back");
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
            Console.WriteLine("cannot found");
            Console.Write("-> Press any key to come back");
            Console.ReadLine();
            return;
        }
        Utility.PrintOrderDetailTable(order);
        Console.WriteLine("Press 1 to Confirm");
        Console.WriteLine("Press 2 to Cancel");
        Console.WriteLine("Press 0 to Back");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                return;
            }
            else if (pressedKey.KeyChar == '1')
            {
                if (order.status == "Confirmed")
                {
                    Console.WriteLine("This order has been confirmed.");
                    Console.Write("-> Press any key to come back");
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
                    Console.WriteLine("This order has been cancelled.");
                    Console.Write("-> Press any key to come back");
                    Console.ReadKey();
                    return;
                }
                else if (order.status == "Confirmed")
                {
                    Console.WriteLine("This order has been confirmed, can not cancel.");
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
            Console.WriteLine("cap nhat thanh cong");
        }
        else Console.WriteLine("cap nhat that bai");
        Console.Write("-> Press any key to come back");
        Console.ReadKey();
    }

    public static void ViewRevenue()
    {
        Console.Clear();
        Utility.PrintTitle("Revenue Management");
        Console.WriteLine("---------------------------------Revenue----------------------------------");
        var (revenueDay, orderCountDay) = OrderDAL.GetReVenue(1);
        var (revenueMonth, orderCountMonth) = OrderDAL.GetReVenue(2);
        var (revenueYear, orderCountYear) = OrderDAL.GetReVenue(3);
        Utility.PrintRevenueTable(revenueDay, orderCountDay, revenueMonth, orderCountMonth, revenueYear, orderCountYear);
        Console.Write("-> Press any key to come back");
        Console.ReadKey();
        Console.Clear();
    }
}
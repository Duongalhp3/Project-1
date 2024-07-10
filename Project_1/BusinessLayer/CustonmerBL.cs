using Spectre.Console;

public abstract class CustomerBL
{
    private static Order order = new Order();

    public static void IsLogin(User user)
    {
        order.user = user;
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Online Shop");
            List<Product> menu = ProductDAL.GetAllProduct();
            int pageCount = (int)Math.Ceiling((double)menu.Count / pageSize);
            List<Product> products = menu.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (products.Count == pageSize)
            {
                Utility.PrintProductTable(products,1);
            }
            else
            {
                Utility.PrintProductTable(products,1);
                for (int i = 0; i < pageSize - products.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Purchase");
            Console.WriteLine("Press 2 to Search Product");
            Console.WriteLine("Press 3 to View Cart");
            Console.WriteLine("Press 4 to View My Order");
            Console.WriteLine("Press 5 to Change Password");
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
                    Purchase();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    SearchProduct();
                    break;
                }
                else if (pressedKey.KeyChar == '3')
                {
                    ViewCart();
                    break;
                }
                else if (pressedKey.KeyChar == '4')
                {
                    ViewMyOrder();
                    break;
                }
                else if (pressedKey.KeyChar == '5')
                {
                    Utility.ChangePassword(order.user);
                    break;
                }
            }
        }
    }

    private static void Purchase()
    {
        while(true)        
        {
            Console.Clear();
            Utility.PrintTitle("Online Shop");
            Console.WriteLine("----------------------------------Purchase----------------------------------");
            Product product = ChoiceProduct();
            if (product == null) return;
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Press 1 to Add To Cart");
            Console.WriteLine("Press 2 to Payment");
            Console.WriteLine("Press 0 to Exit");
            Console.WriteLine("------------------------------------");
            while (true)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
                if (pressedKey.KeyChar == '0')
                {
                    return;
                }
                else if (pressedKey.KeyChar == '1')
                {
                    AddProductToCart(product);
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    Order newOrder = new Order()
                    {
                        user = order.user
                    };
                    newOrder.orderProductList.Add(product);
                    Payment(newOrder);
                    return;
                }
            }
            Console.WriteLine("Do you want to buy more items. Press [y/n] ");
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

    private static void SearchProduct()
    {
        Console.Clear();
        Utility.PrintTitle("Online Shop");
        Console.WriteLine("------------------------------Search Product-------------------------------");
        Console.Write("-> Enter product name to search: ");
        string input = Utility.CheckInput(Console.ReadLine());
        List<Product> searchList = ProductDAL.GetProductByName(input);
        if (searchList == null)
        {
            AnsiConsole.MarkupLine("[Red]The product not found ![/]");
            Console.Write("-> Press any key to go back");
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
            Console.WriteLine("Press 1 to Purchase");
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
                    Purchase();
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

    private static void ViewCart()
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Online Shop");
            Console.WriteLine("-----------------------------------Cart------------------------------------");
            if (order.orderProductList.Count == 0)
            {
                AnsiConsole.MarkupLine("[Red]The cart is empty ![/]");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                return;
            }
            int pageCount = (int)Math.Ceiling((double)order.orderProductList.Count / pageSize);
            List<Product> products = order.orderProductList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (products.Count == pageSize)
            {
                Utility.PrintProductTable(products, 2);
            }
            else
            {
                Utility.PrintProductTable(products, 2);
                for (int i = 0; i < pageSize - products.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Edit Product In Cart");
            Console.WriteLine("Press 2 to Payment");
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
                    EditProductInCart(order);
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    Payment(order);
                    break;
                }
            }
        }
    }

    public static void ViewMyOrder()
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("------------------------------View My Order-----------------------------");
            List<Order> orderPurchasedList = OrderDAL.GetAllOrderByUser(order.user);
            if(orderPurchasedList == null)
            {
                AnsiConsole.MarkupLine("[Red]My order empty ![/]");
                Console.Write("-> Press any button to back");
                Console.ReadLine();
                return;
            }
            int pageCount = (int)Math.Ceiling((double)orderPurchasedList.Count / pageSize);
            List<Order> orderList = orderPurchasedList.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            if (orderList.Count == pageSize)
            {
                Utility.PrintOrderTable(orderList, 1);
            }
            else
            {
                Utility.PrintOrderTable(orderList, 1);
                for (int i = 0; i < pageSize - orderList.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            Console.WriteLine("Page: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to Cancel Order");
            Console.WriteLine("Press 2 to View Order Detail");
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
                    CancelOrder();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    ViewOrderDetail();
                    break;
                }
            }
        }
    }

    private static void AddProductToCart(Product newProduct)
    {
        Product product = ProductDAL.GetProductById(newProduct.id);
        Product oldProduct = order.orderProductList.Find(x => x.id == newProduct.id);
        if (oldProduct == null)
        {
            order.orderProductList.Add(newProduct);
        }
        else if(oldProduct.quantity + newProduct.quantity > product.quantity)
        {
            AnsiConsole.MarkupLine("[Red]Adding failed because the quantity exceeds the limit ![/]");
            return;
        }
        else
        {
            oldProduct.quantity = oldProduct.quantity + newProduct.quantity;
        }
        Console.WriteLine("Successfully added the product to the cart !");
    }

    private static void EditProductInCart(Order order)
    {
        Console.Clear();
        Utility.PrintTitle("Online Shop");
        Console.WriteLine("-------------------Edit Product In Cart------------------");
        Console.Write("-> Enter the product ID to edit: ");
        int id = Utility.CheckDigitalInput(Utility.CheckInput(Console.ReadLine()));
        Product product = order.orderProductList.Find(x => x.id == id);
        if (product == null)
        {
            AnsiConsole.MarkupLine("[Red]Invalid id ![/]");
            Console.Write("-> Press any button to back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Press 1 to Remove Product");
        Console.WriteLine("Press 2 de Edit Quantity");
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
                if (order.orderProductList.RemoveAll(x => x.id == id) > 0)
                {
                    Console.WriteLine("Delete Successful");
                }
                else
                {
                    AnsiConsole.MarkupLine("[Red]Delete Failed ![/]");
                }
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                Product product1 = ProductDAL.GetProductById(id);
                if (product1.status == "Out Of Stock")
                {
                    AnsiConsole.MarkupLine("[Red]This product is out of stock ![/]");
                    Console.Write("-> Press any key to go back");
                    Console.ReadLine();
                    break;
                }
                Console.Write("-> Enter new quanity: ");
                int newQuantity = int.Parse(Console.ReadLine());
                if (newQuantity < 1 || newQuantity > product1.quantity)
                {
                    AnsiConsole.MarkupLine("[Red]Invalid quantity ![/]");
                    Console.Write("-> Press any key to go back");
                    Console.ReadLine();
                    return;
                }
                order.orderProductList.Find(x => x.id == id).quantity = newQuantity;
                Console.WriteLine("Successfully edited quantity");
                Console.Write("-> Press any key to go back");
                Console.ReadLine();
                break;
            }
        }
    }

    private static void Payment(Order order)
    {
        Console.Clear();
        Utility.PrintTitle("Online Shop");
        Console.WriteLine("--------------------------Payment------------------------");
        bool canPayment = true;
        foreach (Product orderProduct in order.orderProductList)
        {
            if (!ProductDAL.CheckQuantity(orderProduct))
            {
                AnsiConsole.MarkupLine("[Red]The item with id = " + orderProduct.id + " in your cart does not have enough quantity or is out of stock ![/]");
                canPayment = false;
            }
        }
        if (!canPayment)
        {
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("Please select a payment method:");
        Console.WriteLine("Press 1 for Credit Card Payment");
        Console.WriteLine("Press 2 for Payment On Delivery");
        Console.WriteLine("Press 0 to Exit");
        Console.WriteLine("--------------------------------------------------");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                return;
            }
            else if (pressedKey.KeyChar == '1')
            {
                order.paymentMethod = "Credit Card Payment";
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                order.paymentMethod = "Payment On Delivery";
                break;
            }
        }
        int savedOrderId = OrderDAL.SaveOrder(order);
        if (savedOrderId != 0)
        {
            order.orderProductList.Clear();
            Order savedOrder = OrderDAL.GetOrderById(savedOrderId);
            Utility.PrintOrderDetailTable(savedOrder);
            Console.WriteLine("Order Successfully");
        }
        else AnsiConsole.MarkupLine("[Red]Order Failed ![/]");
        Console.Write("-> Press any key to go back");
        Console.ReadLine();
        return;
    }

    private static Product ChoiceProduct()
    {
        Console.Write("-> Enter product id to purchase: ");
        int id = Utility.CheckDigitalInput(Utility.CheckInput(Console.ReadLine()));
        Product product = ProductDAL.GetProductById(id);
        if (product == null)
        {
            AnsiConsole.MarkupLine("[Red]Invalid Id ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return null;
        }
        else if (product.status == "Out Of Stock")
        {
            AnsiConsole.MarkupLine("[Red]The product is out of stock ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return null;
        }
        else if (product.status == "Stop Selling")
        {
            AnsiConsole.MarkupLine("[Red]The product is stop selling ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return null;
        }
        Console.Write("-> Enter quantity: ");
        int quantity = Utility.CheckDigitalInput(Utility.CheckInput(Console.ReadLine()));
        if (quantity < 1 || quantity > product.quantity)
        {
            AnsiConsole.MarkupLine("[Red]Invalid quantity ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return null;
        }
        else
        {
            product.quantity = quantity;
        }
        return product;
    }

    private static void CancelOrder()
    {
        Console.Clear();
        Utility.PrintTitle("Online Shop");
        Console.WriteLine("--------------------------------Cancel Order-------------------------------");
        Console.Write("-> Enter order id to cancel: ");
        int id = Utility.CheckDigitalInput(Utility.CheckInput(Console.ReadLine()));
        Order order = OrderDAL.GetOrderById(id);
        if (order == null)
        {
            AnsiConsole.MarkupLine("[Red]Invalid Id ![/]");
            Console.Write("-> Press any button to back");
            Console.ReadLine();
            return;
        }
        else if (order.status == "Comfirmed")
        {
            AnsiConsole.MarkupLine("[Red]This order has been confirmed ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return;
        }
        else if (order.status == "Cancelled")
        {
            AnsiConsole.MarkupLine("[Red]This order has been canceled ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return;
        }
        Console.Write("-> Enter cancel reason: ");
        Utility.CheckInput(Console.ReadLine());
        if (OrderDAL.UpdateOrderAttribute(id, "order_status", "Cancel") != 0)
        {
            Console.WriteLine("Order cancel successful");
        }
        else
        {
            AnsiConsole.MarkupLine("[Red]Failed to cancel the order ![/]");
        }
        Console.Write("-> Press any key to go back");
        Console.ReadLine();
    }

    private static void ViewOrderDetail()
    {
        Console.Clear();
        Utility.PrintTitle("Online Shop");
        Console.WriteLine("-----------------------------View Order Detail-----------------------------");
        Console.Write("-> Enter order id to view order detail: ");
        int id = Utility.CheckDigitalInput(Utility.CheckInput(Console.ReadLine()));
        Order order = OrderDAL.GetOrderById(id);
        if (order == null)
        {
            AnsiConsole.MarkupLine("[Red]Invalid Id ![/]");
            Console.Write("-> Press any key to go back");
            Console.ReadLine();
            return;
        }
        Utility.PrintOrderDetailTable(order);
        Console.Write("-> Press any key to go back");
        Console.ReadLine();
    }
}

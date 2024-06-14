public abstract class CustomerBL
{
    public static void IsLogin(Order order)
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to buy a product");
            Console.WriteLine("Press 2 to search for a product");
            Console.WriteLine("Press 3 to view the cart");
            Console.WriteLine("Press 4 to manage orders");
            Console.WriteLine("Press 5 to change password");
            Console.WriteLine("Press 0 to log out");
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
                    BuyProduct(order);
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    SearchProduct(order);
                    break;
                }
                else if (pressedKey.KeyChar == '3')
                {
                    ShowCart(order);
                    break;
                }
                else if (pressedKey.KeyChar == '4')
                {
                    OrderManagement(order.user);
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

    private static void BuyProduct(Order order)
    {
        while(true)        
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("--------------------------------Buy Product--------------------------------");
            Product product = ChoiceProduct();
            if (product == null) return;
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Press 1 to add to cart");
            Console.WriteLine("Press 2 to checkout");
            Console.WriteLine("Press 0 to exit");
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
                    AddProductToCart(order, product);
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
            Console.Write("Do you want to buy more items? Press [y/n] ");
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

    private static void SearchProduct(Order order)
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("------------------------------Search Product-------------------------------");
        Console.Write("Enter the name of the product to search: ");
        string input = Utility.CheckInput(Console.ReadLine());
        List<Product> searchList = ProductDAL.GetProductByName(input);
        if (searchList == null)
        {
            Console.WriteLine("Product not found");
            Console.Write("Press any key to go back");
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
            Console.WriteLine("Press Left Arrow/Right Arrow to change page");
            Console.WriteLine("Press 1 to purchase");
            Console.WriteLine("Press 0 to return");
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
                    BuyProduct(order);
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

    private static void ShowCart(Order order)
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("-----------------------------------Cart------------------------------------");
            if (order.orderProductList.Count == 0)
            {
                Console.WriteLine("Gio hang trong !");
                Console.Write("Press any button to back");
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
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
            Console.WriteLine("Nhan 1 de edit san pham trong gio hang");
            Console.WriteLine("Nhan 2 de thanh toan");
            Console.WriteLine("Nhan 0 de thoat");
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

    public static void OrderManagement(User user)
    {
        int pageSize = 10;
        int currentPage = 1;
        while (true)
        {
            Console.Clear();
            Utility.PrintTitle("Caffe Store");
            Console.WriteLine("------------------------------Order Management-----------------------------");
            List<Order> orderPurchasedList = OrderDAL.GetAllOrderByUser(user);
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
            Console.WriteLine("Trang: <" + currentPage + "/" + pageCount + ">");
            Console.WriteLine("Nhan Left Arrow/Right Arrow de chuyen trang");
            Console.WriteLine("Nhan 1 de huy don hang");
            Console.WriteLine("Nhan 2 de xem chi tiet don hang");
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
                    CancelOrder();
                    break;
                }
                else if (pressedKey.KeyChar == '2')
                {
                    ShowOrderDetail();
                    break;
                }
            }
        }
    }

    private static void AddProductToCart(Order order, Product newProduct)
    {
        Product product = ProductDAL.GetProductById(newProduct.id);
        Product oldProduct = order.orderProductList.Find(x => x.id == newProduct.id);
        if (oldProduct == null)
        {
            order.orderProductList.Add(newProduct);
        }
        else if(oldProduct.quantity + newProduct.quantity > product.quantity)
        {
            Console.WriteLine("Them khong thanh cong vi so luong vuot qua gioi han !");
            return;
        }
        else
        {
            oldProduct.quantity = oldProduct.quantity + newProduct.quantity;
        }
        Console.WriteLine("Them thanh cong san pham vao gio hang !");
    }

    private static void EditProductInCart(Order order)
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("-------------------Edit Product In Cart------------------");
        Console.Write("Nhap id san pham muon edit: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        Product product = order.orderProductList.Find(x => x.id == id);
        if (product == null)
        {
            Console.WriteLine("id ko hop le");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Nhan 1 de Xoa san pham");
        Console.WriteLine("Nhan 2 de Chinh sua so luong");
        Console.WriteLine("Nhan 0 de Thoat");
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
                    Console.WriteLine("Xoa Thanh Cong");
                }
                else
                {
                    Console.WriteLine("Xoa That Bai");
                }
                Console.Write("Press any button to back");
                Console.ReadLine();
                break;
            }
            else if (pressedKey.KeyChar == '2')
            {
                Product product1 = ProductDAL.GetProductById(id);
                if (product1.status == "Out Of Stock")
                {
                    Console.WriteLine("San pham nay da het hang");
                    Console.Write("Press any button to back");
                    Console.ReadLine();
                    break;
                }
                int newQuantity;
                Console.Write("Nhap so luong muon chinh sua: ");
                do
                {
                    newQuantity = int.Parse(Console.ReadLine());
                    if (newQuantity < 1 || newQuantity > product1.quantity)
                    {
                        Console.Write("So luong khong phu hop, xin moi nhap lai: ");
                    }
                } while (newQuantity < 1 || newQuantity > product1.quantity);
                order.orderProductList.Find(x => x.id == id).quantity = newQuantity;
                Console.WriteLine("Chinh sua so luong thanh cong");
                Console.Write("Press any button to back");
                Console.ReadLine();
                break;
            }
        }
    }

    private static void Payment(Order order)
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("--------------------------Payment------------------------");
        bool canPayment = true;
        foreach (Product orderProduct in order.orderProductList)
        {
            if (!ProductDAL.CheckQuantity(orderProduct))
            {
                Console.WriteLine("");
                Console.WriteLine("Mat hang co id = " + orderProduct.id + " trong gio hang cua ban khong du so luong hoac het hang");
                canPayment = false;
            }
        }
        if (!canPayment)
        {
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        Console.WriteLine("Xin moi chon phuong thuc thanh toan:");
        Console.WriteLine("nhan 1 de chon thanh toan qua Credit Card");
        Console.WriteLine("Nhan 2 de chon thanh toan khi nhan hang");
        Console.WriteLine("Nhan 0 de tro lai");
        while (true)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(intercept: true);
            if (pressedKey.KeyChar == '0')
            {
                return;
            }
            else if (pressedKey.KeyChar == '1')
            {
                order.paymentMethod = "Credit Card";
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
            Console.WriteLine("Dat Hang Thanh Cong !");
        }
        else Console.WriteLine("Dat Hang That Bai");
        Console.Write("Press any button to back");
        Console.ReadLine();
        return;
    }

    private static Product ChoiceProduct()
    {
        int quantity;
        Console.Write("Nhap Ma San Pham Muon Mua: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        Product product = ProductDAL.GetProductById(id);
        if (product == null)
        {
            Console.WriteLine("Ma San Pham Khong Dung");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return null;
        }
        else if (product.status == "Out Of Stock")
        {
            Console.WriteLine("San pham het hang");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return null;
        }
        Console.Write("Nhap so luong: ");
        do
        {
            quantity = int.Parse(Utility.CheckInput(Console.ReadLine()));
            if (quantity < 1 || quantity > product.quantity)
            {
                Console.Write("So luong ko hop le, xin moi nhap lai so luong: ");
            }
            else
            {
                product.quantity = quantity;
            }
        } while (quantity < 1 || quantity > product.quantity);
        return product;
    }

    private static void CancelOrder()
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("--------------------------------Cancel Order-------------------------------");
        Console.Write("Nhap id oder muon cancel: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        Order order = OrderDAL.GetOrderById(id);
        if (order == null)
        {
            Console.WriteLine("id ko hop le");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        else if (order.status != "Pending")
        {
            Console.WriteLine("don hang nay khong the huy");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        Console.Write("Nhap ly do huy: ");
        Utility.CheckInput(Console.ReadLine());
        if (OrderDAL.UpdateOrderAttribute(id, "order_status", "Cancel") != 0)
        {
            Console.WriteLine("Huy don hanh thanh cong");
        }
        else
        {
            Console.WriteLine("Huy don hang that bai");
        }
        Console.Write("Press any button to back");
        Console.ReadLine();
    }

    private static void ShowOrderDetail()
    {
        Console.Clear();
        Utility.PrintTitle("Caffe Store");
        Console.WriteLine("-----------------------------Show Order Detail-----------------------------");
        Console.Write("Nhap id oder muon xem chi tiet: ");
        int id = int.Parse(Utility.CheckInput(Console.ReadLine()));
        Order order = OrderDAL.GetOrderById(id);
        if (order == null)
        {
            Console.WriteLine("id ko hop le");
            Console.Write("Press any button to back");
            Console.ReadLine();
            return;
        }
        Utility.PrintOrderDetailTable(order);
        Console.Write("Press any button to back");
        Console.ReadLine();
    }
}

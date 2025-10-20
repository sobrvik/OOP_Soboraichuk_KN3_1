using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5v12
{
    // ---------------------------
    // Власні винятки
    // ---------------------------
    public class InvalidMenuItemException : Exception
    {
        public InvalidMenuItemException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
  
    //MenuItem, OrderLine, Order (композиція)
    public class MenuItem
    {
        public string Code { get; }                 // унікальний код позиції
        public string Name { get; }
        public bool IsVegetarian { get; }
        public int Calories { get; }
        public decimal Price { get; }

        public MenuItem(string code, string name, bool isVegetarian, int calories, decimal price)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new InvalidMenuItemException("Code is required.");
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidMenuItemException("Name is required.");
            if (calories <= 0) throw new InvalidMenuItemException("Calories must be > 0.");
            if (price <= 0) throw new InvalidMenuItemException("Price must be > 0.");

            Code = code.Trim();
            Name = name.Trim();
            IsVegetarian = isVegetarian;
            Calories = calories;
            Price = price;
        }

        public override string ToString() => $"{Name} ({(IsVegetarian ? "Veg" : "Non-veg")}) - {Calories} kcal, {Price:C}";
    }

    // Позиція замовлення тримає посилання на MenuItem (через композицію у складі Order)
    public class OrderLine
    {
        public MenuItem Item { get; }
        public int Quantity { get; }

        public OrderLine(MenuItem item, int quantity)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be > 0.");

            Item = item;
            Quantity = quantity;
        }

        public int LineCalories => Item.Calories * Quantity;
        public decimal LineTotal => Item.Price * Quantity;

        public override string ToString() => $"{Item.Name} x{Quantity} = {LineTotal:C} ({LineCalories} kcal)";
    }

    // Замовлення складається з OrderLine (КОМПОЗИЦІЯ)
    public class Order
    {
        private readonly List<OrderLine> _lines = new();
        public string OrderId { get; }
        public DateTime CreatedAt { get; } = DateTime.Now;

        public Order(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId)) throw new ArgumentException("OrderId is required.");
            OrderId = orderId.Trim();
        }

        public void AddLine(OrderLine line) => _lines.Add(line);
        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

        // Обчислення
        public decimal Total => _lines.Sum(l => l.LineTotal);
        public int TotalCalories => _lines.Sum(l => l.LineCalories);

        // Вегетаріанська частка (за кількістю одиниць, а не найменувань)
        public double VegetarianSharePercent
        {
            get
            {
                int totalQty = _lines.Sum(l => l.Quantity);
                if (totalQty == 0) return 0;
                int vegQty = _lines.Where(l => l.Item.IsVegetarian).Sum(l => l.Quantity);
                return (double)vegQty / totalQty * 100.0;
            }
        }

        public override string ToString() => $"Order {OrderId}: {Lines.Count} lines, Total = {Total:C}, Calories = {TotalCalories}, Veg% = {VegetarianSharePercent:F1}%";
    }

    // Дженерики: IRepository<T> / InMemoryRepository<T>
    public interface IRepository<T>
    {
        void Add(T entity);
        bool Remove(Predicate<T> predicate);
        T Find(Predicate<T> predicate);
        IEnumerable<T> All();
        IEnumerable<T> Where(Predicate<T> predicate);
    }

    public class InMemoryRepository<T> : IRepository<T>
    {
        private readonly List<T> _items = new();

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _items.Add(entity);
        }

        public bool Remove(Predicate<T> predicate)
        {
            var idx = _items.FindIndex(predicate);
            if (idx >= 0)
            {
                _items.RemoveAt(idx);
                return true;
            }
            return false;
        }

        public T Find(Predicate<T> predicate)
        {
            var entity = _items.Find(predicate);
            if (entity == null) throw new NotFoundException("Entity not found.");
            return entity;
        }

        public IEnumerable<T> All() => _items.ToList();

        public IEnumerable<T> Where(Predicate<T> predicate) => _items.Where(x => predicate(x));
    }

    // Дженерик-компонент: Filter<T>(IEnumerable<T>, Predicate<T>)
    public static class GenericUtils
    {
        public static IEnumerable<T> Filter<T>(IEnumerable<T> src, Predicate<T> predicate)
        {
            foreach (var item in src)
                if (predicate(item)) yield return item;
        }

        // Додатково: Max<T> з IComparer<T> (приклад)
        public static T Max<T>(IEnumerable<T> src, IComparer<T> comparer)
        {
            using var it = src.GetEnumerator();
            if (!it.MoveNext()) throw new InvalidOperationException("Sequence contains no elements.");
            T best = it.Current;
            while (it.MoveNext())
            {
                if (comparer.Compare(it.Current, best) > 0)
                    best = it.Current;
            }
            return best;
        }
    }

    // Порівняння MenuItem за калоріями
    public class MenuItemCaloriesComparer : IComparer<MenuItem>
    {
        public int Compare(MenuItem? x, MenuItem? y)
        {
            if (x == null || y == null) throw new ArgumentNullException();
            return x.Calories.CompareTo(y.Calories);
        }
    }

    // Program.cs — демонстрація, try-catch, обчислення
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Репозиторії меню та замовлень
            IRepository<MenuItem> menuRepo = new InMemoryRepository<MenuItem>();
            IRepository<Order> orderRepo = new InMemoryRepository<Order>();

            // 1) Контроль вхідних даних + власні винятки
            try
            {
                // Валідні позиції меню
                menuRepo.Add(new MenuItem("S01", "Салат овочевий", isVegetarian: true, calories: 180, price: 95m));
                menuRepo.Add(new MenuItem("P01", "Піца Маргарита", isVegetarian: true, calories: 820, price: 189m));
                menuRepo.Add(new MenuItem("B01", "Бургер класичний", isVegetarian: false, calories: 950, price: 215m));

                // Навмисно невалідна позиція (кине InvalidMenuItemException)
                menuRepo.Add(new MenuItem("X00", "Фантом страва", isVegetarian: false, calories: 0, price: 100m));
            }
            catch (InvalidMenuItemException ex)
            {
                Console.WriteLine($"[InvalidMenuItemException] {ex.Message}");
            }

            Console.WriteLine("\n=== MENU (All) ===");
            foreach (var m in menuRepo.All())
                Console.WriteLine($" - {m}");

            // 2) Дженерик Filter<T>: відфільтруємо вегетаріанські позиції
            var vegetarianOnly = GenericUtils.Filter(menuRepo.All(), m => m.IsVegetarian);
            Console.WriteLine("\n=== Vegetarian Only ===");
            foreach (var m in vegetarianOnly)
                Console.WriteLine($" - {m}");

            // 3) Створимо 2 замовлення (КОМПОЗИЦІЯ: Order містить OrderLine, які посилаються на MenuItem)
            var order1 = new Order("ORD-1001");
            order1.AddLine(new OrderLine(menuRepo.Find(m => m.Code == "S01"), 2)); // 2 салати
            order1.AddLine(new OrderLine(menuRepo.Find(m => m.Code == "B01"), 1)); // 1 бургер
            orderRepo.Add(order1);

            var order2 = new Order("ORD-1002");
            order2.AddLine(new OrderLine(menuRepo.Find(m => m.Code == "P01"), 1)); // піца
            order2.AddLine(new OrderLine(menuRepo.Find(m => m.Code == "S01"), 1)); // салат
            orderRepo.Add(order2);

            // Виведемо обидва замовлення
            Console.WriteLine("\n=== Orders ===");
            foreach (var o in orderRepo.All())
            {
                Console.WriteLine(o);
                foreach (var line in o.Lines)
                    Console.WriteLine($"   • {line}");
            }

            // 4) Обчислення/операції з колекціями
            // Середній чек по всіх замовленнях
            var allOrders = orderRepo.All().ToList();
            var averageCheck = allOrders.Any() ? allOrders.Average(o => o.Total) : 0m;

            // Мін/Макс калорійна позиція в меню (демо GenericUtils.Max з IComparer)
            var mostCaloric = GenericUtils.Max(menuRepo.All(), new MenuItemCaloriesComparer());

            Console.WriteLine("\n=== Aggregates ===");
            Console.WriteLine($"Average check (середній чек): {averageCheck:C}");
            Console.WriteLine($"Most caloric menu item: {mostCaloric.Name} — {mostCaloric.Calories} kcal");

            // 5) Обробка винятків пошуку (NotFoundException)
            try
            {
                var missing = menuRepo.Find(m => m.Code == "NOPE");
                Console.WriteLine(missing); // не виконається
            }
            catch (NotFoundException nf)
            {
                Console.WriteLine($"\n[NotFoundException] {nf.Message}");
            }

            Console.WriteLine("\nDone. Натисніть Enter, щоб вийти...");
            Console.ReadLine();
        }
    }
}
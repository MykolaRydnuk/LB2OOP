using System;
using System.Collections.Generic;
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager manager = new ReservationManager();
        manager.AddRestaurant("A", 10);
        manager.AddRestaurant("B", 5);

        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}
public class ReservationManager
{
    public List<Restaurant> Restaurants { get; }

    public ReservationManager()
    {
        Restaurants = new List<Restaurant>();
    }

    public void AddRestaurant(string name, int tableCount)
    {
        try
        {
            var restaurant = new Restaurant { Name = name, Tables = new RestaurantTable[tableCount] };

            for (int i = 0; i < tableCount; i++)
            {
                restaurant.Tables[i] = new RestaurantTable();
            }

            Restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error AddRestaurant");
        }
    }

    private void LoadRestaurantsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error LoadRestaurantsFromFile");
        }
    }

    public List<string> FindAllFreeTables(DateTime dateTime)
    {
        try
        {
            List<string> freeTables = new List<string>();

            foreach (var restaurant in Restaurants)
            {
                for (int i = 0; i < restaurant.Tables.Length; i++)
                {
                    if (!restaurant.Tables[i].IsBooked(dateTime))
                    {
                        freeTables.Add($"{restaurant.Name} - Table {i + 1}");
                    }
                }
            }

            return freeTables;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error FindAllFreeTables");
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime dateTime, int tableNumber)
    {
        try
        {
            var restaurant = Restaurants.Find(r => r.Name == restaurantName);

            if (restaurant != null && tableNumber >= 0 && tableNumber < restaurant.Tables.Length)
            {
                return restaurant.Tables[tableNumber].Book(dateTime);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error BookTable");
        }

        return false;
    }

    public void Sort(DateTime dateTime)
    {
        try
        {
            Restaurants.Sort((r1, r2) => Count(r2, dateTime).CompareTo(Count(r1, dateTime)));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Sort");
        }
    }

    public int Count(Restaurant restaurant, DateTime dateTime)
    {
        try
        {
            return restaurant.Tables.Count(table => !table.IsBooked(dateTime));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Count");
            return 0;
        }
    }
}


public class Restaurant
{
    public string name; 
    public RestaurantTable[] table;

    public string Name { get; internal set; }
    public RestaurantTable[] Tables { get; internal set; }
}

public class RestaurantTable
{
    private List<DateTime> bookdate;


    public RestaurantTable()
    {
        bookdate = new List<DateTime>();
    }

    public bool Book(DateTime d)
    {
        try
        { 
            if (bookdate.Contains(d))
            {
                return false;
            }
            bookdate.Add(d);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Book");
            return false;
        }
    }

    public bool IsBooked(DateTime d)
    {
        return bookdate.Contains(d);
    }
}

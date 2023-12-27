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
    public List<Restaurant> res;

    public ReservationManager()
    {
        res = new List<Restaurant>();
    }

    public void AddRestaurant(string n, int t)
    {
        try
        {
            Restaurant restaurant = new Restaurant();
            restaurant.name = n;
            restaurant.table = new RestaurantTable[t];
            for (int i = 0; i < t; i++)
            {
                restaurant.table[i] = new RestaurantTable();
            }
            res.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error AddRestaurant");
        }
    }

    private void LoadRestaurantsFromFile(string fileP)
    {
        try
        {
            string[] ls = File.ReadAllLines(fileP);
            foreach (string l in ls)
            {
                var parts = l.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(l);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error LoadRestaurantsFromFile");
        }
    }
    public List<string> FindAllFreeTables(DateTime dt)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var restaurant in res)
            {
                for (int i = 0; i < restaurant.table.Length; i++)
                {
                    if (!restaurant.table[i].IsBooked(dt))
                    {
                        free.Add($"{restaurant.name} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error FindAllFreeTables");
            return new List<string>();
        }
    }

    public bool BookTable(string rName, DateTime d, int tNumber)
    {
        foreach (var r in res)
        {
            if (r.name == rName)
            {
                if (tNumber < 0 || tNumber >= r.table.Length)
                {
                    throw new Exception(null);
                }

                return r.table[tNumber].Book(d);
            }
        }

        throw new Exception(null);
    }

    public void Sort(DateTime dt)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int avTc = Count(res[i], dt);
                    int avTn = Count(res[i + 1], dt);

                    if (avTc < avTn)
                    {
                        var temp = res[i];
                        res[i] = res[i + 1];
                        res[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Sort");
        }
    }

    public int Count(Restaurant restaurant, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var table in restaurant.table)
            {
                if (!table.IsBooked(dt))
                {
                    count++;
                }
            }
            return count;
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

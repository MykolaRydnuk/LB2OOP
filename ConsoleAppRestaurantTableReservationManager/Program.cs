using System;
using System.Collections.Generic;
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager m = new ReservationManager();
        m.AddRestaurant("A", 10);
        m.AddRestaurant("B", 5);

        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
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
            Restaurant r = new Restaurant();
            r.n = n;
            r.t = new RestaurantTable[t];
            for (int i = 0; i < t; i++)
            {
                r.t[i] = new RestaurantTable();
            }
            res.Add(r);
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
            foreach (var r in res)
            {
                for (int i = 0; i < r.t.Length; i++)
                {
                    if (!r.t[i].IsBooked(dt))
                    {
                        free.Add($"{r.n} - Table {i + 1}");
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
            if (r.n == rName)
            {
                if (tNumber < 0 || tNumber >= r.t.Length)
                {
                    throw new Exception(null);
                }

                return r.t[tNumber].Book(d);
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

    public int Count(Restaurant r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.t)
            {
                if (!t.IsBooked(dt))
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
    public string n; 
    public RestaurantTable[] t; 
}

public class RestaurantTable
{
    private List<DateTime> bd;


    public RestaurantTable()
    {
        bd = new List<DateTime>();
    }

    public bool Book(DateTime d)
    {
        try
        { 
            if (bd.Contains(d))
            {
                return false;
            }
            bd.Add(d);
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
        return bd.Contains(d);
    }
}

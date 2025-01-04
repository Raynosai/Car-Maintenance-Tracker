// Car Maintenance Tracker using C# and OOP Principles

using System;
using System.Collections.Generic;
using System.Linq;

// Base Class: Vehicle
public class Vehicle
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LuxuryLevel { get; set; } // New property for luxury level
    public List<MaintenanceTask> MaintenanceTasks { get; set; }

    public Vehicle(string make, string model, int year, string luxuryLevel)
    {
        Make = make;
        Model = model;
        Year = year;
        LuxuryLevel = luxuryLevel;
        MaintenanceTasks = new List<MaintenanceTask>();
    }

    public void AddMaintenanceTask(MaintenanceTask task)
    {
        task.AdjustCost(LuxuryLevel); // Adjust cost based on luxury level
        MaintenanceTasks.Add(task);
        Console.WriteLine($"Task '{task.Description}' added to {Make} {Model}.");
    }

    public void DisplayMaintenanceTasks()
    {
        Console.WriteLine($"\nMaintenance Tasks for {Year} {Make} {Model}:");
        foreach (var task in MaintenanceTasks)
        {
            task.DisplayTaskInfo();
        }
    }

    public decimal CalculateTotalMaintenanceCost()
    {
        return MaintenanceTasks.Sum(task => task.Cost);
    }
}

// Derived Class: MaintenanceTask
public class MaintenanceTask
{
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public bool IsCompleted { get; set; }

    public MaintenanceTask(string description, decimal baseCost)
    {
        Description = description;
        Cost = baseCost;
        IsCompleted = false;
    }

    public void AdjustCost(string luxuryLevel)
    {
        switch (luxuryLevel.ToLower())
        {
            case "economy":
                Cost *= 1.0m;
                break;
            case "standard":
                Cost *= 1.2m;
                break;
            case "luxury":
                Cost *= 1.5m;
                break;
            default:
                Console.WriteLine("Unknown luxury level. No cost adjustment applied.");
                break;
        }
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        Console.WriteLine($"Task '{Description}' marked as completed.");
    }

    public void DisplayTaskInfo()
    {
        decimal kdvRate = 0.18m;
        decimal costWithKDV = Cost * (1 + kdvRate);
        decimal usdToTryRate = 35.1m; // Updated conversion rate
        decimal costInTRY = costWithKDV * usdToTryRate;

        Console.WriteLine($"Description: {Description}, Cost: ${Cost:F2} (Cost with KDV: ${costWithKDV:F2} / {costInTRY:F2} TRY), Completed: {IsCompleted}");
    }
}

// Utility Class: MaintenanceManager
public static class MaintenanceManager
{
    private static Dictionary<string, decimal> BaseCosts = new Dictionary<string, decimal>
    {
        { "Oil Change", 30 },
        { "Tire Rotation", 25 },
        { "Brake Inspection", 50 },
        { "Battery Check", 20 },
        { "Engine Tune-Up", 100 },
        { "Transmission Service", 150 },
        { "Coolant Flush", 70 },
        { "Air Filter Replacement", 40 }
    };

    private static List<string> AvailableMaintenance = BaseCosts.Keys.ToList();

    public static void DisplaySummary(List<Vehicle> vehicles)
    {
        Console.WriteLine("\n--- Maintenance Summary ---");
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine($"{vehicle.Year} {vehicle.Make} {vehicle.Model}: {vehicle.MaintenanceTasks.Count} tasks.");
        }
    }

    public static void CalculateTotalCost(List<Vehicle> vehicles)
    {
        decimal totalCost = 0;
        decimal kdvRate = 0.18m;
        decimal usdToTryRate = 35.1m; // Updated conversion rate

        foreach (var vehicle in vehicles)
        {
            totalCost += vehicle.CalculateTotalMaintenanceCost();
        }

        decimal totalCostWithKDV = totalCost * (1 + kdvRate);
        decimal totalCostInTRY = totalCostWithKDV * usdToTryRate;

        Console.WriteLine($"\nTotal Maintenance Cost for All Vehicles: ${totalCost:F2} (With KDV: ${totalCostWithKDV:F2} / {totalCostInTRY:F2} TRY)");
    }

    public static void DisplayDetailedReport(List<Vehicle> vehicles)
    {
        Console.WriteLine("\n--- Detailed Maintenance Report ---");
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine($"Vehicle: {vehicle.Year} {vehicle.Make} {vehicle.Model}");
            vehicle.DisplayMaintenanceTasks();
            decimal kdvRate = 0.18m;
            decimal usdToTryRate = 35.1m; // Updated conversion rate
            decimal totalCost = vehicle.CalculateTotalMaintenanceCost();
            decimal totalCostWithKDV = totalCost * (1 + kdvRate);
            decimal totalCostInTRY = totalCostWithKDV * usdToTryRate;

            Console.WriteLine($"Total Cost for {vehicle.Make} {vehicle.Model}: ${totalCost:F2} (With KDV: ${totalCostWithKDV:F2} / {totalCostInTRY:F2} TRY)");
        }
    }

    public static Vehicle SelectVehicle(List<Vehicle> vehicles)
    {
        Console.WriteLine("\nSelect a vehicle by entering its number:");
        for (int i = 0; i < vehicles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {vehicles[i].Year} {vehicles[i].Make} {vehicles[i].Model} ({vehicles[i].LuxuryLevel})");
        }

        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > vehicles.Count)
        {
            Console.WriteLine("Invalid choice. Please try again.");
        }

        return vehicles[choice - 1];
    }

    public static List<MaintenanceTask> CreateMultipleMaintenanceTasks(string luxuryLevel)
    {
        List<MaintenanceTask> tasks = new List<MaintenanceTask>();

        Console.WriteLine("\nAvailable Maintenance Services:");
        for (int i = 0; i < AvailableMaintenance.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {AvailableMaintenance[i]}");
        }

        Console.WriteLine("Select maintenance services by entering their numbers separated by commas (e.g., 1,3,5):");
        string input = Console.ReadLine();
        var choices = input.Split(',');

        foreach (var choice in choices)
        {
            if (int.TryParse(choice.Trim(), out int serviceChoice) && serviceChoice >= 1 && serviceChoice <= AvailableMaintenance.Count)
            {
                string description = AvailableMaintenance[serviceChoice - 1];
                decimal baseCost = BaseCosts[description];
                tasks.Add(new MaintenanceTask(description, baseCost));
            }
            else
            {
                Console.WriteLine($"Invalid choice '{choice}'. Skipping...");
            }
        }

        return tasks;
    }

    public static void DisplayLastVehicleSummary(Vehicle vehicle)
    {
        Console.WriteLine($"\n--- Summary for {vehicle.Year} {vehicle.Make} {vehicle.Model} ---");
        vehicle.DisplayMaintenanceTasks();
        decimal kdvRate = 0.18m;
        decimal usdToTryRate = 35.1m; // Updated conversion rate
        decimal totalCost = vehicle.CalculateTotalMaintenanceCost();
        decimal totalCostWithKDV = totalCost * (1 + kdvRate);
        decimal totalCostInTRY = totalCostWithKDV * usdToTryRate;

        Console.WriteLine($"Total Maintenance Cost: ${totalCost:F2} (With KDV: ${totalCostWithKDV:F2} / {totalCostInTRY:F2} TRY)");
    }
}

// Main Program
class Program
{
    static void Main()
    {
        // Create Vehicles
        var car1 = new Vehicle("Toyota", "Corolla", 2020, "Economy");
        var car2 = new Vehicle("Ford", "F-150", 2018, "Standard");
        var car3 = new Vehicle("Honda", "Civic", 2021, "Economy");
        var car4 = new Vehicle("Chevrolet", "Malibu", 2019, "Standard");
        var car5 = new Vehicle("BMW", "X5", 2022, "Luxury");
        var car6 = new Vehicle("Audi", "A6", 2023, "Luxury");
        var car7 = new Vehicle("Hyundai", "Elantra", 2022, "Economy");
        var car8 = new Vehicle("Mercedes", "C-Class", 2021, "Luxury");
        var car9 = new Vehicle("Nissan", "Altima", 2020, "Standard");
        var car10 = new Vehicle("Kia", "Sportage", 2023, "Economy");

        var vehicles = new List<Vehicle> { car1, car2, car3, car4, car5, car6, car7, car8, car9, car10 };

        Console.WriteLine("Welcome to the Car Maintenance Tracker!");

        Vehicle currentVehicle = null;

        while (true)
        {
            if (currentVehicle == null)
            {
                Console.WriteLine("\nSelect a vehicle to work on:");
                currentVehicle = MaintenanceManager.SelectVehicle(vehicles);
                Console.WriteLine($"You are now working on: {currentVehicle.Year} {currentVehicle.Make} {currentVehicle.Model}.");
            }

            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add Maintenance Task");
            Console.WriteLine("2. Display Maintenance Tasks");
            Console.WriteLine("3. Finish Working on This Vehicle");
            Console.WriteLine("4. Maintenance Summary");
            Console.WriteLine("5. Total Cost");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            if (choice == 6)
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            switch (choice)
            {
                case 1:
                    var newTasks = MaintenanceManager.CreateMultipleMaintenanceTasks(currentVehicle.LuxuryLevel);
                    foreach (var task in newTasks)
                    {
                        currentVehicle.AddMaintenanceTask(task);
                    }
                    break;
                case 2:
                    currentVehicle.DisplayMaintenanceTasks();
                    break;
                case 3:
                    MaintenanceManager.DisplayLastVehicleSummary(currentVehicle);
                    currentVehicle = null;
                    break;
                case 4:
                    MaintenanceManager.DisplaySummary(vehicles);
                    break;
                case 5:
                    MaintenanceManager.CalculateTotalCost(vehicles);
                    break;
            }
        }
    }
}

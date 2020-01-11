using Dapper;
using Dapper.Contrib.Extensions;
using DbUp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Dapper_ADONET
{
    class Program
    {
        private const string connectionString = "Server=DESKTOP-468M5GV;Database=ShopDapper;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            InsertTable();
            Console.WriteLine("Введите имя товара: ");
            var nameItem = Console.ReadLine();
            List<Item> items = new List<Item>();
            items = Search(nameItem) as List<Item>;

            Console.WriteLine("Введите страницу: ");
            int page = 1, count = 3;
            if (int.TryParse(Console.ReadLine(), out page))
            {
                var paginationResult = ToPagination(items, page, count);
                foreach (var item in paginationResult)
                {
                    Console.WriteLine($"{item.Name} | {item.Price} | {item.Description} | {item.CategoryId} |");
                }
                if (paginationResult.Count == 0)
                {
                    Console.WriteLine("Пусто!");
                }
            }
            else
            {
                Console.WriteLine("Не правильный ввод!");
            }
        }
        public static List<Item> ToPagination(List<Item> result, int page = 0, int count = 0)
        {
            var paginationResult = result.Skip(page * count).Take(count);
            var items = paginationResult.ToList();

            return items;
        }

        private static ICollection<Item> Search(string nameItem)
        {
            string sql = "Select * from Items where Name = @Name";
            List<Item> items = new List<Item>();
            using (var connection = new SqlConnection(connectionString))
            {
                items = connection.Query<Item>(sql, new { Name = nameItem }).ToList();
            }
            return items;
        }

        private static void InsertTable()
        {
            List<Category> categories = new List<Category> {
                new Category
                {
                  Name = "Phone",
                  ImagePath = "C:/data"
                },
                new Category
                {
                  Name = "Computer",
                  ImagePath = "C:/data"
                },
            };
            List<Item> items = new List<Item> {
                new Item
                {
                    Name = "Samsung Galaxy S5",
                    ImagePath = "C:/data",
                    Price = 150_990,
                    Description = "Better for this price",
                },
                new Item
                {
                    Name = "Samsung chiller 2016",
                    ImagePath = "C:/data",
                    Price = 500_000,
                    Description = "For save tasty foods",
                },
                new Item
                {
                    Name = "Bosch",
                    ImagePath = "C:/data",
                    Price = 1_000_000,
                    Description = "Better dishwasher",
                },
            };
            using (var connection = new SqlConnection(connectionString))
            {
                foreach (var item in items)
                {
                    connection.Insert(item);
                }
            }
        }
    }
}

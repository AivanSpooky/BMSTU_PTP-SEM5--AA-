using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SQLiteToJson
{
    public class Recipe
    {
        [JsonProperty("issue_id")]
        public int IssueId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ingredients")]
        public string Ingredients { get; set; }

        [JsonProperty("steps")]
        public string Steps { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = "../../../../conveyor/bin/Debug/recipes.db";
            string jsonOutputPath = "../../../recipes.json";

            try
            {
                List<Recipe> recipes = GetRecipesFromDatabase(databasePath);

                // Проверка данных перед сериализацией
                foreach (var recipe in recipes)
                {
                    Console.WriteLine($"Title: {recipe.Title}");
                }

                string jsonString = SerializeRecipesToJson(recipes);
                File.WriteAllText(jsonOutputPath, jsonString, Encoding.UTF8); // Явно указываем UTF-8
                Console.WriteLine($"Данные успешно экспортированы в файл {jsonOutputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static List<Recipe> GetRecipesFromDatabase(string databasePath)
        {
            var recipes = new List<Recipe>();

            if (!File.Exists(databasePath))
                throw new FileNotFoundException("Файл базы данных не найден.", databasePath);

            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT issue_id, url, title, ingredients, steps, image_url FROM Recipes";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var recipe = new Recipe
                            {
                                IssueId = reader.GetInt32(reader.GetOrdinal("issue_id")),
                                Url = reader["url"] as string,
                                Title = reader["title"] as string,
                                Ingredients = reader["ingredients"] as string,
                                Steps = reader["steps"] as string,
                                ImageUrl = reader["image_url"] as string
                            };

                            if (recipe.Title == null)
                            {
                                Console.WriteLine($"Предупреждение: Задача {recipe.IssueId} имеет пустое поле Title.");
                            }

                            recipes.Add(recipe);
                        }
                    }
                }

                connection.Close();
            }

            return recipes;
        }

        static string SerializeRecipesToJson(List<Recipe> recipes)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.Default
            };

            return JsonConvert.SerializeObject(recipes, settings);
        }
    }
}

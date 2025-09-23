// 代码生成时间: 2025-09-23 10:52:01
using System;

namespace RandomNumberGeneratorApp
{
    public class RandomNumberGenerator
    {
        private readonly Random _random = new Random();

        // Generates a random number between min and max, inclusive.
        public int GenerateRandomNumber(int min, int max)
        {
            // Check if min is less than max to avoid ArgumentOutOfRangeException.
            if (min > max)
            {
                throw new ArgumentException("Minimum cannot be greater than maximum.", nameof(min));
            }

            // Generate random number within the specified range.
            return _random.Next(min, max + 1);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Example usage of the RandomNumberGenerator class.
                RandomNumberGenerator rng = new RandomNumberGenerator();
                int randomNumber = rng.GenerateRandomNumber(1, 100);
                Console.WriteLine($"Random Number: {randomNumber}");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the execution.
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
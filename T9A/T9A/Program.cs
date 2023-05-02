using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Зчитуємо вміст файлу зі словами
        string[] words = File.ReadAllLines("/Users/zakerden1234/Desktop/T9/T9A/T9A/words_list.txt");

        // Перетворюємо масив слів у список для зручності перевірки
        List<string> wordList = new List<string>(words);

        Console.Write("Enter a sentence: ");
        string input = Console.ReadLine();

        // Розділяємо речення на слова та видаляємо зайві символи
        char[] separators = new char[] { ' ', '.', ',', ';', ':', '!', '?' };
        string[] sentence = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        // Перевіряємо кожне слово речення на входження до списку слів
        List<string> unknownWords = new List<string>();
        foreach (string word in sentence)
        {
            if (!wordList.Contains(word.ToLower()))
            {
                unknownWords.Add(word);
            }
        }

        // Виводимо підказку, якщо знайдено невідомі слова
        if (unknownWords.Count > 0)
        {
            Console.Write("Looks like you have typos in next words: ");
            Console.WriteLine(string.Join(", ", unknownWords));
        }
        else
        {
            Console.WriteLine("All words are correct!");
        }
    }
}
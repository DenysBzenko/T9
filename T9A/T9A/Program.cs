using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    private static void Main()
    {
        // Зчитуємо вміст файлу зі словами
        string[] words = File.ReadAllLines("C:\\Users\\User\\T9\\T9A\\T9A\\words_list.txt");

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
                List<string> fiveOfferedWords = GetFiveOfferedWords(word.ToLower(), wordList);
                Console.Write($"Did you mean '{word}'? Try these words instead: ");
                Console.WriteLine(string.Join(", ", fiveOfferedWords));
                unknownWords.Add(word);
            }
        }

        // Виводимо підказку, якщо знайдено невідомі слова
        if (unknownWords.Count > 0)
        {
            Console.WriteLine($"Looks like you have typos in {unknownWords.Count} words.");
        }
        else
        {
            Console.WriteLine("All words are correct!");
        }
    }
    private static List<string> GetFiveOfferedWords(string wrongWord, List<string> glossary)
    {
        Dictionary<string, int> wordsAndValues = new Dictionary<string, int>();
        foreach (var correctWord in glossary)
        {
            bool needToBeAdded = DoesWordNeedToBeAdded(wordsAndValues, wrongWord, correctWord);
            if (needToBeAdded)
            {
                wordsAndValues[correctWord] = FindLCS(wrongWord, correctWord);
            }
        }

        // Сортуємо слова за значенням і повертаємо перші п'ять
        var sortedWords = wordsAndValues.OrderBy(x => x.Value).Select(x => x.Key).ToList();
        return sortedWords.Take(5).ToList();
    }


    private static bool DoesWordNeedToBeAdded(Dictionary<string, int> wordsAndValues, string wrongWord,
        string correctWord)
    {
        string minValueOfKey = null;
        int lcsOfTheOfferedWord = FindLCS(wrongWord, correctWord);
        if (wordsAndValues.Keys.Count < 5)
        {
            return true;
        }

        foreach (var key in wordsAndValues.Keys)
        {
            if (minValueOfKey == null || wordsAndValues[key] < wordsAndValues[minValueOfKey])
            {
                minValueOfKey = key;
            }
        }

        if (minValueOfKey != null && wordsAndValues[minValueOfKey] < lcsOfTheOfferedWord)
        {
            wordsAndValues.Remove(minValueOfKey);
            return true;
        }

        return false;
    }



    public static int FindLCS(string misstipedWord, string offeredWord)
    {

        int mistypedLen = misstipedWord.Length;
        int offeredLen = offeredWord.Length;
        int[,] tableLCS = new int[mistypedLen + 1, offeredLen + 1];



        for (int m = 1; m <= mistypedLen; m++)
        {
            for (int o = 1; o <= offeredLen; o++)
            {
                if (misstipedWord[m - 1] == offeredWord[o - 1])
                {
                    tableLCS[m, o] = tableLCS[m - 1, o - 1] + 1;
                }
                else
                {
                    tableLCS[m, o] = Math.Max(tableLCS[m - 1, o], tableLCS[m, o - 1]);
                }
            }
        }

        return tableLCS[mistypedLen, offeredLen];
    }
}

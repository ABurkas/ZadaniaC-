using DictionaryExperiment;
using System;

using System;
using System.Collections.Generic;

namespace DictionaryExperiment
{
    // Struktura reprezentująca klucz słownika
    public struct KeyStruct
    {
        // Wartość klucza
        public int Value;

        // Konstruktor struktury
        public KeyStruct(int value)
        {
            Value = value;
        }

        // Nadpisanie metody GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    // Klasa reprezentująca klucz słownika
    public class KeyClass
    {
        public int Value;

        // Nadpisanie metody GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // Nadpisanie metody Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            KeyClass other = (KeyClass)obj;
            return Value == other.Value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Testowanie różnych wariantów

            Console.WriteLine("Testowanie wariantu: Struktura z dobrym GetHashCode");
            TestDictionary<KeyStruct>();

            Console.WriteLine("\nTestowanie wariantu: Struktura z złym GetHashCode");
            TestDictionaryBadHash<KeyStruct>();

            Console.WriteLine("\nTestowanie wariantu: Klasa z dobrym GetHashCode");
            TestDictionary<KeyClass>();

            Console.WriteLine("\nTestowanie wariantu: Klasa z brakiem GetHashCode");
            TestDictionaryNoHash<KeyClass>();

            Console.WriteLine("\nTestowanie wariantu: Klasa z złym GetHashCode");
            TestDictionaryBadHash<KeyClass>();

            Console.WriteLine("\nTestowanie wariantu: Struktura z brakiem GetHashCode");
            TestDictionaryNoHash<KeyStruct>();
        }

        // Testowanie słownika z różnymi typami kluczy

        static void TestDictionary<T>()
        {
            var dictionary = new Dictionary<T, int>();

            // Dodawanie elementów do słownika
            var startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateKey<T>(i);
                dictionary[key] = i;
            }
            var endTime = DateTime.Now;
            Console.WriteLine($"Czas dodawania elementów: {(endTime - startTime).TotalMilliseconds} ms");

            // Pobieranie elementów ze słownika
            startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateKey<T>(i);
                var value = dictionary[key];
            }
            endTime = DateTime.Now;
            Console.WriteLine($"Czas pobierania elementów: {(endTime - startTime).TotalMilliseconds} ms");
        }

        // Generowanie klucza dla testu

        static T GenerateKey<T>(int value)
        {
            if (typeof(T) == typeof(KeyStruct))
                return (T)Convert.ChangeType(new KeyStruct { Value = value }, typeof(T));
            else if (typeof(T) == typeof(KeyClass))
                return (T)Convert.ChangeType(new KeyClass { Value = value }, typeof(T)); // Tworzenie nowego obiektu KeyClass dla każdej iteracji
            else
                throw new ArgumentException("Nieobsługiwany typ klucza");
        }

        // Testowanie słownika z kluczami, które mają złą implementację GetHashCode

        static void TestDictionaryBadHash<T>()
        {
            var dictionary = new Dictionary<T, int>();

            // Dodawanie elementów do słownika
            var startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateBadHashKey<T>();
                dictionary[key] = i;
            }
            var endTime = DateTime.Now;
            Console.WriteLine($"Czas dodawania elementów: {(endTime - startTime).TotalMilliseconds} ms");

            // Pobieranie elementów ze słownika
            startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateBadHashKey<T>();
                var value = dictionary[key];
            }
            endTime = DateTime.Now;
            Console.WriteLine($"Czas pobierania elementów: {(endTime - startTime).TotalMilliseconds} ms");
        }

        // Generowanie klucza z niepoprawną implementacją GetHashCode

        static T GenerateBadHashKey<T>()
        {
            if (typeof(T) == typeof(KeyStruct))
                return (T)Convert.ChangeType(new KeyStruct { Value = 0 }, typeof(T));
            else if (typeof(T) == typeof(KeyClass))
                return (T)Convert.ChangeType(new KeyClass { Value = 0 }, typeof(T));
            else
                throw new ArgumentException("Nieobsługiwany typ klucza");
        }

        // Testowanie słownika z kluczami, które nie implementują GetHashCode

        static void TestDictionaryNoHash<T>()
        {
            var dictionary = new Dictionary<T, int>();

            // Dodawanie elementów do słownika
            var startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateNoHashKey<T>();
                dictionary[key] = i;
            }
            var endTime = DateTime.Now;
            Console.WriteLine($"Czas dodawania elementów: {(endTime - startTime).TotalMilliseconds} ms");

            // Pobieranie elementów ze słownika
            startTime = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var key = GenerateNoHashKey<T>();
                var value = dictionary[key];
            }
            endTime = DateTime.Now;
            Console.WriteLine($"Czas pobierania elementów: {(endTime - startTime).TotalMilliseconds} ms");
        }

        // Generowanie klucza bez implementacji GetHashCode

        static T GenerateNoHashKey<T>()
        {
            if (typeof(T) == typeof(KeyStruct))
                return (T)Convert.ChangeType(new KeyStruct { Value = 0 }, typeof(T));
            else if (typeof(T) == typeof(KeyClass))
                return (T)Convert.ChangeType(new KeyClass { Value = 0 }, typeof(T));
            else
                throw new ArgumentException("Nieobsługiwany typ klucza");
        }
    }
}
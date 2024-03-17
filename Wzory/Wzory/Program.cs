using System;

class Liczenie
{
    static void Main()
    {
        Console.WriteLine("Podaj pierwszą liczbę:");
        int a = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Podaj drugą liczbę:");
        int b = Convert.ToInt32(Console.ReadLine());


        int suma = a + b;
        Console.WriteLine("Suma: " + suma);


        int roznica = a - b;
        Console.WriteLine("Różnica: " + roznica);

        Console.ReadLine();

    }
}
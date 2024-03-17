using System;

class Liczenie
{   int a = 3;
    int b = 4;

    public int Dodawanie(int a, int b)
    {
      
        int suma = a + b;
        Console.WriteLine("Suma: " + suma);
        return suma;
    }

    int c = 6;
    int d = 1;
    public int Roznica(int c, int d)
    {
        
        int roznica = c - d;
        Console.WriteLine("Różnica: " + roznica);
        return roznica;
    }
     
    public static void Main ()
    {
        Console.WriteLine("Huj");
    }
 }

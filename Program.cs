using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kółko_i_krzyżyk
{
    class Program
    {
        static int ScenariuszBlok;
        static int ScenariuszPC;
        static char znak;
        static int AktualnyGracz = -1; //X=1 O=-1
        static int[,] Pola = new int[3, 3]; //0-puste 1=X -1=O
        static List<int> WynikiGry = new List<int>();
        static int[] Wyniki = new int[3];
        static int[] Licznik = new int[9]; //indeksy 0-2 licznik zywyciestw w wierszach indeksy 3-5 licznik zywyciestw w kolumnach 
        //indeksy 6 i 7 licznik zywyciestw w przekątnych indeks 8 licznik wykonanych ruchów 
        static bool Sprawdz() //funkcja sprawdzająca stan gry 
        {
            LiczCzyWygral(); //funkcja uzupelniająca tablice zwyciestw tablica jest aktualizowana przy każdej rundzie 
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 8; i++)
            {
                if (Licznik[i] == 3)
                {
                    Console.Clear();
                    Console.SetCursorPosition(9, 4);
                    Console.Write("Wygrał X");
                    WynikiGry.Add(0);
                    return false;
                }

                else if (Licznik[i] == -3)
                {
                    Console.Clear();
                    WynikiGry.Add(1);
                    Console.Write("Wygral O");
                    return false;
                }

            }
            if (Licznik[8] == 9)
            {
                Console.Clear();
                 Console.Write("Nikt nie wygral");
                WynikiGry.Add(2);
                return false;
            }
           

            //pomocnicze
/*
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.SetCursorPosition(40+j, 2+i);
                    Console.Write($"{Pola[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(40, 6+ Licznik[8]);
            for (int i = 0; i < 9; i++)
            {
                
                Console.Write($"{Licznik[i]} ");
            }

            Console.WriteLine($"scena {ScenariuszPC}");
            */
            //pomocnicze
            for (int j = 0; j < 8; j++)
            {
                Licznik[j] = 0;
            }
            Console.ResetColor();
            return true;
        }
        static void LiczCzyWygral()
        {
           
            int u = 3;
            for (int i = 0; i < 3; i++) 
            {
                for (int j = 0; j < 3; j++)
                {
                    Licznik[i] += Pola[i, j];
                }
            }
            for (int i = 0; i < 3; i++) 
            {
                for (int j = 0; j < 3; j++)
                {
                    Licznik[i + 3] += Pola[j, i];
                }
            }

            for (int i = 0; i < 3; i++) 
                Licznik[6] += Pola[i, i];

            for (int i = 0; i < 3; i++) 
            {
                u--;
                Licznik[7] += Pola[i, u];
            }

        }
        
        static void RysujPlansze()
        {
            
            Console.Write("+");

            for (int i = 0; i < 5; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");

            for (int i = 0; i < 3; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(" |");
                }
                Console.WriteLine();
                if (i != 2)
                {
                    for (int w = 0; w < 3; w++)
                    {
                        Console.Write("+-");
                    }
                    Console.WriteLine("+");

                }
            }
            Console.Write("+");

            for (int i = 0; i < 5; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
            Console.SetCursorPosition(3, 3); //srodek planszy
        }

        static void WykonajRuch() //wykonywanie ruchu na ekranie oraz przekazanie indeksow do tablicy
        {
            
            int left = 3, top = 3; 
            ConsoleKeyInfo wejscie;
            do
            {
                wejscie = Console.ReadKey(true);
                Console.SetCursorPosition(left, top);
                switch (wejscie.Key.ToString())
                {
                    case "UpArrow":
                        top -= 2;
                        if (top == -1) //ograniczenie wykroczenia po za plansze
                            top = 5;
                        break;
                    case "DownArrow":
                        top += 2;
                        if (top == 7)
                            top = 1;
                        break;
                    case "RightArrow":
                        left += 2;
                        if (left == 7)
                            left = 1;
                        break;
                    case "LeftArrow":
                        left -= 2;
                        if (left == -1)
                        {
                            left = 5;
                        }
                        break;
                }
                Console.SetCursorPosition(left, top);
            } while (wejscie.Key != ConsoleKey.Enter); 

           
                znak = 'X';
      
            int k = 0, w = 0;
            switch (left) //zamiana zmiennych okreslających pozycje kursora na indeksy tablicy 
            {
                case 1:
                    k = 0;
                    break;
                case 3:
                    k = 1;
                    break;
                case 5:
                    k = 2;
                    break;
            }
            switch (top)
            {
                case 1:
                    w = 0;
                    break;
                case 3:
                    w = 1;
                    break;
                case 5:
                    w = 2;
                    break;
            }
            Ruch(k, w, znak);

        }
        static void  RuchPC()
        { znak = 'O';
         

            if (Licznik[8] == 0)
            {
                Console.SetCursorPosition(1, 1);
                Ruch(0, 0, znak);
            }else if (Licznik[8] == 2)
            {
                if (Pola[0, 1] == 1 || Pola[1, 0] == 1 || Pola[1, 2] == 1 || Pola[2, 1] == 1)
                {
                    ScenariuszPC = 0;
                    Console.SetCursorPosition(3, 3);
                    Ruch(1, 1, znak);

                }
                else if (Pola[0, 2] == 1 || Pola[2, 0] == 1)
                {
                    ScenariuszPC = 1;
                    if(Pola[0, 2] == 1)
                    {
                        Console.SetCursorPosition(1, 5);
                        Ruch(0, 2, znak);
                    }
                    else
                    {
                        Console.SetCursorPosition(5,1);
                        Ruch(2, 0, znak);
                    }
                }
                else if(Pola[2,2] == 1)
                {
                    ScenariuszPC = 2;
                    Console.SetCursorPosition(5, 1);
                    Ruch(2, 0, znak);
                }
                else
                {
                    ScenariuszPC = 3;
                    Console.SetCursorPosition(5, 5);
                    Ruch(2, 2, znak);
                }

            }

            switch (ScenariuszPC)
            {
                case 0:
                    if (Licznik[8] == 4)
                    {
                        if (Pola[2, 2] == 0)
                        {
                            Console.SetCursorPosition(5, 5);
                            Ruch(2, 2, znak);
                        }
                        else
                        {
                            if (Pola[1, 2] == 1)
                            {
                                Console.SetCursorPosition(5, 1);
                                Ruch(2, 0, znak);
                            }
                            else
                            {
                                if (Pola[2, 2] == 1)
                                {
                                    if (Pola[1, 0] == 0 && Pola[2, 0] == 0)
                                    {
                                        Console.SetCursorPosition(1, 5);
                                        Ruch(0, 2, znak);
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(5, 1);
                                        Ruch(2, 0, znak);
                                    }
                                }

                            }
                        }

                    }
                    else if (Licznik[8] == 6)
                    {
                        if (Pola[2, 0] == 1)
                        {
                            Console.SetCursorPosition(3, 1);
                            Ruch(1, 0, znak);
                        }
                        else if (Pola[1, 2] == 1 || Pola[1, 0] == 1)
                        {
                            Console.SetCursorPosition(5,1);
                            Ruch(2,0, znak);
                        }
                        else
                        {
                            
                            if (Pola[1, 0] == 0)
                            {
                                Console.SetCursorPosition(1, 3);
                                Ruch(0, 1, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 1);
                                Ruch(0, 2, znak);
                            }
                        }
                    }
                    break;
                case 1:
                    if (Licznik[8] == 4)
                    {
                        if (Pola[0, 1] == 1 || Pola[1, 0] == 1)
                        {
                            Console.SetCursorPosition(5, 5);
                            Ruch(2, 2, znak);
                        }
                        else if (Pola[0, 1] == 0 && Pola[0, 2] == -1)
                        {
                            Console.SetCursorPosition(3, 1);
                            Ruch(1, 0, znak);
                        }
                        else
                        {
                            Console.SetCursorPosition(1, 3);
                            Ruch(0, 1, znak);
                        }
                    }
                    else if (Licznik[8] == 6)
                    {
                        if (Pola[1, 1] == 1)
                        {
                            if (Pola[2, 0] == -1 && Pola[2, 1] == 0 && Pola[2, 2] == -1)
                            {
                                Console.SetCursorPosition(3, 5);
                                Ruch(1, 2, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 3);
                                Ruch(2, 1, znak);
                            }
                        }
                        else if (Pola[1, 2] == 1)
                        {
                            Console.SetCursorPosition(3, 3);
                            Ruch(1, 1, znak);
                        }
                        else
                        {
                            Console.SetCursorPosition(3, 3);
                            Ruch(1, 1, znak);
                        }
                    }
                    break;

                case 2:
                    if (Licznik[8] == 4)
                    {
                        if (Pola[0, 1] == 1)
                        {
                            Console.SetCursorPosition(1, 5);
                            Ruch(0, 2, znak);
                        }
                        else
                        {
                            Console.SetCursorPosition(3, 1);
                            Ruch(1, 0, znak);
                        }
                    }
                    else if (Licznik[8] == 6)
                    {
                        if (Pola[1, 1] == 1)
                        {
                            Console.SetCursorPosition(1, 3);
                            Ruch(0, 1, znak);
                        }
                        else if (Pola[1, 0] == 1)
                        {
                            Console.SetCursorPosition(3, 3);
                            Ruch(1, 1, znak);
                        }
                        else
                        {
                            Console.SetCursorPosition(3, 3);
                            Ruch(1, 1, znak);
                        }
                    }
                    break;
                case 3:
                    if (Licznik[8] == 4)
                    {
                        if (Pola[0, 2] == 1 || Pola[2, 0] == 1)
                        {
                            if (Pola[0, 2] == 1)
                            {
                                Console.SetCursorPosition(1, 5);
                                Ruch(0, 2, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 1);
                                Ruch(2, 0, znak);
                            }
                        }
                        else if (Pola[0, 1] == 1 || Pola[1, 0] == 1 || Pola[1, 2] == 1 || Pola[2, 1] == 1)
                        {

                            ScenariuszPC = 4;
                        }


                    }
                    else if (Licznik[8] == 6)
                    {
                        if (Pola[0, 2] == 1)
                        {
                            if (Pola[1, 0] == 1)
                            {
                                Console.SetCursorPosition(3, 5);
                                Ruch(1, 2, znak);
                            }
                            else if (Pola[2, 1] == 1)
                            {
                                Console.SetCursorPosition(1, 3);
                                Ruch(0, 1, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(1, 3);
                                Ruch(0, 1, znak);
                            }
                        }
                        else
                        {
                            if (Pola[0, 1] == 1)
                            {
                                Console.SetCursorPosition(5, 3);
                                Ruch(2, 1, znak);
                            }
                            else if (Pola[1, 2] == 1)
                            {
                                Console.SetCursorPosition(3, 1);
                                Ruch(1, 0, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 3);
                                Ruch(2, 1, znak);
                            }
                        }

                    }
                    break;
            }
            if (ScenariuszPC == 4)
            {
              

                if (Licznik[8] == 4)
                {
                    if (Pola[1, 2] == 1)
                    {
                        Console.SetCursorPosition(1, 3);
                        Ruch(0, 1, znak);
                        ScenariuszBlok = 0;
                    }
                    else if (Pola[1, 0] == 1)
                    {
                        Console.SetCursorPosition(5, 3);
                        Ruch(2, 1, znak);
                        ScenariuszBlok = 0;

                    }
                    else if (Pola[0, 1] == 1)
                    {
                        Console.SetCursorPosition(3, 5);
                        Ruch(1, 2, znak);
                        ScenariuszBlok = 1;
                  
                    }
                    else 
                    {
                        Console.SetCursorPosition(3, 1);
                        Ruch(1,0, znak);
                        ScenariuszBlok = 1;
                        
                    }
                }
                else
                {
                    if (Licznik[8] == 6)
                    {
                        if (ScenariuszBlok == 0)
                        {
                            if (Pola[2, 0] == 0)
                            {
                                Console.SetCursorPosition(1, 5);
                                Ruch(0, 2, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 1);
                                Ruch(2, 0, znak);
                            }
                        }
                        else
                        {
                            if (Pola[0, 2] == 1)
                            {
                                Console.SetCursorPosition(1, 5);
                                Ruch(0, 2, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(5, 1);
                                Ruch(2, 0, znak);
                            }
                        }

                    }
                    else
                    {

                        if (ScenariuszBlok == 0)
                        {

                            if (Pola[0, 1] == 0)
                            {
                                Console.SetCursorPosition(3, 1);
                                Ruch(1, 0, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(3, 5);
                                Ruch(1, 2, znak);
                            }
                        }
                        else
                        {

                            if (Pola[1, 0] == 1)
                            {

                                Console.SetCursorPosition(5, 3);
                                Ruch(2, 1, znak);
                            }
                            else
                            {
                                Console.SetCursorPosition(1, 3);
                                Ruch(0, 1, znak);
                            }
                        }
                    }

                }

            }



        }
        static void Przebieg()
        {
            if (AktualnyGracz == 1)
                WykonajRuch();
            else
                RuchPC();
        }
        static void Ruch(int k, int w, char znak) //wlasciwe wykonanie ruchu sprawdaza czy miejsce jest puste jesli tak dadaje liczbe wykonanych ruchow i wstawia znak 
        {
            if (Pola[w, k] == 0)
            {
                Licznik[8]++;
                Console.Write(znak);
                if (AktualnyGracz == 1)
                    Pola[w, k] = 1;
                else
                    Pola[w, k] = -1;
            }
            else
                WykonajRuch();
        }
        static void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Pola[i, j] = 0;
                }
            }
                
            for (int j = 0; j <Licznik.Length; j++)
            {
                Licznik[j] = 0;
            }
            for (int i = 0; i < Wyniki.Length; i++)
            {
                Wyniki[i] = 0;
            }
            ScenariuszBlok=0;
            ScenariuszPC=0;
            AktualnyGracz = -1;

            Console.Clear();
            Console.ResetColor();
        }
        static void Statystyki()
        {
            foreach (var item in WynikiGry)
            {
                if (item == 0)
                    Wyniki[0]++;
                else if (item == 1)
                    Wyniki[1]++;
                else
                    Wyniki[2]++;
                
            }
            Console.Clear();
            Console.WriteLine($"Wygral X:{Wyniki[0]} \n Wygral O:{Wyniki[1]} \n Remis:{Wyniki[2]}");
            Console.Write("Graj ponownie Enter \n zakończ ESC");
        }
        static void gra() 
        {
            
                Console.WriteLine("Grasz w gre kolko i krzyzyk \n steruj strzalkami zatwierdz klawiszem enter \n wcisnij enter aby rozpoczac");
                ConsoleKeyInfo wejscie2;
                Console.ReadKey();
            do
            {
                Reset();
                RysujPlansze();
                while (Sprawdz())
                {
                    WyswietlInfo();
                    Przebieg();
                    AktualnyGracz *= -1;
                }
                Statystyki();
                wejscie2 = Console.ReadKey(true);
            }
            while (wejscie2.Key != ConsoleKey.Escape);
                
        }
        static void WyswietlInfo()
        {
            char Gracz;
            string tekst = "Aktualny Gracz to:";

            Console.SetCursorPosition((Console.WindowWidth - tekst.Length + 1) / 2, 1);
            Console.Write($"{tekst}");
            if (AktualnyGracz == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Gracz = 'X';
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Gracz = 'O';
            }
            Console.Write($"{Gracz}");
            Console.ResetColor();
            Console.SetCursorPosition(3, 3);
        }

        static void Main(string[] args)
        {
            gra();
        }
    }
}

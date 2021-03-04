using System;
using System.Collections;
using System.Collections.Generic;

namespace LFTA_Tema1
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            s = s != "" ? s : "a * (b - t / s * h) / t - x * y"; //Sirul exemplu de la seminar
            SirPolonez sir = new SirPolonez(s);
            Console.WriteLine(sir.sir);
            Expresie expresie = new Expresie(s);
            Console.WriteLine(expresie.exp);
        }
    }

    class Expresie
    {
        public string exp {get;set;}
        public Expresie(string s)
        {
            exp = creazaExpresie(s);
        }

        private string creazaExpresie(string s)
        {
            Stack<char> stivaOperatori = new Stack<char>();
            Stack<int> costOperatori = new Stack<int>();
            Stack<string> stivaOperanzi = new Stack<string>();
            string rez = "";
            int p = 0;
            foreach(var x in s)
            {
                if (x == '(') { p += 10; stivaOperanzi.Push(x.ToString());  continue; }
                if (x == ')') { p -= 10; var l = stivaOperanzi.Pop(); stivaOperanzi.Push(l+x.ToString()); continue; }
                if (x == ' ') continue;
                if (SirPolonez.isChar(x))
                {
                    try
                    {
                        if (stivaOperanzi.Peek() == "(") { stivaOperanzi.Pop(); stivaOperanzi.Push("(" + x.ToString()); }
                        else stivaOperanzi.Push(x.ToString());
                    }
                    catch
                    {
                        stivaOperanzi.Push(x.ToString());
                    }
                }
                else
                {
                    reincearca:
                    if (stivaOperatori.Count > 0)
                    {
                        if (costOperatori.Peek() < SirPolonez.cost(x))
                        {
                            stivaOperatori.Push(x);
                            costOperatori.Push(SirPolonez.cost(x)+p);
                        }
                        else
                        {
                            var vf = stivaOperatori.Pop(); costOperatori.Pop();
                            var ult = stivaOperanzi.Pop();
                            string penult = stivaOperanzi.Pop();
                            penult = penult + vf + ult;
                            stivaOperanzi.Push(penult);
                            rez = penult;
                            goto reincearca;
                        }
                    }
                    else
                    {
                        stivaOperatori.Push(x);
                        costOperatori.Push(SirPolonez.cost(x) + p);
                    }
                }
            }
            while (stivaOperatori.Count > 0) {
                var vf = stivaOperatori.Pop(); costOperatori.Pop();
                var ult = stivaOperanzi.Pop();
                string penult = stivaOperanzi.Pop();
                penult = penult + vf + ult;
                stivaOperanzi.Push(penult);
                rez = penult;
            }
            return rez;
        }
    }
   
    class SirPolonez
    {
        public string sir { get; set; }
        public SirPolonez(string s)
        {
            this.sir = creazaSir(s);
        }

        private string creazaSir(string s)
        {
            char[] rez = new char[100];
            char[] stiva = new char[50];
            int[] costs = new int[50];
            int k = 0,sk=0,p=0;
            
            foreach(var x in s)
            {
                if (x == '(') { p += 10; continue; }
                if (x == ')') { p -= 10; continue; }
                if (x == ' ') continue;
                if (isChar(x))
                    rez[k++] = x;
                else {
                    if (cost(x) > cost(stiva[sk]))
                    {
                        stiva[sk++] = x;
                        costs[sk-1] = cost(x) + p;
                    }
                    else
                    {
                        for (int i = sk; i >= 0; i--)
                        {
                            if (cost(x) + p <= costs[i])
                            {
                                rez[k++] = stiva[i];
                                stiva[i] = '\0';
                                costs[i] = 0; 
                                sk--;
                            }
                        }
                        sk++;
                        stiva[sk++] = x;
                        costs[sk - 1] = cost(x) + p;
                    }
                }
            }
            for (int i = sk; i >= 0; i--)
            { 
                rez[k++] = stiva[i];
            }
            return new string(rez);
        }

       

        public static int cost(char x)
        {
            return x switch
            {
                '+' => 2,
                '-' => 2,
                '*' => 3,
                '/' => 3,
                '\0' => 100,
                '.'  => -5,
                _ => 0,
            };
        }

        public static bool isChar(char x)
        {
            return ((int)x >= 65 && (int)x <= 122) || ((int)x>=48 && (int)x<=57);
        }
    }
}

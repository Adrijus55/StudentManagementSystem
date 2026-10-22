using System;
using System.Collections.Generic;
using System.IO;

namespace StudentManagementSystem
{
    internal class Program
    {
        const string CFd = "..\\..\\Studentai.txt";

        static void Main(string[] args)
        {
            List<Studentas> studentai;

            if (File.Exists(CFd))
            {
                studentai = SkaitytiStudentus();
            }
            else
            {
                studentai = new List<Studentas>();
            }

            string pas = "0";
            do
            {
                Console.WriteLine("1. Peržiūrėti visus studentus\n2. Ieškoti studento\n3. Pridėti naują studentą\n" +
                "4. Pridėti studentui pažymį\n5. Pašalinti studentą\n0. Išeiti");
                pas = Console.ReadLine();
                switch (pas)
                {
                    case "1":

                        PerziuretiStudentus(studentai);
                        break;
                    case "2":
                        IeskotiStudento(studentai);
                        break;
                    case "3":
                        PridetiStudenta(studentai);
                        break;
                    case "4":
                        PridetiPazymi(studentai);
                        break;
                    case "5":
                        PasalintiStudenta(studentai);
                        break;
                    case "0":
                        Console.WriteLine("Studentų valdymo programa baigiama.");
                        break;
                    default:
                        Console.WriteLine("Neteisingas pasirinkimas");
                        break;
                }
            } while (pas != "0");

            IssaugotiStudentus(studentai);

        }

        static List<Studentas> RastiStudenta(List<Studentas> studentai, string vardas, string pavarde)
        {
            List<Studentas> rasti = new List<Studentas>();
            foreach (Studentas studentas in studentai)
            {
                if (studentas.Vardas.Equals(vardas, StringComparison.OrdinalIgnoreCase) && studentas.Pavarde.Equals(pavarde, StringComparison.OrdinalIgnoreCase))
                {
                    rasti.Add(studentas);
                }

            }
            return rasti;
        }
        static List<Studentas> SkaitytiStudentus()
        {
            List<Studentas> studentai = new List<Studentas>();

            string[] eilutes = File.ReadAllLines(CFd);

            foreach (string eilute in eilutes)
            {
                string[] dalys = eilute.Split(';');
                if (dalys.Length != 3)
                {
                    continue;
                }

                string var = dalys[0];
                string pav = dalys[1];
                if (string.IsNullOrWhiteSpace(var) ||
                string.IsNullOrWhiteSpace(pav))
                {
                    continue;
                }

                Studentas S1 = new Studentas(var, pav);
                string[] pazymiai = dalys[2].Split(',');
                foreach (string pazymys in pazymiai)
                {
                    int paz;
                    bool pavyko = int.TryParse(pazymys, out paz);
                    if (pavyko && paz >= 1 && paz <= 10)
                    {
                        S1.PridetiPaz(paz);
                    }
                }
                studentai.Add(S1);
            }




            return studentai;
        }
        static Studentas PasirinktiStudenta(List<Studentas> rasti)
        {
            if (rasti.Count == 1)
            {
                return rasti[0];
            }
            else if (rasti.Count == 0)
            {
                return null;
            }
            else
            {
                int skait = 1;
                foreach (Studentas studentas in rasti)
                {
                    Console.WriteLine("Rasti studentai: {0} {1} {2}",
                        skait, studentas.Vardas, studentas.Pavarde);
                    skait++;
                }
                while (true)
                {
                    Console.WriteLine("Įveskite studento numerį: ");
                    int nr;
                    bool pavyko = int.TryParse(Console.ReadLine(), out nr);

                    if (pavyko && nr >= 1 && nr <= rasti.Count)
                    {
                        return rasti[nr - 1];

                    }
                    else Console.WriteLine("Neteisingas studento numeris!");

                }
            }
        }
        static void IeskotiStudento(List<Studentas> studentai)
        {
            Console.WriteLine("Įveskite studento vardą, kurio ieškote: ");
            string ieskomasvard = Console.ReadLine();
            Console.WriteLine("Įveskite studento pavardę, kurio ieškote: ");
            string ieskomaspav = Console.ReadLine();
            List<Studentas> rastas = RastiStudenta(studentai, ieskomasvard, ieskomaspav);
            if (rastas.Count > 0)
            {
                foreach (Studentas studentas in rastas)
                {
                    Console.WriteLine("{0}, {1}, {2}", studentas.Vardas, studentas.Pavarde, studentas.GautiVidurki());
                }
            }
            else Console.WriteLine("Studentas nerastas");
        }
        static void PridetiStudenta(List<Studentas> studentai)
        {
            Console.WriteLine("Įveskite vardą: ");
            string vardas = Console.ReadLine();
            Console.WriteLine("Įveskite pavardę: ");
            string pavarde = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(vardas) || string.IsNullOrWhiteSpace(pavarde))
            {
                Console.WriteLine("Vardas ir pavardė negali būti tušti!");
            }
            else
            {
                List<Studentas> rasti = RastiStudenta(studentai, vardas, pavarde);
                if (rasti.Count > 0)
                {
                    Console.WriteLine("Toks studentas jau egzistuoja!");
                }
                else
                {
                    Studentas naujasStudentas = new Studentas(vardas, pavarde);
                    studentai.Add(naujasStudentas);
                    Console.WriteLine("Studentas pridėtas sėkmingai!");
                }
            }
        }
        static void PridetiPazymi(List<Studentas> studentai)
        {
            Console.WriteLine("Įveskite vardą: ");
            string vardas = Console.ReadLine();
            Console.WriteLine("Įveskite pavardę: ");
            string pavarde = Console.ReadLine();
            List<Studentas> rastas = RastiStudenta(studentai, vardas, pavarde);

            int paz;

            Studentas pasirinktasStudentas = PasirinktiStudenta(rastas);
            if (pasirinktasStudentas != null)
            {
                while (true)
                {
                    Console.WriteLine("Įveskite pažymį: ");
                    bool pavyko = int.TryParse(Console.ReadLine(), out paz);
                    if (pavyko)
                    {
                        if (paz >= 1 && paz <= 10)
                        {
                            pasirinktasStudentas.PridetiPaz(paz);
                            Console.WriteLine("Pažymys pridėtas sėkmingai!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Pažymys turi būti nuo 1 iki 10!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Įveskite skaičių!");
                    }
                }
            }
            else Console.WriteLine("Studentas nerastas");
        }
        static void PasalintiStudenta(List<Studentas> studentai)
        {
            Console.WriteLine("Įveskite studento vardą: ");
            string vardas = Console.ReadLine();
            Console.WriteLine("Įveskite studento pavardę: ");
            string pavarde = Console.ReadLine();
            List<Studentas> rasti = RastiStudenta(studentai, vardas, pavarde);
            Studentas pasirinktasStudentas = PasirinktiStudenta(rasti);

            if (pasirinktasStudentas != null)
            {
                studentai.Remove(pasirinktasStudentas);
                Console.WriteLine("Studentas pašalintas sėkmingai!");
            }
            else Console.WriteLine("Studentas nerastas");
        }
        static void PerziuretiStudentus(List<Studentas> studentai)
        {
            foreach (Studentas studentas in studentai)
            {
                Console.WriteLine("{0}, {1}", studentas.Vardas, studentas.Pavarde);
                Console.WriteLine("Pažymiai: {0}", string.Join(",", studentas.Pazymiai));
                Console.WriteLine("Vidurkis: {0}", studentas.GautiVidurki());
                Console.WriteLine();
            }
        }
        static void IssaugotiStudentus(List<Studentas> studentai)
        {
            List<string> eilutes = new List<string>();
            foreach (Studentas studentas in studentai)
            {
                string eilute = studentas.Vardas + ";" + studentas.Pavarde + ";" + string.Join(",", studentas.Pazymiai);
                eilutes.Add(eilute);
            }
            File.WriteAllLines(CFd, eilutes);
        }



    }



}


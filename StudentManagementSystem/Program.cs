using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentManagementSystem
{
    internal class Program
    {
        static readonly string studentuFailoKelias = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Studentai.txt");

        static void Main(string[] args)
        {
            List<Studentas> studentai;

            if (File.Exists(studentuFailoKelias))
            {
                try
                {
                    studentai = SkaitytiStudentus();
                }
                catch (InvalidDataException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Klaida skaitant failą: " + ex.Message);
                    return;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Klaida skaitant failą: " + ex.Message);
                    return;
                }

            }
            else
            {
                studentai = new List<Studentas>();
            }

            string pasirinkimas = "0";
            do
            {
                Console.WriteLine("1. Peržiūrėti visus studentus\n2. Ieškoti studento\n3. Pridėti naują studentą\n" +
                "4. Pridėti studentui pažymį\n5. Pašalinti studentą\n6. Rūšiuoti studentus pagal vidurkį\n0. Išeiti");
                pasirinkimas = Console.ReadLine();
                switch (pasirinkimas)
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
                    case "6":
                        RusiuotiStudentus(studentai);
                        break;
                    default:
                        Console.WriteLine("Neteisingas pasirinkimas");
                        break;
                }
            } while (pasirinkimas != "0");

            try { IssaugotiStudentus(studentai); }

            catch (IOException ex)
            {
                Console.WriteLine("Klaida įrašant failą: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Klaida įrašant failą: " + ex.Message);
            }

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

            string[] eilutes = File.ReadAllLines(studentuFailoKelias);

            foreach (string eilute in eilutes)
            {
                string[] dalys = eilute.Split(';');
                if (dalys.Length != 3)
                {
                    throw new InvalidDataException("Netinkamas duomenų formatas: " + eilute);
                }

                string var = dalys[0];
                string pav = dalys[1];
                if (string.IsNullOrWhiteSpace(var) ||
                string.IsNullOrWhiteSpace(pav))
                {
                    throw new InvalidDataException("Vardas ir pavardė negali būti tušti: " + eilute);
                }

                Studentas studentas = new Studentas(var, pav);
                string[] pazymiai = dalys[2].Split(',');
                if (!string.IsNullOrWhiteSpace(dalys[2]))
                {
                    foreach (string pazymys in pazymiai)
                    {
                        int paz;
                        bool pavyko = int.TryParse(pazymys, out paz);
                        if (pavyko && paz >= 1 && paz <= 10)
                        {
                            studentas.PridetiPaz(paz);
                        }
                        else throw new InvalidDataException("Duomenų formatas netinkamas: " + eilute);
                    }
                }
                studentai.Add(studentas);
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
                int numeris = 1;
                foreach (Studentas studentas in rasti)
                {
                    Console.WriteLine("Rasti studentai: {0} {1} {2}",
                        numeris, studentas.Vardas, studentas.Pavarde);
                    numeris++;
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
            string ieskomasvard = Console.ReadLine().Trim();
            Console.WriteLine("Įveskite studento pavardę, kurio ieškote: ");
            string ieskomaspav = Console.ReadLine().Trim();
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
            string vardas = Console.ReadLine().Trim();
            Console.WriteLine("Įveskite pavardę: ");
            string pavarde = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(vardas) || string.IsNullOrWhiteSpace(pavarde) || vardas.Contains(";") || pavarde.Contains(";"))
            {
                Console.WriteLine("Vardas ir pavardė negali būti tušti arba turėti kabliataškių!");
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
            string vardas = Console.ReadLine().Trim();
            Console.WriteLine("Įveskite pavardę: ");
            string pavarde = Console.ReadLine().Trim();
            List<Studentas> rastas = RastiStudenta(studentai, vardas, pavarde);

            int paz;

            Studentas pasirinktasStudentas = PasirinktiStudenta(rastas);
            if (pasirinktasStudentas != null)
            {
                while (true)
                {
                    Console.WriteLine("Įveskite pažymį: ");
                    bool pavyko = int.TryParse(Console.ReadLine().Trim(), out paz);
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
            string vardas = Console.ReadLine().Trim();
            Console.WriteLine("Įveskite studento pavardę: ");
            string pavarde = Console.ReadLine().Trim();
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
            string laikinasFailas = studentuFailoKelias + ".tmp";
            File.WriteAllLines(laikinasFailas, eilutes);
            if (File.Exists(studentuFailoKelias))
            {
                File.Replace(laikinasFailas, studentuFailoKelias, studentuFailoKelias + ".bak");
            }
            else File.Move(laikinasFailas, studentuFailoKelias);
        }
        static void RusiuotiStudentus(List<Studentas> studentai)
        {
            if (studentai.Count == 0)
            {
                Console.WriteLine("Studentų sąrašas tuščias.");
                return;
            }
            var surikiuoti = studentai.OrderByDescending(s => s.GautiVidurki()).ToList();
            foreach (Studentas studentas in surikiuoti)
            {
                Console.WriteLine("{0} {1} — vidurkis: {2:F2}",
                studentas.Vardas, studentas.Pavarde, studentas.GautiVidurki());
            }

        }



    }



}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem
{
    internal class Studentas
    {
        private string _vardas;
        private string _pavarde;
        private List<int> _pazymiai;

        public Studentas(string vardas, string pavarde)
        {
            this._vardas = vardas;
            this._pavarde = pavarde;

            this._pazymiai = new List<int>();
        }

        public string Vardas
        {
            get { return _vardas; }
        }
        public string Pavarde
        {
            get { return _pavarde; }
        }

        public List<int> Pazymiai
        {
            get { return new List<int>(_pazymiai); }
        }

        public void PridetiPaz(int pazymys)
        {
            if (pazymys >= 1 && pazymys <= 10)
            {
                _pazymiai.Add(pazymys);
            }
            else { Console.WriteLine("Pažymys netinkamas"); }
        }
        public double GautiVidurki()
        {
            if (_pazymiai.Count == 0)
            {
                return 0;
            }
            else
            {
                return (double)_pazymiai.Sum() / _pazymiai.Count;
            }
        }
    }
}

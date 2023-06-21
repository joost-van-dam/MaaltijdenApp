using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Package
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } //Beschrijvende naam (niet leeg)
        //[AllowNull]
        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                IsOver18 = false;
                foreach (var p in Products)
                {
                    if (p.ContainsAlcohol)
                    {
                        //_isOver18 = true;
                        IsOver18 = true;
                    }
                }
            }
        }

        //} //Lijst van producten(indicatie op basis van historie)
        public City City { get; set; } //Stad (Breda, Den Bosch, Tilburg)
        public Canteen Canteen { get; set; } //Kantine (per stad zijn er meerdere kantines, mag via enumeratie)
        public DateTime PickupMoment { get; set; } // Datum en tijdstip van ophalen
        public DateTime PickupClosingTime { get; set; } //Tijdstip tot wanneer een pakket opgehaald kan worden
        //public bool _isOver18;
        //public bool IsOver18 { get => _isOver18; } //Indicatie 18+
        public bool IsOver18 { get; set; }
        [DataType("Decimal(10,2)")]
        public decimal Price { get; set; } //Prijs
        public TypeOfMeal TypeOfMeal { get; set; } //Type maaltijd (brood, warme avondmaaltijd, drank, …, mag via enumeratie)
        private Student? _reservation;
        public Student? Reservation 
        {
            get => _reservation;
            set
            {
                var student = value as Student;

                if (student != null)
                {
                    //Heeft de student reserveringen?
                    if (student.Packages != null)
                    {
                        foreach (var p in student.Packages)
                        {
                            //Is de (al bestaande) reserving op dezelfde dag als het ophaalmoment van het pakket
                            if (p.PickupMoment.Year.Equals(this.PickupMoment.Year) && p.PickupMoment.Month.Equals(this.PickupMoment.Month) && p.PickupMoment.Day.Equals(this.PickupMoment.Day))
                            {
                                throw new InvalidOperationException("The student has already reserved a package this day");
                            }
                        }
                    }

                    //Checkt of het pakket alcohol bevat
                    if (this.IsOver18)
                    {
                        //Checkt of de student 18 jaar is tijdens het ophalen van het pakket 
                        //Jonger -> getal > 0 -> mag niet reserveren
                        //Ouder -> getal < 0
                        //Precies18 -> getal = 0
                        if (DateTime.Compare(student.Birthday.AddYears(18), this.PickupMoment) > 0)
                        {
                            throw new InvalidOperationException("The student is under 18 at pickupmoment");
                        }
                    }

                    //if (this.Reservation != null)
                    //{
                    //    throw new InvalidOperationException("This package has already an other reservation");
                    //}

                    this._reservation = student;
                }
            }
        }
    }
}

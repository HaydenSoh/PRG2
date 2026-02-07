//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
// This SpecialOffer.cs is done by Zheng Yang!
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10275174_PRG2Assignment
{
    public class SpecialOffer
    {
        public string OfferCode { get; set; }
        public string OfferDesc { get; set; }
        public double Discount { get; set; }

        public SpecialOffer(string code, string desc, double discount)
        {
            OfferCode = code;
            OfferDesc = desc;
            Discount = discount;
        }

        public override string ToString()
        {
            return $"Offer Code: {OfferCode}, Offer Description: {OfferDesc}, Discount: {Discount}";
        }
    }
}

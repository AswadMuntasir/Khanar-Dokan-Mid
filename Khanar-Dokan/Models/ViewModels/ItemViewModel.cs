using Khanar_Dokan.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khanar_Dokan.Models.ViewModels
{
    public class ItemViewModel
    {
        public FoodItem Food { get; set; }

        public int Quantity { get; set; }
    }
}
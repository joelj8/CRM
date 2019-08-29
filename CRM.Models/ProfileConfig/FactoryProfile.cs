using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models.ProfileConfig
{
    public class FactoryProfile
    {
        public static TModel CreateProfile<TModel>() where TModel : new() 
            {
            return new TModel();
        }
}
}

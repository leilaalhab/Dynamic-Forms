using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Data
{
    public class DynamicFormsDatabaseSettings
    {
         public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DynamicFormsCollectionName { get; set; } = null!;
        
    }
}
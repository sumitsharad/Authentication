using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    /// <summary>
    /// JWT token authentication
    /// </summary>
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}

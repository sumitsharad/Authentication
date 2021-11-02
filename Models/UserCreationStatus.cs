﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Models
{
    /// <summary>
    /// Taking input for displaying status message 
    /// </summary>
    public class UserCreationStatus
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
    }
}

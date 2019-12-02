﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Year { get; set; }   // год рождения
    }
}
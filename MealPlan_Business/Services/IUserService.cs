﻿using MealPlan_Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Services
{
    internal interface IUserService
    {
        public User GetUserById(int userId);
    }
}

﻿using eUseControl.Domain.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eUseControl.Domain.Entities.User
{
    public class User : BaseEntity
     {
          public string Name { get; set; }
          public string Email { get; set; }
          public string Password { get; set; }
          public DateTime LoginDateTime { get; set; }
     }
}
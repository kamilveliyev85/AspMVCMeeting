﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FeedYourCat.Models.Feeders
{
    public class NewFeederModel
    {
        [Required]
        public int User_Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool Empty { get; set; }
        [Required]
        public string Schedule_Feed { get; set; }
    }
}
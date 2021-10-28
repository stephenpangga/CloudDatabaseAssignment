﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ReviewDTO
    {
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string Comments { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    public class MemberTypesEF
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}

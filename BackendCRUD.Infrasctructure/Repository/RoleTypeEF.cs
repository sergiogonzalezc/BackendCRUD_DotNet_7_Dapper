using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    public class RoleTypeEF
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
    }
}

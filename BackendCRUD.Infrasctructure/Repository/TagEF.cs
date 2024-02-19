using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    public class TagEF
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
    }
}

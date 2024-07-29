using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    [Table("MemberType", Schema = "dbo")]
    public class MemberTypesEF
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
                
        [Column("description")]
        public string Description { get; set; }
    }
}

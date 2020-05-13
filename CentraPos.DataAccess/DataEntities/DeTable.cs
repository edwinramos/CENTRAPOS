using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentraPos.DataAccess.DataEntities
{
    [Table("srTable")]
    public class DeTable : Base
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string KeyFixed { get; set; }

        public string KeyVariable { get; set; }

        public string Value { get; set; }
    }
}

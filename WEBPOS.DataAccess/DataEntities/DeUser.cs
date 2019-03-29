using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBPOS.DataAccess.DataEntities
{
    [Table("srUser")]
    public class DeUser : Base
    {
        [Key]
        public string UserCode { get; set; }
        [Required]
        public UserType UserType { get; set; }
        [Required]
        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public bool IsEditing { get; set; }
    }

    public enum UserType
    {
        ADMINISTRADOR = 0,
        CAJERO = 1,
        GERENTE = 2,
        MOVIL = 3
    }

    public enum Gender
    {
        HOMBRE = 0,
        MUJER = 1
    }
}

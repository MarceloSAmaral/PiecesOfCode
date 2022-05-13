using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.CoreObjects.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [StringLength(14, MinimumLength = 3)]
        public string UserName { get; set; } /*MaxLengh 14, AlfaNumerico*/

        public DateTime JoinDate { get; set; }
    }

}

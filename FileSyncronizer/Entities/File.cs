using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FileSyncronizer.Entities
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Stream { get; set; }
        public string Extention { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string EncryptionKey { get; set; }
        public string SharingUrl { get; set; }



    }
}

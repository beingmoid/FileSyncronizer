using FileSyncronizer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSyncronizer
{
    public class FileContext:DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) :base(options)
        {

        }
        public DbSet<File> File { get; set; }
    }
}

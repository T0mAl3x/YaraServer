using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YaraServer.Models;

namespace YaraServer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CertificateDetailsModel> Certificates { get; set; }
        public DbSet<TerminalDetailsModel> Terminals { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<ScanModel> Scans { get; set; }
        public DbSet<YaraResultModel> YaraResults { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<YaraMetaModel> YaraMetas { get; set; }
    }
}

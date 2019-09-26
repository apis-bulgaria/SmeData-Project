using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SmeData.WebApi.Tests
{
    public class DbConfig
    {
        public string BackupPath { get; set; }
        public string BinPath { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Schema { get; set; }
        public string User { get; set; }
        public static DbConfig LoadFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<DbConfig>(jsonString);
        }
    }
    public class PostGressDumpRestore
    {
        private DbConfig dbConfig;
        private string postgreBinPath;

        public PostGressDumpRestore(DbConfig dbConfig)
        {
            this.dbConfig = dbConfig;
            this.postgreBinPath = this.dbConfig.BinPath;
        }

        public void Backup()
        {
            StringBuilder args = new StringBuilder();
            args.Append($" -U {this.dbConfig.User}");
            args.Append($" -p {this.dbConfig.Port}");
            args.Append($" -d {this.dbConfig.DatabaseName}");
            args.Append(" -Fc");
            // args.Append($" -n {database.Schema}");

            var dateTimeNow = DateTime.Now;
            string backupName = $"{this.dbConfig.DatabaseName}_{dateTimeNow.ToString("ddMMyyyy_HHmmss")}.pg_dump";
            string dumpFilePath = $"\"{this.dbConfig.BackupPath}{backupName}\"";
            args.Append($" -f {dumpFilePath}");

            this.StartPgCommandLine("pg_dump", args.ToString());
        }

        public void Restore(string backupName)
        {
            StringBuilder args = new StringBuilder();
            args.Append($@"-w -C -c -d postgres -v -p {dbConfig.Port} -U {dbConfig.User} ");
            //args.Append($" -U {this.dbConfig.User}");
            //args.Append($" -p {this.dbConfig.Port}");
            //args.Append($" -d {this.dbConfig.DatabaseName}");
            //args.Append(" -a");
            // args.Append($" -n {database.Schema}");

            string dumpFilePath = $"\"{this.dbConfig.BackupPath}{backupName}\"";
            args.Append($" {dumpFilePath}");

            this.StartPgCommandLine("pg_restore", args.ToString());

        }

        private void StartPgCommandLine(string pgCommand, string arguments)
        {
            Console.WriteLine(arguments);
            ProcessStartInfo startinfo = new ProcessStartInfo
            {
                FileName = $"\"{postgreBinPath}{pgCommand}.exe\"",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startinfo))
            {
                //process.WaitForExit();
                using (StreamReader reader = process.StandardError)
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }

            
        }
    }

}

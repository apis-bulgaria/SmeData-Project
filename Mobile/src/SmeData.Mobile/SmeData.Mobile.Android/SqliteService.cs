using System;
using System.IO;
using System.Linq;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Droid;
using SmeData.Shared.Common;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteService))]
namespace SmeData.Mobile.Droid
{
    public class SqliteService : ISQLite
    {
        public SqliteService() { }

        SQLiteAsyncConnection ISQLite.GetConnection(string dbPath)
        {
            AndroidLogging logger = new AndroidLogging();

            bool isDbExistWithFiles = CopyDatabaseIfNotExists(dbPath, logger);
            var connection = new SQLiteAsyncConnection(dbPath);

            if (isDbExistWithFiles)
            {
                logger.WritoToLog($"In merger;");

                var connection2 = new SQLiteAsyncConnection(dbPath.Replace(".sqlite", @"2.sqlite"));

                logger.WritoToLog($"Connection2 with system DB is created");

                MergeDbs(connection, connection2, logger);
            }

            return connection;
        }

        private static bool CopyDatabaseIfNotExists(string dbPath, AndroidLogging logger)
        {
            //if (!File.Exists(dbPath))
            //{
            //    long fileSize = 0;

            //    using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open("database_small.sqlite")))
            //    {
            //        using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
            //        {
            //            byte[] buffer = new byte[2048];
            //            int length = 0;

            //            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
            //            {
            //                bw.Write(buffer, 0, length);
            //            }
            //        }
            //    }
            //}

            bool isDbExistWithFiles = false;

            try
            {
                logger.WritoToLog($"In Android SqliteService;");

                if (File.Exists(dbPath))
                {
                    logger.WritoToLog($"{dbPath} exist on device;");

                    long length = new FileInfo(dbPath).Length;

                    logger.WritoToLog($"database.sqlite is with length of {length};");

                    if (length <= 30000)
                    {
                        File.Delete(dbPath);
                        logger.WritoToLog($"database.sqlite is deleted;");
                    }
                    else
                    {
                        logger.WritoToLog($"database.sqlite is NOT deleted, because it is with lenght of {length};");
                        isDbExistWithFiles = true;
                        dbPath = dbPath.Replace(".sqlite", @"2.sqlite");

                        logger.WritoToLog($"database2.sqlite is created for merge");
                    }
                }
                else
                {
                    logger.WritoToLog($"database.sqlite doesn't exist on device;");
                }

                if (!File.Exists(dbPath))
                {
                    logger.WritoToLog($"In writer;");
                    long fileSize = 0;

                    using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open("database.sqlite")))
                    {
                        using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;

                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                                logger.WritoToLog($"Copying file -> {fileSize += length};");
                            }
                        }
                    }
                }

                logger.WritoToLog($"After Copying the database file;");

                if (File.Exists(dbPath))
                {
                    logger.WritoToLog($"{dbPath} exist on device;");

                    long length = new FileInfo(dbPath).Length;

                    logger.WritoToLog($"database.sqlite is with length of {length};");
                }
                else
                {
                    logger.WritoToLog($"{dbPath} doesn't exist on device");
                }
            }
            catch (Exception ex)
            {
                logger.WritoToLog($"Exeption in CopyDatabaseIfNotExists: {ex}");
            }

            return isDbExistWithFiles;
        }

        private async void MergeDbs(SQLiteAsyncConnection dbCon1, SQLiteAsyncConnection dbCon2, AndroidLogging logger)
        {
            try
            {
                var allDocFrom1 = await dbCon1.Table<DocumentModel>().ToListAsync();
                var allDocFrom2 = await dbCon2.Table<DocumentModel>().ToListAsync();

                logger.WritoToLog($"Gets all documents from Con1;");

                foreach (var doc in allDocFrom1)
                {
                    if ((await dbCon2.Table<DocumentModel>().FirstOrDefaultAsync(x => x.Identifier == doc.Identifier)) == null)
                    {
                        await dbCon2.InsertAsync(doc);
                    }
                    else
                    {
                        await dbCon2.UpdateAsync(doc);
                    }
                }

                logger.WritoToLog($"Insert/update all docs from Con1 to Con2;");

                await dbCon1.DeleteAllAsync<DocumentModel>();
                logger.WritoToLog($"Delete all docs form Con1;");

                await dbCon1.InsertAllAsync(await dbCon2.Table<DocumentModel>().ToListAsync());

                allDocFrom1 = await dbCon1.Table<DocumentModel>().ToListAsync();

                var contacts = allDocFrom1.Where(x => x.Identifier == "3bd96c0d-816f-4502-ab9a-799c4f518564").FirstOrDefault();

                string jsonDecompressed = Compression.DecompressString(contacts.JsonSmeDoc).Replace("Piazza di Monte Citorio, 121, 00186 Roma", "Piazza Venezia, 11, 00187 Roma");

                contacts.JsonSmeDoc = Compression.CompressString(jsonDecompressed);

                if (contacts != null)
                {
                    if ((await dbCon1.Table<DocumentModel>().FirstOrDefaultAsync(x => x.Identifier == contacts.Identifier)) == null)
                    {
                        await dbCon1.InsertAsync(contacts);
                    }
                    else
                    {
                        await dbCon1.UpdateAsync(contacts);
                    }
                }

                allDocFrom1 = await dbCon1.Table<DocumentModel>().ToListAsync();

                logger.WritoToLog($"Insert all docs form Con2 to Con1;");
                logger.WritoToLog($"Merge completed!");

                await dbCon2.CloseAsync();
                File.Delete(dbCon2.DatabasePath);
            }
            catch (Exception ex)
            {
                logger.WritoToLog($"Exeption in Merger: {ex}");
            }
        }
    }
}
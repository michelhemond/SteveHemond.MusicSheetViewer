using SteveHemond.MusicSheetViewer.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;

namespace SteveHemond.MusicSheetViewer.Services
{
    public class PartitionService
    {
        public bool PartitionExists(string fileName)
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                return dbContext.Partitions.Any(p => p.FileName == fileNameWithoutExtension);
            }
        }

        public void AddPartition(Partition partition)
        {
            try
            {
                using (var dbContext = new MusicSheetViewerContext())
                {
                    dbContext.Partitions.Add(partition);
                    dbContext.SaveChanges();
                }
            }
            catch(DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                    }
                }
            }
        }

        public async Task<List<Partition>> GetPartitions()
        {
            using (var dbContext = new MusicSheetViewerContext())
            {
                return await dbContext.Partitions.ToListAsync();
            }
        }
    }
}
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DataExchange.EntityModel;
using DataExchange.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SharedResources.Contracts;

namespace DataExchange
{
    public class DataManager : IDisposable
    {
        public AlbumRepository Albums { get; set; }

        public AppDbContext Context => _context;

        public ErrorLogRepository ErrorLogs { get; private set; }
        public EventLogRepository EventLogs { get; private set; }
        public GalleryRepository Gallery { get; private set; }
        public GenderRepository Genders { get; private set; }
        public ImageRepository Images { get; private set; }
        public MenuRepository Menus { get; private set; }
        public TownRepository Towns { get; private set; }

        public DataManager(AppDbContext context)
        {
            _context = context;
            Albums = new AlbumRepository(context);
            ErrorLogs = new ErrorLogRepository(context);
            EventLogs = new EventLogRepository(context);
            Genders = new GenderRepository(context);
            Images = new ImageRepository(context);
            Menus = new MenuRepository(context);
            Towns = new TownRepository(context);
            Gallery = new GalleryRepository(context);
        }

        public void Dispose()
        {
            SaveChanges();
            _context.Dispose();
        }

        private readonly AppDbContext _context;

        private int SaveChanges()
        {
            var resultCode = 0;
            try
            {
                resultCode = _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var message = string.Empty;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity,
                            validationError.ErrorMessage);
                    }
                }
                throw new InvalidOperationException(message, ex);
            }

            return resultCode;
        }
    }
}
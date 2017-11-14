using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataExchange.EntityModel;
using SharedResources.Contracts;
using SharedResources.Views;

namespace DataExchange.Repositories
{
    public class GalleryRepository : Repository<AppDbContext, Album>
    {
        public GalleryRepository(AppDbContext context) : base(context)
        {
        }

        public List<VwGallery> GetGallerys()
        {
            var list = ListAll();
            var result = list.Select(i => new VwGallery
            {
                AlbumId = i.ID,
                UserId = i.UserId,
                Image = i.Images.Select(j => new VwImage()
                {
                    ID = j.ID,
                    Description = j.Description,
                    Title = j.Title,
                    Path = j.Path,
                    CreationDate = j.CreationDate,
                    ModifiedOn = j.ModifiedOn
                }).FirstOrDefault()
            });
            return result.ToList();
        }

        public List<VwGallery> GetUserAlbums(string userId)
        {
            var list = ListAll();
            var gug = list.Where(g => g.UserId == userId).ToList();

            var result = gug.Select(i => new VwGallery
            {
                Album = new VwAlbum()
                {
                    ID = i.ID,
                    Title = i.Title,
                    Description = i.Description,
                    CreationDate = i.CreationDate,
                    ModifiedOn = i.ModifiedOn,
                    UserId = i.UserId
                },
                UserId = i.UserId,
                Image = i.Images.Select(j => new VwImage()
                {
                    ID = j.ID,
                    Description = j.Description,
                    Title = j.Title,
                    Path = j.Path,
                    ThumbNail = j.ThumbNail,
                    CreationDate = j.CreationDate,
                    ModifiedOn = j.ModifiedOn
                }).FirstOrDefault()
            });

            return result.ToList();
        }
    }
}
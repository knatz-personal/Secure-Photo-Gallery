using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResources.Views
{
    public class VwAlbum
    {
        public System.DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
    }
}
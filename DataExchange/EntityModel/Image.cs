//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataExchange.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string ThumbNail { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string Signature { get; set; }
        public int AlbumId { get; set; }
        public string ESecretKey { get; set; }
        public string EncryptedIV { get; set; }
    
        public virtual Album Album { get; set; }
    }
}

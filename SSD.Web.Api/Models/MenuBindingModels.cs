using System;
using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Api.Models
{
    /// <summary>
    ///     Binding model for menu data objects 
    /// </summary>
    public class MenuListBindingModel
    {
        /// <summary>
        ///     Gets or sets the action name. 
        /// </summary>
        /// <value>
        ///     The action name. 
        /// </value>
        [Required]
        public string Action { get; set; }

        /// <summary>
        ///     Gets or sets the controller name. 
        /// </summary>
        /// <value>
        ///     The controller name. 
        /// </value>
        [Required]
        public string Controller { get; set; }

        /// <summary>
        ///     Gets or sets the description of the menu item. 
        /// </summary>
        /// <value>
        ///     The description of the menu item. 
        /// </value>
        [Required]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier. 
        /// </summary>
        /// <value>
        ///     The identifier. 
        /// </value>
        [Required]
        public int ID { get; set; }

        /// <summary>
        ///     Gets or sets the friendly name of the menu item. 
        /// </summary>
        /// <value>
        ///     The friendly name of the menu item. 
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the parent menu item's unique identifier. 
        /// </summary>
        /// <value>
        ///     The parent menu item's unique identifier. 
        /// </value>
        [Required]
        public int? ParentID { get; set; }

        /// <summary>
        ///     Gets or sets the sort order. 
        /// </summary>
        /// <value>
        ///     The sort order. 
        /// </value>
        [Required]
        public int? SortOrder { get; set; }

        /// <summary>
        ///     Gets or sets the URL for external links. 
        /// </summary>
        /// <value>
        ///     The external URL. 
        /// </value>
        public string Url { get; set; }
    }
}
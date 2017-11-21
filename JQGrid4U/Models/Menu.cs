using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JQGrid4U.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string MenuText { get; set; }
        public int? ParentId { get; set; }
        public bool Active { get; set; }
        public string Link { get; set; }
        public List<Menu> List { get; set; }
    }
}
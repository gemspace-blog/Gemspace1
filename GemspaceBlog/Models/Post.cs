//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GemspaceBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Category { get; set; }
        public int ReadTime { get; set; }
        public string Img1Path { get; set; }
        public string Img2Path { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}

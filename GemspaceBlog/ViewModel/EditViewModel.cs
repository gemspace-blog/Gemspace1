using GemspaceBlog.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GemspaceBlog.ViewModel
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [DisplayName("Post Title")]
        [Required(ErrorMessage = "Post title is required")]
        [StringLength(50)]
        public string Title { get; set; }
        [DisplayName("Short Description")]
        [Required(ErrorMessage = "Short Description is required")]
        [StringLength(250)]
        public string ShortDescription { get; set; }
        [DisplayName("Long Description")]
        [Required(ErrorMessage = "Long Description is required")]
        public string LongDescription { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }
        [DisplayName("Time To Read")]
        [Required(ErrorMessage = "Time To Read is required")]
        public int ReadTime { get; set; }
        [DisplayName("Small Image")]
        public string Img1Path { get; set; }
        [DisplayName("Main Image")]
        public string Img2Path { get; set; }
        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }
        public HttpPostedFileBase Image1File { get; set; }
        public HttpPostedFileBase Image2File { get; set; }
    }
}
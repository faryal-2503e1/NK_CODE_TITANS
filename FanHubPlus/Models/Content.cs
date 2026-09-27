using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Content
{
    [Key]
    public int ContentId { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required, MaxLength(50)]
    public string Type { get; set; } // article, video, audio, image

    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PopularityScore { get; set; }

    [ValidateNever]
    public ICollection<ContentTag> ContentTags { get; set; }
    [ValidateNever]
    public ICollection<Bookmark> Bookmarks { get; set; }
    [ValidateNever]
    public ICollection<ContentViews> ContentViews { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

[Table("images")]
public class Image
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("name")] public string Name { get; set; }
    [Column("content_type")] public ImageContentType ContentType { get; set; }
    [Column("data")] public byte[] Data { get; set; }
    [Column("created_at")] public DateTime CreatedAt { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SproutVRSchool.Domain.Entities.SystemSettings;
[Table("SystemSettings")]
public class SystemSetting
{
    [Key] 
    [MaxLength(100)]
    public string Key { get; set; } 

    public string Value { get; set; } 

    [MaxLength(255)]
    public string Description { get; set; }
}

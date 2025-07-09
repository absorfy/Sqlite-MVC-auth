using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyMVCAppAuth.Entities;

public class UserProfileEntity
{
    [Key, ForeignKey(nameof(User))]
    public string UserId { get; set; }
    
    public virtual IdentityUser User { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}

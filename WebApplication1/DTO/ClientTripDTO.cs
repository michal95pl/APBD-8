using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WebApplication1.Dto;

public class ClientTripDto
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Telephone { get; set; }
    [Required]
    public string? Pesel { get; set; }
    [Required]
    public string? TripName { get; set; }
    
    public DateTime? PaymentDate { get; set; }


}
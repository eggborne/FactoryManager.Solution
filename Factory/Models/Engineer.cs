using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Engineer
  {
    public int EngineerId { get; set; }

    [Required(ErrorMessage = "First name can't be empty!")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name can't be empty!")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Phone number can't be empty!")]
    [StringLength(maximumLength: 14)]
    [Phone(ErrorMessage = "Must be a valid phone number!")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Hourly rate can't be empty!")]
    [Range(20, int.MaxValue, ErrorMessage = "Hourly rate must be at least 20!")]
    public int HourlyRate { get; set; }
    public List<EngineerMachine> CertificationPartners { get; }
    public List<EngineerAvailableDay> AvailableDays { get; }
  }
}
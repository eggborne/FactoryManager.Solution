using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Machine
  {
    public int MachineId { get; set; }
    
    [Required(ErrorMessage = "Manufacturer can't be empty!")]
    public string Manufacturer { get; set; }
    
    [Required(ErrorMessage = "Model can't be empty!")]
    public string Model { get; set; }
    
    [Required(ErrorMessage = "Description can't be empty!")]
    public string Description { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}", ApplyFormatInEditMode = false)]
    [Required(ErrorMessage = "Date Of Purchase can't be empty!")]
    public DateTime PurchaseDate { get; set; }
    
    [Required(ErrorMessage = "Status can't be empty!")]
    public string Status { get; set; }
    public List<EngineerMachine> CertificationPartners { get; }
  }
}
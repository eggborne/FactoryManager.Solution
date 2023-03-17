using System.Collections.Generic;
using System;

namespace Factory.Models
{
  public class Engineer
  {
    public int EngineerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public int HourlyRate { get; set; }
    public List<EngineerMachine> JoinEntities { get; }
  }
}
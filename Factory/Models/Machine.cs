using System.Collections.Generic;
using System;

namespace Factory.Models
{
  public class Machine
  {
    public int MachineId { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public List<EngineerMachine> JoinEntities { get; }
  }
}
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    // UTILITY

    private Dictionary<string, Dictionary<string, string>> GetStartEndDictFromStringList(List<string> stringList) {
      Dictionary<string, Dictionary<string, string>> availableDays = new Dictionary<string, Dictionary<string, string>>{ };
      List<string> dayNames = new List<string>{"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
      for (var i = 0; i < stringList.Count; i += 2) {
        availableDays.Add(key: dayNames[i / 2], value: new Dictionary<string, string> { { "StartTime", stringList[i] }, { "EndTime", stringList[(i + 1)] } });
      }

      return availableDays;
    }

    private void SaveAvailableDays(Engineer engineer, Dictionary<string, Dictionary<string, string>> availableDays) {
      foreach (var day in availableDays)
        {
          string dayOfWeek = day.Key;
          
          DateTime startTime = DateTime.Parse(day.Value["StartTime"]);
          DateTime endTime = DateTime.Parse(day.Value["EndTime"]);
          EngineerAvailableDay newAvailableDay = new EngineerAvailableDay() { EngineerId = engineer.EngineerId, DayOfWeek = dayOfWeek, StartTime = startTime, EndTime = endTime }; 
          #nullable enable
          EngineerAvailableDay? existingDayData = _db.EngineerAvailableDays.FirstOrDefault(join => (join.EngineerId == engineer.EngineerId && join.DayOfWeek == dayOfWeek));
          #nullable disable
          if (existingDayData != null) {
            _db.EngineerAvailableDays.Remove(existingDayData);
            newAvailableDay.EngineerAvailableDayId = existingDayData.EngineerAvailableDayId;
          }
          _db.EngineerAvailableDays.Add(newAvailableDay);
        }
        _db.SaveChanges();
    }

    // CREATE

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer, string MondayStartTime, string MondayEndTime, string TuesdayStartTime, string TuesdayEndTime, string WednesdayStartTime, string WednesdayEndTime, string ThursdayStartTime, string ThursdayEndTime, string FridayStartTime, string FridayEndTime, string SaturdayStartTime, string SaturdayEndTime, string SundayStartTime, string SundayEndTime)
    {
      if (!ModelState.IsValid)
      {
          return View();
      }
      else
      {
        _db.Engineers.Add(engineer);
        _db.SaveChanges();

        List<string> dayStringList = new List<string>{ MondayStartTime, MondayEndTime, TuesdayStartTime, TuesdayEndTime, WednesdayStartTime, WednesdayEndTime, ThursdayStartTime, ThursdayEndTime, FridayStartTime, FridayEndTime, SaturdayStartTime, SaturdayEndTime, SundayStartTime, SundayEndTime };
        Dictionary<string, Dictionary<string, string>> availableDays = GetStartEndDictFromStringList(dayStringList);
        
        SaveAvailableDays(engineer, availableDays);

        return RedirectToAction("Index");
      }
    }

    // READ

    public ActionResult Index()
    {
      return View(_db.Engineers.ToList());
    }

    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
                              .Include(engineer => engineer.CertificationPartners)
                              .ThenInclude(join => join.Machine)
                              .Include(engineer => engineer.AvailableDays)
                              .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    // UPDATE

    public ActionResult Edit(int id)
    {
      Engineer thisEngineer = _db.Engineers
                              .Include(engineer => engineer.CertificationPartners)
                              .ThenInclude(join => join.Machine)
                              .Include(engineer => engineer.AvailableDays)
                              .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer, string MondayStartTime, string MondayEndTime, string TuesdayStartTime, string TuesdayEndTime, string WednesdayStartTime, string WednesdayEndTime, string ThursdayStartTime, string ThursdayEndTime, string FridayStartTime, string FridayEndTime, string SaturdayStartTime, string SaturdayEndTime, string SundayStartTime, string SundayEndTime)
    {
      if (!ModelState.IsValid)
      {
          return View(engineer);
      }
      else
      {
        _db.Engineers.Update(engineer);
        _db.SaveChanges();

        // Dictionary<string, Dictionary<string, string>> availableDays = new Dictionary<string, Dictionary<string, string>>{ 
        //   { "Monday", new Dictionary<string, string> { { "StartTime", MondayStartTime }, { "EndTime", MondayEndTime } } },
        //   { "Tuesday", new Dictionary<string, string> { { "StartTime", TuesdayStartTime }, { "EndTime", TuesdayEndTime } } },
        //   { "Wednesday", new Dictionary<string, string> { { "StartTime", WednesdayStartTime }, { "EndTime", WednesdayEndTime } } },
        //   { "Thursday", new Dictionary<string, string> { { "StartTime", ThursdayStartTime }, { "EndTime", ThursdayEndTime } } },
        //   { "Friday", new Dictionary<string, string> { { "StartTime", FridayStartTime }, { "EndTime", FridayEndTime } } },
        //   { "Saturday", new Dictionary<string, string> { { "StartTime", SaturdayStartTime }, { "EndTime", SaturdayEndTime } } },
        //   { "Sunday", new Dictionary<string, string> { { "StartTime", SundayStartTime }, { "EndTime", SundayEndTime } } }
        // };
        
        List<string> dayStringList = new List<string>{ MondayStartTime, MondayEndTime, TuesdayStartTime, TuesdayEndTime, WednesdayStartTime, WednesdayEndTime, ThursdayStartTime, ThursdayEndTime, FridayStartTime, FridayEndTime, SaturdayStartTime, SaturdayEndTime, SundayStartTime, SundayEndTime };
        
        Dictionary<string, Dictionary<string, string>> availableDays = GetStartEndDictFromStringList(dayStringList);
        
        SaveAvailableDays(engineer, availableDays);
        
        return RedirectToAction(actionName: "Details", new { id = engineer.EngineerId });
      }
    }

    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Model");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int machineId)
    {
      #nullable enable
      EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.MachineId == machineId && join.EngineerId == engineer.EngineerId));
      #nullable disable
      if (joinEntity == null && machineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { MachineId = machineId, EngineerId = engineer.EngineerId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = engineer.EngineerId });
    }

    // DELETE

    public ActionResult Delete(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId, int engineerId)
    {
      EngineerMachine joinEntry = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
      _db.EngineerMachines.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = engineerId });
    }
  }
}
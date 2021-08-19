using Dynamic_User_Defined_Dashboards.Models;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace EnergyAxis.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IContextHelper _context;
        private readonly Dashboard_TutorialContext db;

        public DashboardController(IContextHelper context, Dashboard_TutorialContext dsbContext)
        {
            _context = context;
            db = dsbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TemplateList()
        {
            return View("Templates/TemplateList");
        }

        public IActionResult Dashboard(int id)
        {
            DashboardsInfo dashboard = db.DashboardsInfo.Where(s => s.Id == id).FirstOrDefault();
            int elementsCount = (int)db.Templates.Where(s => s.Id == dashboard.TemplateId).Select(s => s.ElementsCount).FirstOrDefault();

            var linked_elements = db.DashboardLinkedElements.Where(s => s.DashboardId == id).ToList();
            for (int i = 1; i <= elementsCount; i++)
            {
                var element = linked_elements.Where(s => s.Placement == i.ToString());
                if (element.Any())
                {
                    ViewData["Element" + i] = "../Elements/Element" + element.First().ElementId + ".cshtml";
                }
                else
                {
                    ViewData["Element" + i] = "../Elements/Default.cshtml";
                }
            }

            ViewData["dashboardId"] = id;
            return View("Templates/Template" + dashboard.TemplateId);
        }

        public IActionResult WidgetsList()
        {
            var Widgets = db.WidgetStructure.ToList();

            List<Widget> widgetsInfo = new List<Widget>();

            foreach (var widg in Widgets)
            {
                if (widg.ClassType == typeof(TileCard1).ToString())
                {
                    TileCard1 w = (TileCard1)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;

                    var result = db.RawSqlQuery(
                        ((TileCard1)w).Query,
                        x => new TileCard1() { Value = Convert.ToDecimal(x[0]), PerformanceValue = Convert.ToDecimal(x[1]) }
                        );
                    w.Value = result.FirstOrDefault().Value;
                    w.PerformanceValue = result.FirstOrDefault().PerformanceValue;

                    widgetsInfo.Add(w);
                }
                else if (widg.ClassType == typeof(TileCard2).ToString())
                {
                    TileCard2 w = (TileCard2)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;

                    var result = db.RawSqlQuery(
                        ((TileCard2)w).Query,
                        x => new TileCard2() { Count = Convert.ToDecimal(x[0]), Amount = Convert.ToDecimal(x[1]) }
                        );
                    w.Count = result.FirstOrDefault().Count;
                    w.Amount = result.FirstOrDefault().Amount;

                    widgetsInfo.Add(w);
                }
            }
            return View("Elements/WidgetsList", widgetsInfo);
        }

        public string Createdashboard(DashboardsInfo dashboard)
        {
            try
            {
                db.DashboardsInfo.Add(dashboard);
                db.SaveChanges();

                return dashboard.Id.ToString();
            }
            catch (System.Exception e)
            {
                return "False";
            }
        }


        public IActionResult ElementList(int id)
        {

            ViewData["dashboardId"] = id;
            return View("Elements/ElementList");
        }


        public IActionResult CreateWidget(int TemplatedID)
        {

            var viewsNames = db.GetTableAndColumns();

            Widget tileCard = null;

            if (TemplatedID == 7)
            {

                tileCard = new TileCard1()
                {
                    ElementTemplateID = TemplatedID,
                    TableAndColumns = viewsNames,
                    RequiredCaptureValues = true
                };
            }
            else if (TemplatedID == 6)
            {

                tileCard = new TileCard2()
                {
                    ElementTemplateID = TemplatedID,
                    TableAndColumns = viewsNames,
                    RequiredCaptureValues = true
                };
            }
            return View("Widget/Index", tileCard);
        }

        [HttpPost]
        public IActionResult SaveTileCard1(TileCard1 card)
        {
            var enteredInfo = card;


            WidgetStructure obj = new WidgetStructure()
            {
                ElementID = card.ElementTemplateID,
                Formation = JsonSerializer.Serialize<TileCard1>(card),
                ClassType = card.GetType().ToString(),
                IsDeActivated = false
            };

            try
            {
                db.WidgetStructure.Add(obj);
                db.SaveChanges();

                var WidgID = obj.ID;
            }
            catch (System.Exception e)
            {

            }

            ViewData["dashboardId"] = 1;
            return RedirectToAction("WidgetsList");
        }
        [HttpPost]
        public IActionResult SaveTileCard2(TileCard2 card)
        {
            var enteredInfo = card;


            WidgetStructure obj = new WidgetStructure()
            {
                ElementID = card.ElementTemplateID,
                Formation = JsonSerializer.Serialize<TileCard2>(card),
                ClassType = card.GetType().ToString(),
                IsDeActivated = false
            };

            try
            {
                db.WidgetStructure.Add(obj);
                db.SaveChanges();

                var WidgID = obj.ID;
            }
            catch (System.Exception e)
            {

            }

            ViewData["dashboardId"] = 1;
            return RedirectToAction("WidgetsList");
        }


        public JsonResult GetColumns(string TableName)
        {
            var columns = db.GetColumnNames(TableName);
            return Json(new SelectList(columns));
        }

        public string AddElement(DashboardLinkedElements element)
        {

            var old = db.DashboardLinkedElements.Where(s => s.DashboardId == element.DashboardId && s.Placement == element.Placement).ToList();
            foreach (var item in old)
            {
                db.DashboardLinkedElements.Remove(item);
            }
            db.SaveChanges();

            try
            {
                db.DashboardLinkedElements.Add(element);
                db.SaveChanges();
                return "True";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public ActionResult GetDashboardsList()
        {
            return Ok(db.DashboardsInfo.ToList());
        }
    }
}

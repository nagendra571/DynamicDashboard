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

        public IActionResult WidgetsSelection(string DashboardName)
        {
            var widgets = getWidgetList();

            widgets.ForEach(m => m.WidgetSelection = true);
            widgets.ForEach(m => m.IsDragDropEnabled = true);

            ViewBag.Dashboardname = DashboardName;

            return View("Elements/EditDashboard", widgets);
        }

        public IActionResult Dashboard(int id)
        {
            DashboardsInfo board = db.DashboardsInfo.Where(m => m.Id == id).FirstOrDefault();
            List<DashboardLinkedElements> widgetListByDashboard = db.DashboardLinkedElements.Where(m => m.DashboardId == id).ToList();

            List<Widget> widgets = new List<Widget>();

            foreach (var widg in widgetListByDashboard)
            {
                widgets.AddRange(getWidgetListByID(widg.WidgetID));
            }
            widgets.ForEach(m => m.DashboardID = id);


            ViewBag.Dashboardname = board.Name;

            return View("Elements/WidgetsList", widgets);
        }

        public IActionResult WidgetsList()
        {
            return View("Elements/WidgetsList", getWidgetList());
        }

        public string Createdashboard(DashboardViewModel dashboard)
        {
            try
            {
                int BoardID = CreateDashboard(dashboard);
                CreateDashboardElements(dashboard, BoardID);

                return BoardID.ToString();
            }
            catch (System.Exception e)
            {
                return "False";
            }
        }

        public IActionResult EditDashboard(int id)
        {
            List<DashboardLinkedElements> widgetListByDashboard = null;
            if (id > 0)
            {
                DashboardsInfo board = db.DashboardsInfo.Where(m => m.Id == id).FirstOrDefault();
                widgetListByDashboard = db.DashboardLinkedElements.Where(m => m.DashboardId == id).ToList();
                ViewBag.Dashboardname = board.Name;
            }

            List<Widget> widgets = getWidgetList();
            if (id > 0)
            {
                foreach (var widg in widgetListByDashboard)
                {
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().IsAccessble = true;
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().DashboardID = id;
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().IsDefaulted = widg.IsDefaultElement;
                }
            }

            widgets.ForEach(m => m.IsDragDropEnabled = true);

            return View("Elements/EditDashboard", widgets);
        }


        [HttpGet]
        public IActionResult MapDashboards()
        {
            List<DashboardsInfo> AllDashboards = db.DashboardsInfo.ToList();

            var roles = db.BusinessRole.ToList();
            roles.Add(new BusinessRole() { ID = 0, BusinessRoleName = "" });
            roles = roles.OrderBy(m => m.ID).ToList();

            ViewBag.Roles = roles;
            return View("Elements/MapDashboards", AllDashboards);
        }

        [HttpPost]
        public string MapDashboards(BusinessRoleDashboardMapping mapping)
        {
            var SelectedDashboards = mapping.SelectedDashboardIds.Split(',').Select(Int32.Parse).ToList<int>();

            try
            {
                if (SelectedDashboards != null && SelectedDashboards.Count() > 0)
                {
                    foreach (var board in SelectedDashboards)
                    {
                        db.BusinessRoleDashboardMapping.Add(new BusinessRoleDashboardMapping()
                        {
                            RoleId = mapping.RoleId,
                            DashboardId = board
                        });
                        db.SaveChanges();
                    }
                }
            }
            catch (System.Exception e)
            {
                return "False";
            }

            return mapping.RoleId.ToString();
        }

        public string UpdateDashboard(DashboardViewModel dashboard)
        {
            try
            {
                int BoardID = dashboard.Id;

                if (BoardID > 0)
                {
                    UpdateDashboardName(dashboard);

                    DeleteDashboardElements(BoardID);
                }
                else
                {
                    BoardID = CreateDashboard(dashboard);
                }

                CreateDashboardElements(dashboard, BoardID);

                return BoardID.ToString();
            }
            catch (System.Exception e)
            {
                return "False";
            }
        }

        public string CreatedashboardWithTemplate(DashboardsInfo dashboard)
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

        public IActionResult SelectDashboard()
        {
            Dictionary<int, string> Dashboards = new Dictionary<int, string>();

            var boards = db.DashboardsInfo.ToList();
            foreach (var board in boards)
            {
                Dashboards.Add(board.Id, board.Name);
            }

            return View("Elements/SelectDashboard", Dashboards);
        }

        [HttpPost]
        public IActionResult SaveTileCard1(TileCard1 card)
        {
            var enteredInfo = card;


            WidgetStructure obj = new WidgetStructure()
            {
                ElementID = card.ElementTemplateID,
                Formation = JsonSerializer.Serialize<TileCard1>(card),
                ClassType = card.GetType().AssemblyQualifiedName,
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

        public ActionResult GetBusinessRolesList()
        {
            return Ok(db.BusinessRole.ToList());
        }

        public ActionResult GetDashboardsByRole(int RoleId)
        {
            var boards = from map in db.BusinessRoleDashboardMapping
                         join role in db.BusinessRole on map.RoleId equals role.ID
                         join board in db.DashboardsInfo on map.DashboardId equals board.Id
                         where map.RoleId == RoleId
                         select board;

            return Ok(boards.ToList());
        }

        private List<Widget> getWidgetList()
        {
            var Widgets = db.WidgetStructure.ToList();

            List<Widget> widgetsInfo = new List<Widget>();

            foreach (var widg in Widgets)
            {
                if (widg.ClassType == typeof(TileCard1).ToString() || widg.ClassType == typeof(TileCard1).AssemblyQualifiedName)
                {
                    TileCard1 w = (TileCard1)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    var result = db.RawSqlQuery(
                        ((TileCard1)w).Query,
                        x => new TileCard1() { Value = Convert.ToDecimal(x[0]), PerformanceValue = Convert.ToDecimal(x[1]) }
                        );
                    w.Value = result.FirstOrDefault().Value;
                    w.PerformanceValue = result.FirstOrDefault().PerformanceValue;
                    w.WidgetID = widg.ID;
                    

                    widgetsInfo.Add(w);
                }
                else if (widg.ClassType == typeof(TileCard2).ToString() || widg.ClassType == typeof(TileCard1).AssemblyQualifiedName)
                {
                    TileCard2 w = (TileCard2)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    var result = db.RawSqlQuery(
                        ((TileCard2)w).Query,
                        x => new TileCard2() { Count = Convert.ToDecimal(x[0]), Amount = Convert.ToDecimal(x[1]) }
                        );
                    w.Count = result.FirstOrDefault().Count;
                    w.Amount = result.FirstOrDefault().Amount;
                    w.WidgetID = widg.ID;

                    widgetsInfo.Add(w);
                }
            }
            return widgetsInfo;
        }

        private List<Widget> getWidgetListByID(int WidgetID)
        {
            var Widgets = db.WidgetStructure.Where(M => M.ID == WidgetID).ToList();

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
                    w.WidgetID = widg.ID;

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
                    w.WidgetID = widg.ID;

                    widgetsInfo.Add(w);
                }
            }
            return widgetsInfo;
        }

        private int CreateDashboard(DashboardViewModel dashboard)
        {
            try
            {
                var Board = new DashboardsInfo()
                {
                    Name = dashboard.Name,
                    ElementsCount = dashboard.SelectedElements.Split(',').Length
                };
                db.DashboardsInfo.Add(Board);
                db.SaveChanges();

                return Board.Id;
            }
            catch (System.Exception e)
            {
                return 0;
            }
        }

        private string UpdateDashboardName(DashboardViewModel dashboard)
        {
            try
            {
                var board = db.DashboardsInfo.Where(m => m.Id == dashboard.Id).FirstOrDefault();

                if (board != null && board.Name != dashboard.Name)
                {
                    db.Attach<DashboardsInfo>(board);

                    board.Name = dashboard.Name;
                    db.Entry(board).Property(p => p.Name).IsModified = true;
                    board.Name = dashboard.Name;

                    db.SaveChanges();
                }

                return "True";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        private string CreateDashboardElements(DashboardViewModel dashboard, int DashboardID)
        {
            var SelectedElements = dashboard.SelectedElements.Split(',').Select(Int32.Parse).ToList<int>();
            var DefaultedGadgets = dashboard.DefaultedElements.Split(',').Select(Int32.Parse).ToList<int>();
            try
            {
                if (SelectedElements != null && SelectedElements.Count() > 0)
                {
                    foreach (var item in SelectedElements)
                    {
                        db.DashboardLinkedElements.Add(new DashboardLinkedElements()
                        {
                            DashboardId = DashboardID,
                            WidgetID = item,
                            IsDefaultElement = false
                        });
                        db.SaveChanges();
                    }
                }
                if (DefaultedGadgets != null && DefaultedGadgets.Count() > 0)
                {
                    foreach (var item in DefaultedGadgets)
                    {
                        db.DashboardLinkedElements.Add(new DashboardLinkedElements()
                        {
                            DashboardId = DashboardID,
                            WidgetID = item,
                            IsDefaultElement = true
                        });
                        db.SaveChanges();
                    }
                }
                return "True";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }

        private string DeleteDashboardElements(int DashboardID)
        {
            try
            {
                var DashboardLinkedElements = db.DashboardLinkedElements.Where(m => m.DashboardId == DashboardID).ToList();

                foreach (var Element in DashboardLinkedElements)
                {
                    db.DashboardLinkedElements.Remove(Element);
                    db.SaveChanges();
                }
                return "True";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }
    }
}

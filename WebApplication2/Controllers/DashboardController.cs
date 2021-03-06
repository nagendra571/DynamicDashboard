using Dynamic_User_Defined_Dashboards.Models;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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

            List<DashboardLinkedElements> defaultDashboardLinkedElements = new List<DashboardLinkedElements>();
            List<DashboardLinkedElements> selectedDashboardLinkedElements = new List<DashboardLinkedElements>();
            List<DashboardLinkedElements> AllDashboardLinkedElements = new List<DashboardLinkedElements>();

            defaultDashboardLinkedElements = widgetListByDashboard.Where(m => m.IsDefaultElement == true).ToList();
            selectedDashboardLinkedElements = widgetListByDashboard.Where(m => m.IsDefaultElement == false).ToList().OrderBy(m => m.Position).ToList();
            selectedDashboardLinkedElements.ForEach(m => m.Position = m.Position + defaultDashboardLinkedElements.Count());

            AllDashboardLinkedElements.AddRange(defaultDashboardLinkedElements);
            AllDashboardLinkedElements.AddRange(selectedDashboardLinkedElements);

            List<Widget> widgets = new List<Widget>();

            foreach (var widg in AllDashboardLinkedElements)
            {
                widgets.AddRange(getWidgetList(widg.WidgetID, widg.Position ?? 0));
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
                ViewBag.DashboardId = board.Id;
            }

            List<Widget> widgets = getWidgetList();
            if (id > 0)
            {
                foreach (var widg in widgetListByDashboard)
                {
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().IsAccessble = true;
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().DashboardID = id;
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().IsDefaulted = widg.IsDefaultElement;
                    widgets.Where(m => m.WidgetID == widg.WidgetID).FirstOrDefault().Position = (int)widg.Position;
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

            var currentBoards = db.BusinessRoleDashboardMapping.Where(m => m.RoleId == mapping.RoleId).ToList();

            try
            {
                if (currentBoards != null && currentBoards.Count() > 0)
                {
                    foreach (var board in currentBoards)
                    {
                        db.BusinessRoleDashboardMapping.Remove(board);
                        db.SaveChanges();
                    }
                }
            }
            catch (System.Exception e)
            {
            }

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

        public IActionResult EditWidgets()
        {
            ViewBag.isEditWidget = true;
            return View("Elements/EditWidgets", getWidgetList());
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
            else if (TemplatedID == 2)
            {
                tileCard = new TileCard3()
                {
                    ElementTemplateID = TemplatedID,
                    TableAndColumns = viewsNames,
                    RequiredCaptureValues = true
                };
            }
            else if (TemplatedID == 5 || TemplatedID == 4 || TemplatedID == 3)
            {
                tileCard = new PieChart()
                {
                    ElementTemplateID = TemplatedID,
                    TableAndColumns = viewsNames,
                    RequiredCaptureValues = true
                };
            }
            return View("Widget/Index", tileCard);
        }

        public IActionResult EditWidget(int WidgetID)
        {
            var viewsNames = db.GetTableAndColumns();

            Widget tileCard = getWidgetList(WidgetID, 0, false).ToList().FirstOrDefault();
            tileCard.TableAndColumns = viewsNames;
            tileCard.RequiredCaptureValues = true;



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
                if (card.WidgetID > 0)
                {
                    obj.ID = card.WidgetID;
                    db.Entry(db.WidgetStructure.FirstOrDefault(x => x.ID == card.WidgetID)).CurrentValues.SetValues(obj);
                }
                else
                {
                    db.WidgetStructure.Add(obj);
                }

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
                if (card.WidgetID > 0)
                {
                    obj.ID = card.WidgetID;
                    db.Entry(db.WidgetStructure.FirstOrDefault(x => x.ID == card.WidgetID)).CurrentValues.SetValues(obj);
                }
                else
                {
                    db.WidgetStructure.Add(obj);
                }

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
        public IActionResult SaveTileCard3(TileCard3 card)
        {
            var enteredInfo = card;


            WidgetStructure obj = new WidgetStructure()
            {
                ElementID = card.ElementTemplateID,
                Formation = JsonSerializer.Serialize<TileCard3>(card),
                ClassType = card.GetType().ToString(),
                IsDeActivated = false
            };

            try
            {
                if (card.WidgetID > 0)
                {
                    obj.ID = card.WidgetID;
                    db.Entry(db.WidgetStructure.FirstOrDefault(x => x.ID == card.WidgetID)).CurrentValues.SetValues(obj);
                }
                else
                {
                    db.WidgetStructure.Add(obj);
                }

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
        public IActionResult SavePieChart(PieChart card)
        {
            var enteredInfo = card;

            WidgetStructure obj = new WidgetStructure()
            {
                ElementID = card.ElementTemplateID,
                Formation = JsonSerializer.Serialize<PieChart>(card),
                ClassType = card.GetType().ToString(),
                IsDeActivated = false
            };

            try
            {
                if (card.WidgetID > 0)
                {
                    obj.ID = card.WidgetID;
                    db.Entry(db.WidgetStructure.FirstOrDefault(x => x.ID == card.WidgetID)).CurrentValues.SetValues(obj);
                }
                else
                {
                    db.WidgetStructure.Add(obj);
                }

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

        private List<Widget> getWidgetList(int WidgetID = 0, int position = 0, bool ExecuteQuery = true)
        {
            List<WidgetStructure> Widgets;

            if (WidgetID > 0)
            {
                Widgets = db.WidgetStructure.Where(M => M.ID == WidgetID).ToList();
            }
            else
            {
                Widgets = db.WidgetStructure.ToList();
            }

            List<Widget> widgetsInfo = new List<Widget>();

            foreach (var widg in Widgets)
            {
                if (widg.ClassType == typeof(TileCard1).ToString() || widg.ClassType == typeof(TileCard1).AssemblyQualifiedName || widg.ClassType.Contains("tilecard1", StringComparison.OrdinalIgnoreCase))
                {
                    TileCard1 w = (TileCard1)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    if (ExecuteQuery)
                    {
                        var result = db.RawSqlQuery(
                            ((TileCard1)w).Query,
                            x => new TileCard1() { Value = Convert.ToDecimal(x[0]), PerformanceValue = Convert.ToDecimal(x[1]), AsOfDateValue = (x.FieldCount == 3 ? (DateTime?)x[2] : null) }
                            );
                        w.Value = result.FirstOrDefault().Value;
                        w.PerformanceValue = result.FirstOrDefault().PerformanceValue;
                        w.AsOfDateValue = result.FirstOrDefault().AsOfDateValue;
                    }
                    w.WidgetID = widg.ID;


                    widgetsInfo.Add(w);
                }
                else if (widg.ClassType == typeof(TileCard2).ToString() || widg.ClassType == typeof(TileCard2).AssemblyQualifiedName || widg.ClassType.Contains("tilecard2", StringComparison.OrdinalIgnoreCase))
                {
                    TileCard2 w = (TileCard2)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    if (ExecuteQuery)
                    {
                        var result = db.RawSqlQuery(
                        ((TileCard2)w).Query,
                        x => new TileCard2() { Count = Convert.ToDecimal(x[0]), Amount = Convert.ToDecimal(x[1]), AsOfDateValue = (x.FieldCount == 3 ? (DateTime?)x[2] : null) }
                        );
                        w.Count = result.FirstOrDefault().Count;
                        w.Amount = result.FirstOrDefault().Amount;
                        w.AsOfDateValue = result.FirstOrDefault().AsOfDateValue;
                    }
                    w.WidgetID = widg.ID;

                    widgetsInfo.Add(w);
                }
                else if (widg.ClassType == typeof(TileCard3).ToString() || widg.ClassType == typeof(TileCard3).AssemblyQualifiedName || widg.ClassType.Contains("tilecard3", StringComparison.OrdinalIgnoreCase))
                {
                    TileCard3 w = (TileCard3)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    if (ExecuteQuery)
                    {
                        var result = db.RawSqlQuery(
                        ((TileCard3)w).Query,
                        x => new TileCard3() { PlanedValue = Convert.ToDecimal(x[0]), ActualValue = Convert.ToDecimal(x[1]), AsOfDateValue = (x.FieldCount == 3 ? (DateTime?)x[2] : null) }
                        );
                        w.PlanedValue = result.FirstOrDefault().PlanedValue;
                        w.ActualValue = result.FirstOrDefault().ActualValue;
                        w.AsOfDateValue = result.FirstOrDefault().AsOfDateValue;
                    }
                    w.WidgetID = widg.ID;

                    widgetsInfo.Add(w);
                }
                else if (widg.ClassType == typeof(PieChart).ToString() || widg.ClassType == typeof(PieChart).AssemblyQualifiedName || widg.ClassType.Contains("PieChart", StringComparison.OrdinalIgnoreCase))
                {
                    PieChart w = (PieChart)WidgetsDataManager.PrepareData(widg);
                    w.IsRealValues = true;
                    w.RequiredCaptureValues = false;
                    w.RoleID = 1;
                    w.UserID = "nagendra.chinnam@otis.com";

                    if (ExecuteQuery)
                    {
                        var result = db.RawSqlQuery(
                        ((PieChart)w).Query,
                        x => new PieChart() { Category = x[0].ToString(), Value = (x[1]).ToString(), AsOfDateValue = (x.FieldCount == 3 ? (DateTime?)x[2] : null) }
                        );

                        var records = from record in result select new PieChartRecord() { Category = record.Category, Value = record.Value };

                        w.Data = records;
                        w.AsOfDateValue = result.FirstOrDefault().AsOfDateValue;
                    }

                    w.WidgetID = widg.ID;

                    widgetsInfo.Add(w);
                }
            }

            if (widgetsInfo != null)
            {
                widgetsInfo.ForEach(m => m.Position = position);
            }

            return widgetsInfo;
        }

        private List<Widget> getWidgetListByID1(int WidgetID)
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
                int normalElementsCount = (dashboard != null && dashboard.SelectedElements != null) ? dashboard.SelectedElements.Split(',').Length : 0;
                int defaultElementsCount = (dashboard != null && dashboard.DefaultedElements != null) ? dashboard.DefaultedElements.Split(',').Length : 0;

                if (normalElementsCount + defaultElementsCount > 0)
                {
                    var Board = new DashboardsInfo()
                    {
                        Name = dashboard.Name,
                        ElementsCount = normalElementsCount + defaultElementsCount
                    };
                    db.DashboardsInfo.Add(Board);
                    db.SaveChanges();

                    return Board.Id;
                }
                else
                {
                    return 0;
                }
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
            var SelectedElements = (dashboard != null && dashboard.SelectedElements != null) ? dashboard.SelectedElements.Split(',').Select(Int32.Parse).ToList<int>() : new List<int>();
            var DefaultedGadgets = (dashboard != null && dashboard.DefaultedElements != null) ? dashboard.DefaultedElements.Split(',').Select(Int32.Parse).ToList<int>() : new List<int>();
            try
            {
                if (1 == 2)
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
                }
                else
                {
                    if (dashboard != null && dashboard.SelectedElementsWithOrder != null)
                    {
                        foreach (var item in dashboard.SelectedElementsWithOrder)
                        {
                            db.DashboardLinkedElements.Add(item);
                            db.SaveChanges();
                        }
                    }
                    if (dashboard != null && dashboard.DefaultedElementsWithOrder != null)
                    {
                        foreach (var item in dashboard.DefaultedElementsWithOrder)
                        {
                            db.DashboardLinkedElements.Add(item);
                            db.SaveChanges();
                        }
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

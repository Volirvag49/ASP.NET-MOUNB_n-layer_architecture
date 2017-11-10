using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Interfaces;
using MOUNB.BLL.Infrastructure;
using AutoMapper;
using PagedList;
using MOUNB.WEB.Models;

namespace MOUNB.WEB.Controllers
{
    public class UsersController : Controller
    {
        IUserService userService;

        public UsersController(IUserService serv)
        {
            userService = serv;
        }

        [HttpGet]
        public ActionResult Index(string sortOrder, string searchString, string searchSelection, string currentFilter, string currentSelection, int? page, int? pageSize)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LoginSortParm = sortOrder == "Login" ? "Login_desc" : "Login";
            ViewBag.PositionSortParm = sortOrder == "Position" ? "Position_desc" : "Position";
            ViewBag.RoleSortParm = sortOrder == "Role" ? "Role_desc" : "Role";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
                searchSelection = currentSelection;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSelection = searchSelection;
            ViewBag.CurrentPageSize = pageSize;

            int currentPage = (page ?? 1);
            pageSize = (pageSize ?? 5);
            StaticPagedList<UserDTO> usersDTOPaged = userService.GetPagedUsers(sortOrder, searchString, searchSelection, currentPage, pageSize);

            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var users = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(usersDTOPaged);
            var usersPaged = new StaticPagedList<UserViewModel>(users, usersDTOPaged.GetMetaData());

            return View(usersPaged);
        }

        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                    UserDTO user = Mapper.Map<UserViewModel, UserDTO>(model);
                    userService.RegisterUser(user);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);

                }
            }
            return View(model);
        } 

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = userService.GetUserById(id.Value);

            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());

            UserViewModel user = Mapper.Map<UserDTO, UserViewModel>(userDTO);

            return View(user);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = userService.GetUserById(id.Value);

            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());

            UserViewModel user = Mapper.Map<UserDTO, UserViewModel>(userDTO);

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                    UserDTO user = Mapper.Map<UserViewModel, UserDTO>(model);
                    userService.EditUser(user);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);

                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = userService.GetUserById(id.Value);
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            UserViewModel user = Mapper.Map<UserDTO, UserViewModel>(userDTO);

            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserViewModel model)
        {

            Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
            UserDTO user = Mapper.Map<UserViewModel, UserDTO>(model);
            userService.DeleteUser(user);

            return RedirectToAction("Index");
        }
    }
}
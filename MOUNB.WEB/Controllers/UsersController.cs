using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Interfaces;
using MOUNB.BLL.Infrastructure;
using AutoMapper;
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
        public ActionResult Index()
        {
            IEnumerable<UserDTO> usersDTO = userService.GetAllUsers();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var users = Mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(usersDTO);
            return View(users);
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
                Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                UserDTO user = Mapper.Map<UserViewModel, UserDTO>(model);
                userService.RegisterUser(user);
                return RedirectToAction("Index");
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
                Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                UserDTO user = Mapper.Map<UserViewModel, UserDTO>(model);
                userService.EditUser(user);
                return RedirectToAction("Index");
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
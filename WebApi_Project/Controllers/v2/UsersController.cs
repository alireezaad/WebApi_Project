using Application.Models.UserModels;
using Application.UseCases.Managers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_Project.Controllers.v2
{
    [Authorize]
    [ApiController]
    //Client should send api version in queryString
    [Route("api/v{version:apiversion}/[Controller]")] // e.x: api/v2/Users/Get
    [ApiVersion("2")]
    public class UsersController : v1.UsersController
    {
        private readonly IUserUseCaseManager _userUseCaseManager;
        private readonly LinkGenerator _linkGenerator;
        public UsersController(IUserUseCaseManager userManager, LinkGenerator linkGenerator) : base(userManager, linkGenerator)
        {
            _userUseCaseManager = userManager;
            _linkGenerator = linkGenerator;
        }
        /// <summary>
        /// Get all users with their tasks
        /// </summary>
        /// <returns>an IEnumerable of users</returns>
        public override async Task<IEnumerable<UserGetModel>> Get()
        {
            //if we wanted to use base method:
            //return base.Get
            //else
            return await _userUseCaseManager.GetAllUserUC.ExecuteAsync(u=> u.Tasks);
        }

        // GET: UsersController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: UsersController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: UsersController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UsersController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsersController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UsersController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UsersController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UsersController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}

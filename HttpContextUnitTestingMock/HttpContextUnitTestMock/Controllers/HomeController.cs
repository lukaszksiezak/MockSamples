using HttpContextUnitTestMock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HttpContextUnitTestMock.Controllers
{
    /// <summary>
    /// Scenario: A controller gets the data from some source: database, service, etc. 
    /// Data is read once and then, it is stored in session.
    /// To test the function 'DoActionWithTheDataFromSession" it is actually needed to get the data from session, and overrite it with modified one.
    /// The purpose of the unit test for this class is to have a reference how to mock session object to exclude it from the scope of uni testing DoActionWithTheDataFromSession
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        //The simple task for that method is to modify session's collection and assign all given users as Apple's emploees and put it to session again
        public void DoActionWithTheDataFromSession(string sessionIndex)
            {
            var listOfUsers = Session[sessionIndex] as List<User>;
            //Fancy looking way of conditional value changing of the property in one line using pure linq
            listOfUsers = listOfUsers.Select(u => { if (u.Company != "Apple") u.Company = "Apple";  return u; }).ToList();
            this.Session[sessionIndex] = listOfUsers;             
            }
        }
}
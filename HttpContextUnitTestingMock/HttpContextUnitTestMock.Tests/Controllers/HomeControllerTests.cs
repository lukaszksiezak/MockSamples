using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using HttpContextUnitTestMock.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace HttpContextUnitTestMock.Controllers.Tests
    {
    /// <summary>
    /// Basically, this class is showing how to mock session object in mvc unit testing process.
    /// Worth noticing is how the 'mock' httpContext is assigned to 'mock' controller
    /// </summary>
    [TestClass()]
    public class HomeControllerTests
        {
        [TestInitialize]
        public void TestSetup()
            {
            // Setting up the Current HTTP Context            

            // Setup the HTTP Request
            var httpRequest = new HttpRequest("", "http://localhost/home/index", "");

            // Setup the HTTP Response
            var httpResponce = new HttpResponse(new StringWriter());

            // Setup the Http Context
            var httpContext = new HttpContext(httpRequest, httpResponce);
            var sessionContainer =
                new HttpSessionStateContainer("id",
                                               new SessionStateItemCollection(),
                                               new HttpStaticObjectsCollection(),
                                               10,
                                               true,
                                               HttpCookieMode.AutoDetect,
                                               SessionStateMode.InProc,
                                               false);
            httpContext.Items["AspSession"] =
                typeof(HttpSessionState)
                .GetConstructor(
                                    BindingFlags.NonPublic | BindingFlags.Instance,
                                    null,
                                    CallingConventions.Standard,
                                    new[] { typeof(HttpSessionStateContainer) },
                                    null)
                .Invoke(new object[] { sessionContainer });

            // Assign the Context
            HttpContext.Current = httpContext;
            }

        private List<User> generateUsers()
            {
            User user1 = new User() { Name = "x", Email = "x", Company = "Apple" };
            User user2 = new User() { Name = "y", Email = "y", Company = "Microsoft" };
            User user3 = new User() { Name = "z", Email = "z", Company = "Apple" };

            var list = new List<User>();
            list.AddMany<User>(user1, user2, user3);

            return list;            
            }

        [TestMethod()]
        public void DoActionWithTheDataFromSessionTest()
            {
        //Arrange:
            //Sessionfy users list with index "fakeUsers"
            HttpContext.Current.Session["fakeUsers"] = generateUsers();
            //Create HttpContextWrapper to prepare mock environment for HomeController
            var wrapper = new HttpContextWrapper(HttpContext.Current);
            //Create instance of home controller
            var homeController = new HomeController();
            //Assign apropriate context to controller
            homeController.ControllerContext = new ControllerContext(wrapper, new RouteData(), homeController);
            
        //Act
            homeController.DoActionWithTheDataFromSession("fakeUsers");                        
            var modifiedUsersList = HttpContext.Current.Session["fakeUsers"] as List<User>;
        
        //Assert
            Assert.IsNotNull(modifiedUsersList);
            Assert.AreEqual(3, modifiedUsersList.Count);
            Assert.IsTrue(modifiedUsersList.TrueForAll(m => m.Company == "Apple")); //Actual assertion 
            }
        }
    }
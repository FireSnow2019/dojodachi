using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    {
       
        public Int32 _FormDataFullness { get; set; }
        public Int32 _FormDataHappiness { get; set; }
        public Int32 _FormDataMeal { get; set; }
        public Int32 _FormDataEnergy { get; set; }
        public int result = 0;

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("SessionMeal", 3);
            HttpContext.Session.SetInt32("SessionFullness",20);
            HttpContext.Session.SetInt32("SessionHappiness", 20);
            HttpContext.Session.SetInt32("SessionEnergy", 50);


            ViewBag.Fullness = 20;
            ViewBag.Happiness = 20;
            ViewBag.Meal = 3;
            ViewBag.Energy = 50;
            return View();
        }

        [HttpPostAttribute]
        [RouteAttribute("")]
        public IActionResult Index(string submit)
        {
            _FormDataFullness = (Int32) HttpContext.Session.GetInt32("SessionFullness");
            _FormDataEnergy = (Int32) HttpContext.Session.GetInt32("SessionEnergy");
            _FormDataHappiness = (Int32) HttpContext.Session.GetInt32("SessionHappiness");
            _FormDataMeal = (Int32) HttpContext.Session.GetInt32("SessionMeal");

            if (submit == "Feed")
            {
                result = PetFeed();
                if(result == 0)
                {
                    ViewBag.ErrorMsg = "Need Food";
                }
            }

            if(submit == "Play")
            {
                result = play();

                if(result == 0)
                {
                    ViewBag.ErrorMsg = "Not in mood!";
                } 

                if(HttpContext.Session.GetString("SessionHappy") == "No")
                {
                    ViewBag.Happy = "I am not happy by this. For God sake..leave me alone";
                }
                else
                {
                    ViewBag.Happy = "Yey... more ... PLEASE";   
                }
            }

            if(submit=="Work")
            {
                result = Work();
                if(result == 0)
                {
                    ViewBag.ErrorMsg = "Need energy to play";
                }
                else
                {
                    ViewBag.ErrorMsg = string.Empty;
                }
            }

            if(submit=="Sleep")
            {
                result = Sleep();
            }

            ViewBag.Fullness = HttpContext.Session.GetInt32("SessionFullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32("SessionHappiness");
            ViewBag.Meal = HttpContext.Session.GetInt32("SessionMeal");
            ViewBag.Energy = HttpContext.Session.GetInt32("SessionEnergy");
            
            ViewBag.FormCalled = submit;

            //Error message
            
            
            return View();
        }

        public int PetFeed()
        {
            if (_FormDataMeal > 0)
            {
                _FormDataMeal = _FormDataMeal - 1;
            
            _FormDataFullness = RandNumber(5, 10);

            HttpContext.Session.SetInt32("SessionMeal", _FormDataMeal);
            HttpContext.Session.SetInt32("SessionFullness",_FormDataFullness);
                return 1;
            }
            else
            {
                return 0;
            }

            //feed it
            //("Meal",_FormDataMeal);
            //TempData.Add("Meal",_FormDataMeal);

            //I thought having dictionary before putting it in session would be easy thing to do. but I found out that
            //mydictionary needs to be serialized before it can go into session which means lots of over heads. Because of that
            //I would not use mydictionary in my session object. 
            /*
            Dictionary<string, int> mydictionary = new Dictionary<string, int>();
            mydictionary.Add("SessionMeal", _FormDataMeal);
            mydictionary.Add("SessionFullness", _FormDataFullness);
            HttpContext.Session("MyformData", mydictionary);
            */
        }

        //PetPlay
        public int play()
        {
            if(_FormDataEnergy >0 && _FormDataMeal > 0)
            {
                _FormDataEnergy = _FormDataEnergy - 5;
                _FormDataHappiness = ((_FormDataHappiness * 25) / 100);
                _FormDataMeal = _FormDataMeal - 1;
                if (_FormDataHappiness < 25)
                    HttpContext.Session.SetString("SessionHappy", "No");
                else
                    HttpContext.Session.SetString("SessionHappy", "Yes");
                //set session
                HttpContext.Session.SetInt32("SessionMeal", _FormDataMeal);
                HttpContext.Session.SetInt32("SessionEnergy",_FormDataEnergy);
                
                return 1;
            }
            else
            { 
                return 0;
            }   
        }

        public int Work()
        {
            //cost 5 energy 
            //earn 1-3 random Meal
            if(_FormDataEnergy >= 5)
            {
                _FormDataEnergy = _FormDataEnergy - 5;
                _FormDataMeal = _FormDataMeal + RandNumber(1, 3);
                HttpContext.Session.SetInt32("SessionMeal", _FormDataMeal);
                HttpContext.Session.SetInt32("SessionEnergy",_FormDataEnergy);

                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Sleep()
        {
            //sleep will increase energy by 15
            //fullness - 5 and Happiness - 5
            _FormDataEnergy = _FormDataEnergy + 15;
            _FormDataFullness = _FormDataFullness - 5;
            _FormDataHappiness = _FormDataHappiness - 5;

            HttpContext.Session.SetInt32("SessionEnergy", _FormDataEnergy);
            HttpContext.Session.SetInt32("SessionFullness",_FormDataFullness);
            HttpContext.Session.SetInt32("SessionHappiness", _FormDataHappiness);
            
            return 1;

        }

        public int RandNumber(int min, int max)
        {
            int rannumber = 0;
            if (max == 0)
            {
                max = 10;
            }

            Random rand = new Random();
            rannumber = rand.Next(min, max);
            return rannumber;
        }
    }
}

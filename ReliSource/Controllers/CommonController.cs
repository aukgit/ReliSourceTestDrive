﻿using System.Linq;
using System.Web.Mvc;
using ReliSource.Filter;
using ReliSource.Modules.Cache;
using ReliSource.Modules.Session;

namespace ReliSource.Controllers {
    //[CompressFilter]
    public class CommonController : Controller {
        [OutputCache(CacheProfile = "Day", VaryByParam = "id")]
        public ActionResult GetTimeZone(int id) {
            if (SessionNames.IsValidationExceed("GetTimeZone", 100)) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var getZones = CachedQueriedData.GetTimezones(id);
            if (getZones != null) {
                var represent = getZones.Select(n => new { text = n.Display, id = n.UserTimeZoneID });
                return Json(represent.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(CacheProfile = "Day", VaryByParam = "id")]
        public ActionResult GetLanguage(int id) {
            if (SessionNames.IsValidationExceed("GetLanguage", 100)) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var languges = CachedQueriedData.GetLanguages(id);
            if (languges != null) {
                var represent =
                    languges.Select(n => new { text = n.Language + " - " + n.NativeName, id = n.CountryLanguageID });
                return Json(represent.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
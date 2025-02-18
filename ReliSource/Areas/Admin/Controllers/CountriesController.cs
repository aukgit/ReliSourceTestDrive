﻿using System;
using System.Linq;
using System.Web.Mvc;
using ReliSource.Models.Context;
using ReliSource.Models.POCO.IdentityCustomization;
using ReliSource.Modules.Cache;

namespace ReliSource.Areas.Admin.Controllers {
    public class CountriesController : Controller {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index() {
            return View(CachedQueriedData.GetCountries().ToList());
        }

        public ActionResult Edit(Int32 id) {
            var zones = CachedQueriedData.GetTimezones(id);
            ViewBag.Timezone = new SelectList(_db.UserTimeZones.ToList(), "UserTimeZoneID", "Display");
            ViewBag.CountryID = id;
            ViewBag.CountryName = _db.Countries.Find(id).DisplayCountryName + " - " + _db.Countries.Find(id).Alpha2Code;
            return View(zones);
        }

        public ActionResult Delete(Int32 id) {
            var timezone = _db.UserTimeZones.Find(id);
            _db.UserTimeZones.Remove(timezone);
            _db.SaveChanges();
            return RedirectToActionPermanent("Edit", new { id });
        }

        [HttpPost]
        public ActionResult Edit(int countryId, int timezone, bool hasMultiple) {
            var country = _db.Countries.Find(countryId);

            var foundTimeZone = _db.UserTimeZones.Find(timezone);
            if (foundTimeZone != null) {
                var addRelation = new CountryTimezoneRelation {
                    CountryID = country.CountryID,
                    UserTimeZoneID = foundTimeZone.UserTimeZoneID
                };
                var anyExist =
                    _db.CountryTimezoneRelations.Any(
                        n => n.UserTimeZoneID == addRelation.UserTimeZoneID && n.CountryID == addRelation.CountryID);

                if (!anyExist) {
                    //not exist then add
                    _db.CountryTimezoneRelations.Add(addRelation);
                    country.RelatedTimeZoneID = addRelation.UserTimeZoneID;
                }

                country.IsSingleTimeZone = !hasMultiple;
                country.RelatedTimeZoneID = addRelation.UserTimeZoneID;
                _db.SaveChanges();
            }
            var zones = CachedQueriedData.GetTimezones(countryId);
            ViewBag.Timezone = new SelectList(_db.UserTimeZones.ToList(), "UserTimeZoneID", "Display");
            ViewBag.CountryID = countryId;
            ViewBag.CountryName = _db.Countries.Find(countryId).DisplayCountryName + " - " +
                                  _db.Countries.Find(countryId).Alpha2Code;

            return View(zones);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
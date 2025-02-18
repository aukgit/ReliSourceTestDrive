﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReliSource.Controllers;
using DevTrends.MvcDonutCaching;
using ReliSource.Models.EntityModel;

namespace ReliSource.Controllers
{
    public class SchoolClassesController : GenericController<SchoolEntities> {

		#region Developer Comments - Alim Ul karim
        /*
         *  Generated by Alim Ul Karim on behalf of Developers Organism.
         *  Find us developers-organism.com
         *  https://fb.com/DevelopersOrganism
         *  mailto:alim@developers-organism.com	
         *  Google 'https://www.google.com.bd/search?q=Alim-ul-karim'
         *  First Written : 23 March 2014
         *  Modified      : 03 March 2015
         * * */
		#endregion

		#region Constants and variables

		const string DeletedError = "Sorry for the inconvenience, last record is not removed. Please be in touch with admin.";
		const string DeletedSaved = "Removed successfully.";
		const string EditedSaved = "Modified successfully.";
		const string EditedError = "Sorry for the inconvenience, transaction is failed to save into the database. Please be in touch with admin.";
		const string CreatedError = "Sorry for the inconvenience, couldn't create the last transaction record.";
		const string CreatedSaved = "Transaction is successfully added to the database.";
		const string ControllerName = "SchoolClasses";
		///Constant value for where the controller is actually visible.
		const string ControllerVisibleUrl = "/SchoolClasses/";
        const string CurrentControllerRemoveOutputCacheUrl = "/Partials/GetSchoolClassID";
        const string DynamicLoadPartialController = "/Partials/";
        bool DropDownDynamic = true;
		#endregion

		#region Enums

		internal enum ViewStates {
            Index,
            Create,
            CreatePostBefore,
            CreatePostAfter,
            Edit,
            EditPostBefore,
            EditPostAfter,
            Details,
            Delete,
            DeletePost
        }

		#endregion

		#region Constructors
		
		public SchoolClassesController(): base(true){
			ViewBag.controller = ControllerName;
            ViewBag.visibleUrl = ControllerVisibleUrl;
            ViewBag.dropDownDynamic = DropDownDynamic;
            ViewBag.dynamicLoadPartialController = DynamicLoadPartialController;
		} 

		#endregion
		
		#region View tapping
		/// <summary>
        /// Always tap once before going into the view.
        /// </summary>
        /// <param name="view">Say the view state, where it is calling from.</param>
        /// <param name="schoolClass">Gives the model if it is a editing state or creating posting state or when deleting.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
		bool ViewTapping(ViewStates view, SchoolClass schoolClass = null, bool entityValidState = true){
			switch (view){
				case ViewStates.Index:
					break;
				case ViewStates.Create:
					break;
				case ViewStates.CreatePostBefore: // before saving it
					break;
                case ViewStates.CreatePostAfter: // after saving
					break;
				case ViewStates.Edit:
					break;
				case ViewStates.Details:
					break;
				case ViewStates.EditPostBefore: // before saving it
					break;
                case ViewStates.EditPostAfter: // after saving
					break;
				case ViewStates.Delete:
					break;
			}
			return true;
		}
		#endregion

		#region Save database common method

		/// <summary>
        /// Better approach to save things into database(than db.SaveChanges()) for this controller.
        /// </summary>
        /// <param name="view">Say the view state, where it is calling from.</param>
        /// <param name="schoolClass">Your model information to send in email to developer when failed to save.</param>
        /// <returns>If successfully saved returns true or else false.</returns>
		bool SaveDatabase(ViewStates view, SchoolClass schoolClass = null){
			// working those at HttpPost time.
			switch (view){
				case ViewStates.Create:
					break;
				case ViewStates.Edit:
					break;
				case ViewStates.Delete:
					break;
			}

			try	{                
				var changes = db.SaveChanges(schoolClass);
				if(changes > 0){
                    RemoveOutputCacheOnIndex();
                    RemoveOutputCache(CurrentControllerRemoveOutputCacheUrl);

                    var indexUrl = ControllerVisibleUrl + "Index";
                    RemoveOutputCache(indexUrl);
                    RemoveOutputCache("/" + ControllerName);
					return true;
				}
			} catch (Exception ex){
				 throw new Exception("Message : " + ex.Message.ToString() + " Inner Message : " + ex.InnerException.Message.ToString());
			}
			return false;
		}
		#endregion

		#region DropDowns Generate

        #region SchoolClassesController : DropDowns to paste into the partial
            
            // [DonutOutputCache(CacheProfile = "YearNoParam")]
            public JsonResult GetSchoolID() {
                var data = db.Schools.Select(n => new {id = n.SchoolID, display = n.SchoolName}).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
      
        #endregion

		public void GetDropDowns(SchoolClass schoolClass = null){
			if(schoolClass != null){
				ViewBag.SchoolID = new SelectList(db.Schools.ToList(), "SchoolID", "SchoolName", schoolClass.SchoolID);
			} else {			
				ViewBag.SchoolID = new SelectList(db.Schools.ToList(), "SchoolID", "SchoolName");
			}
			
		}

		public void GetDropDowns(System.Int32 id){			
				ViewBag.SchoolID = new SelectList(db.Schools.ToList(), "SchoolID", "SchoolName");
		}
		#endregion

		#region Index
        [OutputCache(CacheProfile = "Year")]
        public ActionResult Index() { 
        
            var schoolClasses = db.SchoolClasses.Include(s => s.School);
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(schoolClasses.ToList());
        }
		#endregion

		#region Index Find - Commented
		/*
        [OutputCache(CacheProfile = "Year")]
        public ActionResult Index(System.Int32 id) {
            var schoolClasses = db.SchoolClasses.Include(s => s.School).Where(n=> n. == id);
			bool viewOf = ViewTapping(ViewStates.Index);
            return View(schoolClasses.ToList());
        }
		*/
		#endregion

		#region Details
        public ActionResult Details(System.Int32 id) {
        
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schoolClass = db.SchoolClasses.Find(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
			bool viewOf = ViewTapping(ViewStates.Details, schoolClass);
            return View(schoolClass);
        }
		#endregion

		#region Create or Add
        public ActionResult Create() {        
			if(DropDownDynamic == false){
                GetDropDowns();
            }
			bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }

		/*
		public ActionResult Create(System.Int32 id) {        
			if(DropDownDynamic == false){
                GetDropDowns(id);// Generate hidden.
            }
			bool viewOf = ViewTapping(ViewStates.Create);
            return View();
        }
		*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SchoolClass schoolClass) {
			bool viewOf = ViewTapping(ViewStates.CreatePostBefore, schoolClass);
			if(DropDownDynamic == false){
                GetDropDowns(schoolClass);
            }
            if (ModelState.IsValid) {            
                db.SchoolClasses.Add(schoolClass);
                bool state = SaveDatabase(ViewStates.Create, schoolClass);
				if (state) {			
					AppVar.SetSavedStatus(ViewBag, CreatedSaved); // Saved Successfully.
				} else {					
					AppVar.SetErrorStatus(ViewBag, CreatedError); // Failed to save
				}
				
                viewOf = ViewTapping(ViewStates.CreatePostAfter, schoolClass,state);
                return View(schoolClass);
            }
            viewOf = ViewTapping(ViewStates.CreatePostAfter, schoolClass, false);			
			AppVar.SetErrorStatus(ViewBag, CreatedError); // record is not valid for creation
            return View(schoolClass);
        }
		#endregion

        #region Edit or modify record
        public ActionResult Edit(System.Int32 id) {
        
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schoolClass = db.SchoolClasses.Find(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
			bool viewOf = ViewTapping(ViewStates.Edit, schoolClass);
			if(DropDownDynamic == false){
                GetDropDowns(schoolClass); // Generating drop downs
            }
            return View(schoolClass);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SchoolClass schoolClass) {
			bool viewOf = ViewTapping(ViewStates.EditPostBefore, schoolClass);
            if (ModelState.IsValid)
            {
                db.Entry(schoolClass).State = EntityState.Modified;
                bool state = SaveDatabase(ViewStates.Edit, schoolClass);
				if (state) {
                    AppVar.SetSavedStatus(ViewBag, EditedSaved); // Saved Successfully.
				} else {					
					AppVar.SetErrorStatus(ViewBag, EditedError); // Failed to Save
				}
				
                viewOf = ViewTapping(ViewStates.EditPostAfter, schoolClass , state);
                return RedirectToAction("Index");
            }
            viewOf = ViewTapping(ViewStates.EditPostAfter, schoolClass , false);
        	if(DropDownDynamic == false){
                GetDropDowns(schoolClass); // Generating drop downs
            }
            AppVar.SetErrorStatus(ViewBag, EditedError); // record not valid for save
            return View(schoolClass);
        }
		#endregion

		#region Delete or remove record

		
        public ActionResult Delete(int id) {
        
            var schoolClass = db.SchoolClasses.Find(id);
            bool viewOf = ViewTapping(ViewStates.Delete, schoolClass);
			return View(schoolClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]		
        public ActionResult DeleteConfirmed(int id) {
            var schoolClass = db.SchoolClasses.Find(id);
			bool viewOf = ViewTapping(ViewStates.DeletePost, schoolClass);
            db.SchoolClasses.Remove(schoolClass);
            bool state = SaveDatabase(ViewStates.Delete, schoolClass);
			if (!state) {			
				AppVar.SetErrorStatus(ViewBag, DeletedError); // Failed to Save				
                return View(schoolClass);
			}
			
            return RedirectToAction("Index");
        }
		#endregion

		#region Removing output cache
		public void RemoveOutputCache(string url) {
			HttpResponse.RemoveOutputCacheItem(url);
		}
        
        public void RemoveOutputCacheOnIndex() {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(ControllerName, "Index");
            cacheManager.RemoveItems(ControllerName, "List");
            cacheManager = null;
            GC.Collect();
        }
		#endregion
    }

	
}

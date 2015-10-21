namespace Zanshin.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Zanshin.Domain.Entities.Forum;

    /// <summary>
    /// </summary>
    [Authorize]
    public class ForumsController : Controller
    {
        // GET: Admin/ForumAdmin
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/ForumAdmin/Details/5
        /// <summary>
        /// Details es the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/ForumAdmin/Create
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Forum forum = new Forum();
            return View(forum);
        }

        /// <summary>
        /// Creates the specified collection.
        /// </summary>
        /// <param name="forum">The forum.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Forum forum)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/ForumAdmin/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/ForumAdmin/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/ForumAdmin/Delete/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/ForumAdmin/Delete/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
﻿//----------------------------------------------------------------------
// <copyright file="ActivityCategoriesController.cs" company="17NSJ PR Dept">
// Copyright (c) 17NSJ PR Dept. All rights reserved.
// </copyright>
// <summary>ActivityCategoriesControllerクラス</summary>
//----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using _17nsj.DataAccess;

namespace _17nsj.Service.Controllers
{
    /// <summary>
    /// ActivityCategoriesControllerクラス
    /// </summary>
    [Authorize]
    [RoutePrefix("api/activity_categories")]
    public class ActivityCategoriesController : ControllerBase
    {
        /// <summary>
        /// アクティビティカテゴリ情報を取得します。
        /// </summary>
        /// <returns>HTTPレスポンス</returns>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            if (!this.CanRead())
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            using (Entities entitiies = new Entities())
            {
                var activityCategories = entitiies.ActivityCategories.ToList();

                if (activityCategories != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, activityCategories);
                }
                else
                {
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found");
                }
            }
        }
    }
}

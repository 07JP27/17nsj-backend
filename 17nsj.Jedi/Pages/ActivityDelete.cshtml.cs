﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _17nsj.Jedi.Constants;
using _17nsj.Jedi.Domains;
using _17nsj.Jedi.Extensions;
using _17nsj.Jedi.Models;
using _17nsj.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _17nsj.Jedi.Pages
{
    [Authorize(Roles = UserRoleDomain.SysAdmin)]
    public class ActivityDeleteModel : PageModelBase
    {
        private ILogger _logger;

        public ActivityDeleteModel(JediDbContext dbContext, ILogger<ActivityDeleteModel> logger)
            : base(dbContext)
        {
            _logger = logger;
        }

        [BindProperty]
        public ActivityModel CurrentAct { get; set; }

        public async Task<IActionResult> OnGetAsync(string category, int? id)
        {
            if (category == null || id == null) return new RedirectResult("/NotFound");

            this.PageInitializeAsync();

            var news = await this.DBContext.Activities.Where(x => x.Category == category && x.Id == (int)id && x.IsAvailable == true).FirstOrDefaultAsync();

            if (news == null) return new RedirectResult("/NotFound");

            this.CurrentAct = new ActivityModel();
            CurrentAct.Category = news.Category;
            CurrentAct.Id = news.Id;
            CurrentAct.Title = news.Title;
            CurrentAct.UpdatedAt = news.UpdatedAt;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var tran = await this.DBContext.Database.BeginTransactionAsync())
            {
                if (string.IsNullOrEmpty(CurrentAct.Category) || CurrentAct.Id == 0) return new RedirectResult("/NotFound");

                this.PageInitializeAsync();

                var news = await this.DBContext.Activities.Where(x => x.Category == CurrentAct.Category && x.Id == CurrentAct.Id).FirstOrDefaultAsync();
                if (news == null)
                {
                    //対象なしエラー
                    this.MsgCategory = MsgCategoryDomain.Error;
                    this.Msg = メッセージ.選択対象なし;
                    return this.Page();
                }

                // 既更新チェック
                if (news.UpdatedAt.TruncMillSecond() != CurrentAct.UpdatedAt)
                {
                    this.MsgCategory = MsgCategoryDomain.Error;
                    this.Msg = メッセージ.既更新;
                    return this.Page();
                }

                news.IsAvailable = false;
                news.UpdatedAt = DateTime.UtcNow;
                news.UpdatedBy = this.UserID;

                try
                {
                    await this.DBContext.SaveChangesAsync();
                    tran.Commit();
                    _logger.LogInformation($"【プログラム削除】ユーザー：{this.UserID}　対象：{this.CurrentAct.Category}-{this.CurrentAct.Id}");
                    return new RedirectResult("/ActivityList");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex, $"【プログラム削除エラー】ユーザー：{this.UserID}　対象：{this.CurrentAct.Category}-{this.CurrentAct.Id}");
                    this.MsgCategory = MsgCategoryDomain.Error;
                    this.Msg = ex.Message;
                    return this.Page();
                }
            }
        }
    }
}